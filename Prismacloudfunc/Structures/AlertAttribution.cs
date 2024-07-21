using System.Text.Json.Serialization;

namespace PrismaCloudFunc.Structures
{
    public class AlertAttribution
    {
        [JsonPropertyName("attributionEventList")]
        public List<AttributionEvent> AttributionEventList { get; set; }
        [JsonPropertyName("resourceCreatedBy")]
        public string ResourceCreatedBy { get; set; }
        [JsonPropertyName("resourceCreatedOn")]
        public long ResourceCreatedOn { get; set; }
    }
}
