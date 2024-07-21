using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PrismaCloudFunc.Dto;
using PrismaCloudFunc.Structures;
using PrismaCloudReport;
using System.Globalization;

namespace PrismaCloudFunc
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
                    StartTime = new DateTimeOffset(2018, 8, 29, 0, 0, 0, TimeSpan.Zero),
                    EndTime = DateTimeOffset.Now,
                };
#if DEBUG
                timeDef.EndTime = new DateTimeOffset(timeDef.EndTime.Year, timeDef.EndTime.Month, 1, 0, 0, 0, timeDef.EndTime.Offset);
#endif
                var prefix = GetReportDate(timeDef.EndTime).ToString("yyMM");
                var dateString = GetDate(timeDef.EndTime);

                _logger.LogInformation("Initialize blob storage client...");
                var storageUri = _configuration["StorageUri"];
                var blobClient = new BlobStorageClient(_logger, storageUri);

                _logger.LogInformation("Checking if 'infrastructure' has already been generated...");
                (AssetInventory Azure, AssetInventory AWS) nnServiceReq = (new(), new());
                var infraFilename = "Infrastructure.csv";
                var infrastructureFileExist = blobClient.FileHasBeenModifiedToday(infraFilename);
#if DEBUG
                infrastructureFileExist = false;
#endif
                if (!infrastructureFileExist)
                {
                    nnServiceReq = GetServiceReqData(helper);

                    _logger.LogInformation("Uploading 'infrastructure' csv files...");
                    var data = new List<string>
                    {
                        "Year, Month, Cloud, TotalAssets, TotalPassedAssets, TotalFailedAssets",
                        $"{dateString.Year}, {dateString.Month}, AWS, {nnServiceReq.AWS.Summary.TotalResources}, {nnServiceReq.AWS.Summary.PassedResources}, {nnServiceReq.AWS.Summary.FailedResources}",
                        $"{dateString.Year}, {dateString.Month}, Azure, {nnServiceReq.Azure.Summary.TotalResources}, {nnServiceReq.Azure.Summary.PassedResources}, {nnServiceReq.Azure.Summary.FailedResources}",
                    };
                    var infrastructure = string.Join("\r\n", data);
#if DEBUG
                    blobClient.UploadData(infrastructure, $"DEV-{infraFilename}");
#else
                    blobClient.UploadData(infrastructure, infraFilename);
                    blobClient.UploadData(infrastructure, $"Historic/{prefix}-{infraFilename}");
#endif
                }

                _logger.LogInformation("Checking if 'alerts' and 'accounts' has already been generated...");
                var alertFilename = "Alerts.csv";
                var accountFilename = "Accounts.csv";
                var alertFileExist = blobClient.FileHasBeenModifiedToday(alertFilename);
                var accountFileExist = blobClient.FileHasBeenModifiedToday(accountFilename);
#if DEBUG
                alertFileExist = false;
                accountFileExist = false;
#endif
                if (!alertFileExist || !accountFileExist)
                {
                    if (infrastructureFileExist)
                        nnServiceReq = GetServiceReqData(helper);

                    var alertData = new List<string>() { "Year, Month, Cloud, Name, Severity, Count" };
                    var accountData = new List<string>() { "Year, Month, Cloud, Name, Account, CriticalCount, HighCount, MediumCount, TotalCount" };

                    _logger.LogInformation("Retrieving alert data...");
                    var anomalies = GetAnomalyAlerts(helper, timeDef);
                    var exposures = helper.GetAlerts(CreateQuery(timeDef, qf => { qf.SetPolicyType("network"); }));
                    var attackPath = helper.GetAlerts(CreateQuery(timeDef, qf => { qf.SetPolicyType("attack_path"); }));

                    _logger.LogInformation("Sorting data...");
                    var nnSvcReqData = AggregateAssetInventory(nnServiceReq.Azure, nnServiceReq.AWS, "CSR", dateString);
                    var anomalyData = AggregateAlertData(anomalies, "anomaly", dateString);
                    var exposureData = AggregateAlertData(exposures, "exposure", dateString);
                    var attackpathData = AggregateAlertData(attackPath, "attack path", dateString);

                    alertData.AddRange(nnSvcReqData.AlertData);
                    alertData.AddRange(anomalyData.AlertData);
                    alertData.AddRange(exposureData.AlertData);
                    alertData.AddRange(attackpathData.AlertData);
                    accountData.AddRange(nnSvcReqData.AccountData);
                    accountData.AddRange(anomalyData.AccountData);
                    accountData.AddRange(exposureData.AccountData);
                    accountData.AddRange(attackpathData.AccountData);

                    _logger.LogInformation("Uploading csv files...");
                    var alerts = string.Join("\r\n", alertData);
                    var accounts = string.Join("\r\n", accountData);
#if DEBUG
                    blobClient.UploadData(alerts, $"DEV-{alertFilename}");
                    blobClient.UploadData(accounts, $"DEV-{accountFilename}");
#else
                    blobClient.UploadData(alerts, alertFilename);
                    blobClient.UploadData(accounts, accountFilename);
                    blobClient.UploadData(alerts, $"Historic/{prefix}-{alertFilename}");
                    blobClient.UploadData(accounts, $"Historic/{prefix}-{accountFilename}");
#endif
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "GenerateCsvFiles.Main");
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

        private (AssetInventory Azure, AssetInventory AWS) GetServiceReqData(PrismaHelper helper)
        {
            return helper.GetAssetInventory(new QueryString()
            {
                ComplianceStandard = new List<string> { "NN Cloud Service Security Requirements" },
                GroupBy = "cloud.account",
            });
        }

        private List<AlertDto> GetAnomalyAlerts(PrismaHelper helper, TimeDefinition timeDef)
        {
            var query = CreateQuery(timeDef, qp => { qp.SetPolicyType("anomaly"); });
            query.StartTime = query.EndTime.Value.AddMonths(-1);

            return helper.GetAlerts(query);
        }

        private QueryParameter CreateQuery(TimeDefinition timeDef, Action<QueryParameter> customLogic)
        {
            var query = new QueryParameter()
            {
                TimeType = timeDef.TimeType,
                StartTime = timeDef.StartTime,
                EndTime = timeDef.EndTime,
                Detailed = true,
            };
            query.SetAlertStatus(AlertStatus.open);
            query.SetPolicySeverity(PolicySeverity.critical, PolicySeverity.high, PolicySeverity.medium);
            customLogic(query);

            return query;
        }

        private (List<string> AlertData, List<string> AccountData) AggregateAssetInventory(AssetInventory azure, AssetInventory aws, string category, (int Year, string Month) dateString)
        {
            var alertData = new List<string>();
            var accountData = new List<string>();

            var azureData = AggregateAssetInventory(azure, "azure", category, dateString);
            var awsData = AggregateAssetInventory(aws, "aws", category, dateString);

            alertData.AddRange(azureData.AlertData);
            alertData.AddRange(awsData.AlertData);
            accountData.AddRange(azureData.AccountData);
            accountData.AddRange(awsData.AccountData);

            return (alertData, accountData);
        }

        private (List<string> AlertData, List<string> AccountData) AggregateAssetInventory(AssetInventory asset, string cloudName, string category, (int Year, string Month) dateString)
        {
            long criticalCount, highCount, mediumCount;
            var alertData = new List<string>();
            var accountData = new List<string>();

            if (asset == null || asset.Summary == null || asset.GroupedAggregates == null)
                return (alertData, accountData);

            criticalCount = asset.Summary.CriticalSeverityFailedResources;
            highCount = asset.Summary.HighSeverityFailedResources;
            mediumCount = asset.Summary.MediumSeverityFailedResources;

            alertData.Add($"{dateString.Year}, {dateString.Month}, {cloudName}, {category}, critical, {criticalCount}");
            alertData.Add($"{dateString.Year}, {dateString.Month}, {cloudName}, {category}, high, {highCount}");
            alertData.Add($"{dateString.Year}, {dateString.Month}, {cloudName}, {category}, medium, {mediumCount}");

            var accounts = asset.GroupedAggregates.GroupBy(a => a.AccountName).Select(i => new
            {
                Name = CleanAccountName(i.Key),
                CloudType = cloudName,
                TotalCount = i.Sum(ag => ag.FailedResources),
                CriticalCount = i.Sum(ag => ag.CriticalSeverityFailedResources),
                HighCount = i.Sum(ag => ag.HighSeverityFailedResources),
                MediumCount = i.Sum(ag => ag.MediumSeverityFailedResources),
            })
            .OrderByDescending(a => a.CriticalCount)
            .ThenByDescending(a => a.HighCount)
            .ThenByDescending(a => a.MediumCount)
            .ThenByDescending(a => a.TotalCount)
            .Take(10).ToList();

            accounts.ForEach(a => accountData.Add($"{dateString.Year}, {dateString.Month}, {a.CloudType}, {category}, {a.Name}, {a.CriticalCount}, {a.HighCount}, {a.MediumCount}, {a.TotalCount}"));

            return (alertData, accountData);
        }

        private (List<string> AlertData, List<string> AccountData) AggregateAlertData(List<AlertDto> alerts, string category, (int Year, string Month) dateString)
        {
            int criticalCount, highCount, mediumCount;
            var alertData = new List<string>();
            var accountData = new List<string>();

            var alertByCloud = alerts.GroupBy(a => a.CloudType).Select(i => new
            {
                Cloud = i.Key,
                Items = i.ToList(),
            }).ToList();

            foreach (var ac in alertByCloud)
            {
                var severities = ac.Items.GroupBy(a => a.Severity).Select(i => new
                {
                    Name = i.Key,
                    Count = i.Count(),
                }).ToList();

                criticalCount = severities.FirstOrDefault(s => s.Name == nameof(PolicySeverity.critical))?.Count ?? 0;
                highCount = severities.FirstOrDefault(s => s.Name == nameof(PolicySeverity.high))?.Count ?? 0;
                mediumCount = severities.FirstOrDefault(s => s.Name == nameof(PolicySeverity.medium))?.Count ?? 0;

                alertData.Add($"{dateString.Year}, {dateString.Month}, {ac.Cloud}, {category}, critical, {criticalCount}");
                alertData.Add($"{dateString.Year}, {dateString.Month}, {ac.Cloud}, {category}, high, {highCount}");
                alertData.Add($"{dateString.Year}, {dateString.Month}, {ac.Cloud}, {category}, medium, {mediumCount}");

                var accounts = ac.Items.GroupBy(a => a.Account).Select(i => new
                {
                    Name = CleanAccountName(i.Key),
                    CloudType = i.FirstOrDefault()?.CloudType ?? string.Empty,
                    TotalCount = i.Count(),
                    CriticalCount = i.Where(ag => ag.Severity == nameof(PolicySeverity.critical)).Count(),
                    HighCount = i.Where(ag => ag.Severity == nameof(PolicySeverity.high)).Count(),
                    MediumCount = i.Where(ag => ag.Severity == nameof(PolicySeverity.medium)).Count(),
                })
                .OrderByDescending(a => a.CriticalCount)
                .ThenByDescending(a => a.HighCount)
                .ThenByDescending(a => a.MediumCount)
                .ThenByDescending(a => a.TotalCount)
                .Take(10).ToList();

                accounts.ForEach(a => accountData.Add($"{dateString.Year}, {dateString.Month}, {ac.Cloud}, {category}, {a.Name}, {a.CriticalCount}, {a.HighCount}, {a.MediumCount}, {a.TotalCount}"));
            }

            return (alertData, accountData);
        }

        private string CleanAccountName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return name;

            if (name[0] == ' ')
                name = name.TrimStart();

            var idx = name.IndexOf(' ');
            if (idx < 0 || name.Substring(idx, 3) != " - ")
            {
                idx = name.IndexOf("(");
            }

            if (idx == -1)
                return name;

            return name.Substring(0, idx).TrimEnd();
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
