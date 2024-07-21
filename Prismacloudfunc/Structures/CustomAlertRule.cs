using System.Text.Json.Serialization;

namespace PrismaCloudFunc.Structures
{
    internal class CustomAlertRule
    {
        [JsonPropertyName("alertId")]
        public string AlertId { get; set; }
        [JsonPropertyName("alertName")]
        public string AlertName { get; set; }
    }
}
