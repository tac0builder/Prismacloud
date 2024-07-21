using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PrismaCloudReport.Structures
{
    public class RuleCriteria
    {
        [JsonPropertyName("classificationResult")]
        public string ClassificationResult { get; set; }
        [JsonPropertyName("exposure")]
        public string Exposure { get; set; }
        [JsonPropertyName("extension")]
        public List<string> Extension { get; set; }
    }
}
