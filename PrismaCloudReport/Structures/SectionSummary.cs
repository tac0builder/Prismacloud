using System.Text.Json.Serialization;

namespace PrismaCloudReport.Structures
{
    public class SectionSummary
    {
        [JsonPropertyName("failedResources")]
        public long FailedResources { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("passedResources")]
        public long PassedResources { get; set; }
        [JsonPropertyName("totalResources")]
        public long TotalResources { get; set; }
    }
}
