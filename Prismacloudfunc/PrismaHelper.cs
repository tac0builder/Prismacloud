using PrismaCloudFunc.Dto;
using PrismaCloudFunc.Structures;
using PrismaCloudReport;

namespace PrismaCloudFunc
{
    internal class PrismaHelper
    {
        private readonly PrismaClient _prisma = null;
        private readonly List<string> _awsCloudRegions = new() { "AWS Virginia", "AWS Ohio", "AWS Oregon", "AWS Ireland", "AWS London", "AWS Frankfurt", "AWS Tokyo", "AWS Seoul", "AWS Sao Paulo", "AWS Singapore" };

        public PrismaHelper(PrismaClient client)
        {
            _prisma = client;
        }

        public List<CloudAccount> GetCloudAccounts()
        {
            var azSubscriptions = new List<CloudAccount>();
            var cloudAccounts = _prisma.GetCloudAccounts();

            var azTenant = cloudAccounts.Where(a => a.CloudType == CloudType.azure.ToString() && a.AccountType == AccountType.tenant.ToString()).ToList();
            if (azTenant != null && azTenant.Count > 0)
            {
                azSubscriptions = _prisma.GetOrgCloudAccounts(azTenant[0].CloudType, azTenant[0].AccountId);
            }

            return cloudAccounts.Where(ca => ca.CloudType == CloudType.aws.ToString()).Concat(
                   azSubscriptions.Where(az => az.CloudType == CloudType.azure.ToString()))
                   .Where(ca => ca.AccountType == AccountType.account.ToString()).ToList();
        }

        public (AssetInventory Azure, AssetInventory AWS) GetAssetInventory(QueryString query)
        {
            query.CloudType = CloudType.azure;
            var azure = _prisma.GetAssetInventory(query);

            query.CloudType = CloudType.aws;
            query.CloudRegion = _awsCloudRegions;
            var aws = _prisma.GetAssetInventory(query);

            return (azure, aws);
        }

        public List<AlertDto> GetAlerts(QueryParameter query)
        {
            // Azure data
            query.SetCloudType(CloudType.azure);
            var alerts = _prisma.GetAlertsV2(query);

            // AWS data
            query.SetCloudType(CloudType.aws);
            query.SetCloudRegion(_awsCloudRegions.ToArray());
            alerts.AddRange(_prisma.GetAlertsV2(query));

            var alertDtos = (alerts.Select(alert => new AlertDto
            {
                Id = alert.Id,
                Status = alert.Status,
                CloudType = alert.Resource.CloudType,
                Severity = alert.Policy.Severity,
                Description = alert.Policy.Description,
                DescriptionShort = alert.Policy.Name,
                ResourceType = string.IsNullOrWhiteSpace(alert.Resource.ResourceApiName) ? alert.Resource.Id : alert.Resource.ResourceApiName,
                ResourceId = alert.Resource.Name,
                ResourceRegion = alert.Resource.Region,
                Account = alert.Resource.Account,
            })).ToList();

            return alertDtos;
        }

        public bool SetCloudAccountToDisabled(CloudAccount cloudAccount, string inactiveGroupId)
        {
            var hasInactiveGroup = cloudAccount.GroupIds.Count == 1 && cloudAccount.GroupIds.Contains(inactiveGroupId);
            var success = false;

            if (cloudAccount.Enabled)
            {
                var patchData = new CloudAccountPatch
                {
                    Enabled = false,
                    GroupIds = new List<string> { inactiveGroupId },
                    UpdateChildrenStatus = true,
                };

                success = _prisma.PatchCloudAccount(cloudAccount.AccountId, cloudAccount.CloudType, patchData);
                return success;
            }
            else
            {
                if (hasInactiveGroup)
                {
                    // account is already disabled!
                    return true;
                }
                else
                {
                    object data = null;
                    if (cloudAccount.CloudType == CloudType.aws.ToString())
                    {
                        var awsCA = _prisma.GetAwsCloudAccount(cloudAccount.AccountId);
                        if (awsCA == null) return false;

                        data = new
                        {
                            accountId = cloudAccount.AccountId,
                            accountType = awsCA.AccountType,
                            enabled = false,
                            externalId = awsCA.ExternalId,
                            groupIds = new List<string> { inactiveGroupId },
                            name = awsCA.Name,
                            protectionMode = awsCA.ProtectionMode,
                            roleArn = awsCA.RoleArn,
                            storageScanEnabled = awsCA.StorageScanEnabled,
                        };
                    }
                    else if (cloudAccount.CloudType == CloudType.azure.ToString())
                    {
                        var azureCA = _prisma.GetAzureCloudAccount(cloudAccount.AccountId);
                        if (azureCA == null) return false;

                        data = new
                        {
                            clientId = azureCA.ClientId,
                            environmentType = cloudAccount.CloudType,
                            key = azureCA.Key,
                            monitorFlowLogs = azureCA.MonitorFlowLogs,
                            servicePrincipalId = azureCA.ServicePrincipalId,
                            tenantId = azureCA.TenantId,
                            cloudAccount = new
                            {
                                accountId = cloudAccount.AccountId,
                                accountType = cloudAccount.AccountType,
                                enabled = false,
                                groupIds = new List<string> { inactiveGroupId },
                                name = cloudAccount.Name,
                                protectionMode = cloudAccount.ProtectionMode,
                            },
                        };
                    }
                    else
                    {
                        // Unknown cloud type selected!!
                        return false;
                    }
                    success = _prisma.UpdateCloudAccount(cloudAccount.CloudType, cloudAccount.AccountId, data);
                    // Account is already disabled, but 'success' will determine if it was possible
                    // to set AccountGroup to 'inactive'
                    return success;
                }
            }
        }
    }
}
