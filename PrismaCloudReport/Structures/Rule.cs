using System.Text.Json.Serialization;

namespace PrismaCloudReport.Structures
{
    public class Rule
    {
        [JsonPropertyName("apiName")]
        public string ApiName { get; set; }
        [JsonPropertyName("cloudAccount")]
        public string CloudAccount { get; set; }
        [JsonPropertyName("cloudType")]
        public string CloudType { get; set; }
        [JsonPropertyName("criteria")]
        public string Criteria { get; set; }
        [JsonPropertyName("dataCriteria")]
        public RuleCriteria DataCriteria { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("parameters")]
        public object Parameters { get; set; }
        [JsonPropertyName("resourceIdPath")]
        public string ResourceIdPath { get; set; }
        [JsonPropertyName("resourceType")]
        public string ResourceType { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
