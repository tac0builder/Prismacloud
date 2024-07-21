using System.Text.Json.Serialization;

namespace PrismaCloudReport.Structures
{
    public class CompliancePostureSummary
    {
        [JsonPropertyName("failedResources")]
        public long FailedResources { get; set; }
        [JsonPropertyName("highSeverityFailedResources")]
        public long HighSeverityFailedResources { get; set; }
        [JsonPropertyName("lowSeverityFailedResources")]
        public long LowSeverityFailedResources { get; set; }
        [JsonPropertyName("mediumSeverityFailedResources")]
        public long MediumSeverityFailedResources { get; set; }
        [JsonPropertyName("passedResources")]
        public long PassedResources { get; set; }
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }
        [JsonPropertyName("totalResources")]
        public long TotalResources { get; set; }
    }
}
