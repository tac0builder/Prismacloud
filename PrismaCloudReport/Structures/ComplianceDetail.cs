using System.Text.Json.Serialization;

namespace PrismaCloudReport.Structures
{
    public class ComplianceDetail
    {
        [JsonPropertyName("assignedPolicies")]
        public int AssignedPolicies { get; set; }
        [JsonPropertyName("default")]
        public bool Default { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("failedResources")]
        public long FailedResources { get; set; }
        [JsonPropertyName("highSeverityFailedResources")]
        public long HighSeverityFailedResources { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("lowSeverityFailedResources")]
        public long LowSeverityFailedResources { get; set; }
        [JsonPropertyName("mediumSeverityFailedResources")]
        public long MediumSeverityFailedResources { get; set; }
        [JsonPropertyName("passedResources")]
        public long PassedResources { get; set; }
        [JsonPropertyName("totalResources")]
        public long TotalResources { get; set; }
    }
}
