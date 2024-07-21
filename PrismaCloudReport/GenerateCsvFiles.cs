using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PrismaCloudReport.Dto;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PrismaCloudReport
{
    public class GenerateCsvFiles
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public GenerateCsvFiles(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GenerateCsvFiles>();
            _configuration = configuration;
        }

        [Function("GenerateCsvFiles")]
        //public void Run([TimerTrigger("0 0 */1 * * *"
        public void Run([TimerTrigger("0 0 */2 1 * *"
#if DEBUG
            , RunOnStartup = true
#endif
            )] object timer)
        {
            _logger.LogInformation($"GenerateCsvFiles trigger function started executed at: {DateTime.Now}");

            GenerateCsv();

            _logger.LogInformation($"GenerateCsvFiles trigger function ended execution at: {DateTime.Now}");
            _logger.LogDebug($"Function data: {timer}");

        }

        private void GenerateCsv()
        {
            PrismaClient prisma = null;

            try
            {
                _logger.LogInformation("Initialize Prisma Cloud client...");
                prisma = GetPrismaClient();
                var helper = new PrismaHelper(prisma);

                var timeDef = new TimeDefinition
                {
                    TimeType = TimeType.absolute,
                    StartTime =  new DateTimeOffset(2018, 8, 29, 0, 0, 0, TimeSpan.Zero),
                    EndTime = DateTimeOffset.Now,
                };
#if DEBUG
                timeDef.EndTime = new DateTimeOffset(timeDef.EndTime.Year, timeDef.EndTime.Month, 1, 0, 0, 0, timeDef.EndTime.Offset);
#endif
                var prefix = GetReportDate(timeDef.EndTime).ToString("yyMM");

                _logger.LogInformation("Initialize blob storage client...");
                var storageUri = _configuration["StorageUri"];
                var blobClient = new BlobStorageClient(_logger, storageUri);

                _logger.LogInformation("Getting compliance standards...");
                var standards = helper.GetNNComplianceStandards();
                if (standards == null)
                    return;

                _logger.LogInformation("Checking if 'alerts by policy' has already been generated...");
                AlertPolicyDataDto alertByPolicyData = null;
                var alertFileExist = blobClient.FileHasBeenModifiedToday("AlertByPolicy.csv");
                //var alertFileExist = false;
                if (!alertFileExist)
                {
                    var alertData = GetAlertsByPolicy(prisma, helper, timeDef, standards);
                    alertByPolicyData = alertData.AlertPolicies;

                    _logger.LogInformation("Uploading 'alerts by policy' csv files...");
                    var alertByPolicy = string.Join("\r\n", alertData.AlertByPolicy);
                    //blobClient.UploadData(alertByPolicy, "11AlertByPolicy.csv");
                    blobClient.UploadData(alertByPolicy, "AlertByPolicy.csv");
                    blobClient.UploadData(alertByPolicy, $"Historic/{prefix}-AlertByPolicy.csv");
                }

                _logger.LogInformation("Checking if 'infrastructure' has already been generated...");
                var infrastructureFileExist = blobClient.FileHasBeenModifiedToday("Infrastructure.csv");
                //var infrastructureFileExist = false;
                if (!infrastructureFileExist)
                {
                    var infrastructureNumbers = GetInfrastructureNumbers(prisma, helper, timeDef, standards, alertByPolicyData);

                    _logger.LogInformation("Uploading 'infrastructure' csv files...");
                    var infrastructure = string.Join("\r\n", infrastructureNumbers);
                    //blobClient.UploadData(infrastructure, "11Infrastructure.csv");
                    blobClient.UploadData(infrastructure, "Infrastructure.csv");
                    blobClient.UploadData(infrastructure, $"Historic/{prefix}-Infrastructure.csv");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "main");
            }
            finally
            {
                if (prisma != null)
                    prisma.Close();
            }
        }

        private PrismaClient GetPrismaClient()
        {
            var accessKey = _configuration["ReportClientId"];
            var secretKey = _configuration["ReportClientSecret"];

            var prisma = new PrismaClient(_logger);
            prisma.Initialize(accessKey, secretKey);

            return prisma;
        }

        private (List<string> AlertByPolicy, AlertPolicyDataDto AlertPolicies) GetAlertsByPolicy(PrismaClient prisma, PrismaHelper helper, TimeDefinition timeDef, List<string> standards)
        {
            var accountGroups = helper.GetNNAccountGroups();

            _logger.LogInformation("Getting alerts by policies for NN standards...");
            var alertPolicies = helper.GetAlertsByPolicies(new QueryString
            {
                TimeType = timeDef.TimeType,
                StartTime = timeDef.StartTime,
                EndTime = timeDef.EndTime,
                AlertStatus = AlertStatus.open,
                ComplianceStandard = standards,
                AccountGroup = accountGroups,
            });

            var dateString = GetDate(timeDef.EndTime);
            var alertByPolicy = new List<string> { "Year, Month, Cloud, Severity, PolicyName, Count" };
            foreach (var item in alertPolicies.Azure.Where(ap => ap.Severity == PolicySeverity.high.ToString())
                                                    .OrderByDescending(ap => ap.AlertCount).Take(5).ToList())
            {
                alertByPolicy.Add($"{dateString.Year}, {dateString.Month}, Azure, High, {item.PolicyName}, {item.AlertCount}");
            }
            foreach (var item in alertPolicies.Azure.Where(ap => ap.Severity == PolicySeverity.medium.ToString())
                                                    .OrderByDescending(ap => ap.AlertCount).Take(5).ToList())
            {
                alertByPolicy.Add($"{dateString.Year}, {dateString.Month}, Azure, Medium, {item.PolicyName}, {item.AlertCount}");
            }
            foreach (var item in alertPolicies.AWS.Where(ap => ap.Severity == PolicySeverity.high.ToString())
                                                  .OrderByDescending(ap => ap.AlertCount).Take(5).ToList())
            {
                alertByPolicy.Add($"{dateString.Year}, {dateString.Month}, AWS, High, {item.PolicyName}, {item.AlertCount}");
            }
            foreach (var item in alertPolicies.AWS.Where(ap => ap.Severity == PolicySeverity.medium.ToString())
                                                  .OrderByDescending(ap => ap.AlertCount).Take(5).ToList())
            {
                alertByPolicy.Add($"{dateString.Year}, {dateString.Month}, AWS, Medium, {item.PolicyName}, {item.AlertCount}");
            }

            return (alertByPolicy, alertPolicies);
        }

        private List<string> GetInfrastructureNumbers(PrismaClient prisma, PrismaHelper helper, TimeDefinition timeDef, List<string> standards, AlertPolicyDataDto alertPolicies)
        {
            var accountGroups = helper.GetNNAccountGroups();

            _logger.LogInformation("Getting compliance posture...");
            var posture = helper.GetCompliancePosture(new QueryString
            {
                TimeType = timeDef.TimeType,
                StartTime = timeDef.StartTime,
                EndTime = timeDef.EndTime,
                ComplianceStandard = standards,
                AccountGroup = accountGroups,
            });
            if (posture.Azure == null || posture.AWS == null)
                return null;

            _logger.LogInformation("Updating compliance posture...");
            var allPosture = helper.GetFullCompliancePosture(new QueryString
            {
                TimeType = timeDef.TimeType,
                StartTime = timeDef.StartTime,
                EndTime = timeDef.EndTime,
                AccountGroup = accountGroups,
            });
            if (allPosture.Azure == null || allPosture.AWS == null)
                return null;

            posture.Azure.Summary.TotalResources = allPosture.Azure.Summary.TotalResources;
            posture.Azure.Summary.PassedResources = allPosture.Azure.Summary.TotalResources - posture.Azure.Summary.FailedResources;
            posture.AWS.Summary.TotalResources = allPosture.AWS.Summary.TotalResources;
            posture.AWS.Summary.PassedResources = allPosture.AWS.Summary.TotalResources - posture.AWS.Summary.FailedResources;

            var dateString = GetDate(timeDef.EndTime);
            var azureHighTotal = alertPolicies.Azure.Where(ap => ap.Severity == PolicySeverity.high.ToString()).Sum(ap => ap.AlertCount);
            var azureMediumTotal = alertPolicies.Azure.Where(ap => ap.Severity == PolicySeverity.medium.ToString()).Sum(ap => ap.AlertCount);
            var awsHighTotal = alertPolicies.AWS.Where(ap => ap.Severity == PolicySeverity.high.ToString()).Sum(ap => ap.AlertCount);
            var awsMediumTotal = alertPolicies.AWS.Where(ap => ap.Severity == PolicySeverity.medium.ToString()).Sum(ap => ap.AlertCount);

            var infrastructure = new List<string> { "Year, Month, Cloud, TotalAssets, TotalPassedAssets, TotalFailedAssets, FailedHighAssets, FailedMediumAssets, TotalHighAlerts, TotalMediumAlerts" };
            infrastructure.Add($"{dateString.Year}, {dateString.Month}, Azure, {posture.Azure.Summary.TotalResources}, {posture.Azure.Summary.PassedResources}, {posture.Azure.Summary.FailedResources}, {posture.Azure.Summary.HighSeverityFailedResources}, {posture.Azure.Summary.MediumSeverityFailedResources}, {azureHighTotal}, {azureMediumTotal}");
            infrastructure.Add($"{dateString.Year}, {dateString.Month}, AWS, {posture.AWS.Summary.TotalResources}, {posture.AWS.Summary.PassedResources}, {posture.AWS.Summary.FailedResources}, {posture.AWS.Summary.HighSeverityFailedResources}, {posture.AWS.Summary.MediumSeverityFailedResources}, {awsHighTotal}, {awsMediumTotal}");

            return infrastructure;
        }

        private DateTimeOffset GetReportDate(DateTimeOffset date)
        {
            if (date.Day < 5)
                date = date.AddDays(-5);

            return date;
        }
        private (int Year, string Month) GetDate(DateTimeOffset date)
        {
            date = GetReportDate(date);
            return (date.Year, date.ToString("MMM", CultureInfo.InvariantCulture));
        }
    }
}
