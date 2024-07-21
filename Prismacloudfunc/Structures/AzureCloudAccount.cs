using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrismaCloudFunc.Structures
{
    internal class AzureCloudAccount : CloudAccount
    {
        [JsonPropertyName("clientId")]
        public string ClientId { get; set; }
        [JsonPropertyName("cloudAccount")]
        public JsonElement CloudAccount { get; set; }
        [JsonPropertyName("cloudAccountStatus")]
        public JsonElement CloudAccountStatus { get; set; }
        [JsonPropertyName("cspAccountId")]
        public string CspAccountId { get; set; }
        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("key")]
        public string Key { get; set; }
        [JsonPropertyName("monitorFlowLogs")]
        public bool MonitorFlowLogs { get; set; }
        [JsonPropertyName("parentId")]
        public int ParentId { get; set; }
        [JsonPropertyName("servicePrincipalId")]
        public string ServicePrincipalId { get; set; }
        [JsonPropertyName("subscriptionId")]
        public string SubscriptionId { get; set; }
        [JsonPropertyName("tenantId")]
        public string TenantId { get; set; }
    }
}
