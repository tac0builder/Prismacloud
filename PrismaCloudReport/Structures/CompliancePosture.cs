using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PrismaCloudReport.Structures
{
    public class CompliancePosture
    {
        [JsonPropertyName("complianceDetails")]
        public List<ComplianceDetail> ComplianceDetails { get; set; }
        [JsonPropertyName("requestedTimestamp")]
        public long RequestedTimestamp { get; set; }
        [JsonPropertyName("requirementSummaries")]
        public List<RequirementSummary> RequirementSummaries { get; set; }
        [JsonPropertyName("summary")]
        public CompliancePostureSummary Summary { get; set; }
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }
    }
}
