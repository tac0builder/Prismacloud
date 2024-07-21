using System.Text.Json.Serialization;

namespace PrismaCloudFunc.Structures
{
    public class AssetInventorySummary
    {
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
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }
        [JsonPropertyName("totalResources")]
        public long TotalResources { get; set; }
        [JsonPropertyName("totalVulnerabilityFailedResources")]
        public long TotalVulnerabilityFailedResources { get; set; }
        [JsonPropertyName("unscannedResources")]
        public long UnscannedResources { get; set; }
    }
}
