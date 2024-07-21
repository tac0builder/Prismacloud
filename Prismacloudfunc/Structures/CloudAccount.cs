using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrismaCloudFunc.Structures
{
    internal class CloudAccount
    {
        [JsonPropertyName("accountGroupInfos")]
        public AccountGroupInfo AccountGroupInfos { get; set; }
        [JsonPropertyName("accountId")]
        public string AccountId { get; set; }
        [JsonPropertyName("accountType")]
        public string AccountType { get; set; }
        [JsonPropertyName("addedOn")]
        public long AddedOn { get; set; }
        [JsonPropertyName("associatedAccountGroupsCount")]
        public int AssociatedAccountGroupsCount { get; set; }
        [JsonPropertyName("cloudAccountOwner")]
        public string CloudAccountOwner { get; set; }
        [JsonPropertyName("cloudAccountOwnerCount")]
        public int CloudAccountOwnerCount { get; set; }
        [JsonPropertyName("cloudType")]
        public string CloudType { get; set; }
        [JsonPropertyName("deploymentType")]
        public string DeploymentType { get; set; }
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }
        [JsonPropertyName("groupIds")]
        public List<string> GroupIds { get; set; }
        [JsonPropertyName("groups")]
        public List<JsonElement> Groups { get; set; }
        [JsonPropertyName("ingestionMode")]
        public int IngestionMode { get; set; }
        [JsonPropertyName("lastModifiedBy")]
        public string LastModifiedBy { get; set; }
        [JsonPropertyName("lastModifiedTs")]
        public long LastModifiedTs { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("numberOfChildAccounts")]
        public int NumberOfChildAccounts { get; set; }
        [JsonPropertyName("protectionMode")]
        public string ProtectionMode { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("storageScanEnabled")]
        public bool StorageScanEnabled { get; set; }
        [JsonPropertyName("storageUUID")]
        public string StorageUUID { get; set; }
    }
}
