using System.Text.Json.Serialization;

namespace PrismaCloudFunc.Structures
{
    public class AssetInventoryGroupedAggregate
    {
        [JsonPropertyName("accountId")]
        public string AccountId { get; set; }
        [JsonPropertyName("accountName")]
        public string AccountName { get; set; }
        [JsonPropertyName("allowDrillDown")]
        public bool AllowDrillDown { get; set; }
        [JsonPropertyName("cloudTypeName")]
        public string CloudTypeName { get; set; }
        [JsonPropertyName("criticalSeverityFailedResources")]
        public long CriticalSeverityFailedResources { get; set; }
        [JsonPropertyName("criticalVulnerabilityFailedResources")]
        public long CriticalVulnerabilityFailedResources { get; set; }
        [JsonPropertyName("failedResources")]
        public long FailedResources { get; set; }
        [JsonPropertyName("highSeverityFailedResources")]
        public long HighSeverityFailedResources { get; set; }
        [JsonPropertyName("highVulnerabilityFailedResources")]
        public long HighVulnerabilityFailedResources { get; set; }
        [JsonPropertyName("informationalSeverityFailedResources")]
        public long InformationalSeverityFailedResources { get; set; }
        [JsonPropertyName("lowSeverityFailedResources")]
        public long LowSeverityFailedResources { get; set; }
        [JsonPropertyName("lowVulnerabilityFailedResources")]
        public long LowVulnerabilityFailedResources { get; set; }
        [JsonPropertyName("mediumSeverityFailedResources")]
        public long MediumSeverityFailedResources { get; set; }
        [JsonPropertyName("mediumVulnerabilityFailedResources")]
        public long MediumVulnerabilityFailedResources { get; set; }
        [JsonPropertyName("passedResources")]
        public long PassedResources { get; set; }
        [JsonPropertyName("regionName")]
        public string RegionName { get; set; }
        [JsonPropertyName("resourceTypeName")]
        public string ResourceTypeName { get; set; }
        [JsonPropertyName("serviceName")]
        public string ServiceName { get; set; }
        [JsonPropertyName("totalResources")]
        public long TotalResources { get; set; }
        [JsonPropertyName("totalVulnerabilityFailedResources")]
        public long TotalVulnerabilityFailedResources { get; set; }
        [JsonPropertyName("unscannedResources")]
        public long UnscannedResources { get; set; }
    }
}
