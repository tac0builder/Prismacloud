using System.Text.Json.Serialization;

namespace PrismaCloudReport.Structures
{
    public class History
    {
        [JsonPropertyName("reason")]
        public string Reason { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
