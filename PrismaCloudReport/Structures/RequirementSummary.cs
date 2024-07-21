using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PrismaCloudReport.Structures
{
    public class RequirementSummary
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("sectionSummaries")]
        public List<SectionSummary> SectionSummaries { get; set; }
    }
}
