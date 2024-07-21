using System.Text.Json.Serialization;

namespace PrismaCloudReport.Structures
{
    public class RemediationAction
    {
        [JsonPropertyName("operation")]
        public string Operation { get; set; }
        [JsonPropertyName("payload")]
        public string Payload { get; set; }
    }
}
