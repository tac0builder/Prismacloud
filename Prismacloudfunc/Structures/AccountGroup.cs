using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrismaCloudFunc.Structures
{
    internal class AccountGroup
    {
        [JsonPropertyName("accountIds")]
        public List<string> AccountIds { get; set; }
        [JsonPropertyName("accounts")]
        public JsonElement Accounts { get; set; }
        [JsonPropertyName("alertRules")]
        public List<CustomAlertRule> alertRules { get; set; }
        [JsonPropertyName("autoCreated")]
        public bool AutoCreated { get; set; }
        [JsonPropertyName("cloudAccountCount")]
        public int CloudAccountCount { get; set; }
        [JsonPropertyName("cloudAccountInfos")]
        public List<CloudAccountInfo> CloudAccountInfos { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("lastModifiedBy")]
        public string LastModifiedBy { get; set; }
        [JsonPropertyName("lastModifiedTs")]
        public long LastModifiedTs { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("parentInfo")]
        public ParentInfo ParentInfo { get; set; }
    }
}
