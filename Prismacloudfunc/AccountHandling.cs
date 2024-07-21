using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PrismaCloudFunc.Structures;
using System.Text.Json;

namespace PrismaCloudFunc
{
    public class AccountHandling
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private const string _inactiveGroupId = "fa639e9d-1e17-49bd-b367-c1de3b077ad4";

        public AccountHandling(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AccountHandling>();
            _configuration = configuration;
        }

        [Function("AccountHandling")]
        public void Run([TimerTrigger("0 0 5 */1 * *"
#if DEBUG
            , RunOnStartup = true
#endif
            )] object timer)
        {
            _logger.LogInformation($"AccountHandling trigger function started executed at: {DateTime.Now}");

            HandleAccounts();

            _logger.LogInformation($"AccountHandling trigger function ended execution at: {DateTime.Now}");
            _logger.LogDebug($"Function data: {timer}");
        }

        private void HandleAccounts()
        {
            PrismaClient prisma = null;

            try
            {
                _logger.LogInformation("Initialize Prisma Cloud client...");
                prisma = GetPrismaClient();
                var helper = new PrismaHelper(prisma);

                _logger.LogInformation("Checking inactive cloud accounts state...");
                var cloudAccounts = helper.GetCloudAccounts();
                foreach (var account in cloudAccounts.Where(ca => !ca.Enabled))
                {
                    // Check if we need to update its disable state
                    var success = helper.SetCloudAccountToDisabled(account, _inactiveGroupId);
                    if (!success)
                        _logger.LogInformation($"Unabled to set cloud account: '{account.Name}' to inactive state!");
                }

                _logger.LogInformation("Checking cloud accounts for named account groups...");
                foreach (var account in cloudAccounts.Where(ca => ca.Enabled))
                {
                    if (account.Name.Contains("Dicerna", StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    if (!NameExistInList(account.Groups, account.Name))
                    {
                        var data = new
                        {
                            accountIds = new List<string> { account.AccountId },
                            description = "Read Only Account Group for System Owner and Audit",
                            name = account.Name,
                        };
                        _logger.LogInformation($"Creating account group: '{account.Name}'");
                        prisma.CreateAccountGroup(data);
                    }
                }

                _logger.LogInformation("Checking account groups with no attachments...");
                var accountGroups = prisma.GetAccountGroups();
                foreach (var group in accountGroups.Where(ag => ag.CloudAccountCount == 0 && ag.AccountIds.Count == 0))
                {
                    // Delete account group if no cloud accounts is attached to it
                    _logger.LogInformation($"Deleting account group: '{group.Name}' since it has no attachment...");
                    prisma.DeleteAccountGroup(group.Id);
                }

                _logger.LogInformation("Checking CSP account groups...");
                var awsAC = accountGroups.FirstOrDefault(ag => ag.Name == "AWS");
                if (awsAC != null)
                {
                    var awsAccounts =
                        cloudAccounts.Where(ca => ca.CloudType == CloudType.aws.ToString() && ca.Enabled).ToList();
                    if (awsAC.AccountIds.Count != awsAccounts.Count)
                    {
                        var data = new
                        {
                            accountIds = awsAccounts.Select(a => a.AccountId).ToList(),
                            description = awsAC.Description,
                            name = awsAC.Name,
                        };
                        _logger.LogInformation("Updating account group for AWS...");
                        prisma.UpdateAccountGroup(awsAC.Id, data);
                    }
                }

                var azureAC = accountGroups.FirstOrDefault(ag => ag.Name == "Azure");
                if (azureAC!= null)
                {
                    var azureAccounts =
                        cloudAccounts.Where(ca => ca.CloudType == CloudType.azure.ToString() && ca.Enabled).ToList();
                    if (azureAC.AccountIds.Count != azureAccounts.Count)
                    {
                        var data = new
                        {
                            accountIds = azureAccounts.Select(a => a.AccountId).ToList(),
                            description = azureAC.Description,
                            name = azureAC.Name,
                        };
                        _logger.LogInformation("Updating account group for Azure...");
                        prisma.UpdateAccountGroup(azureAC.Id, data);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "HandleAccounts.Main");
            }
            finally
            {
                if (prisma != null)
                    prisma.Close();
            }
        }

        private PrismaClient GetPrismaClient()
        {
            var accessKey = _configuration["AccountMgtClientId"];
            var secretKey = _configuration["AccountMgtClientSecret"];

            var prisma = new PrismaClient(_logger);
            prisma.Initialize(accessKey, secretKey);
            return prisma;
        }

        private bool NameExistInList(List<JsonElement> list, string name)
        {
            foreach (var item in list)
            {
                if (item.TryGetProperty("name", out var element))
                {
                    if (element.GetString() == name)
                        return true;
                }
            }
            return false;
        }
    }
}
