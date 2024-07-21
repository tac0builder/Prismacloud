using System.Text.Json.Serialization;

namespace PrismaCloudReport.Structures
{
    public class AttributionEvent
    {
        [JsonPropertyName("event")]
        public string Event { get; set; }
        [JsonPropertyName("event_ts")]
        public long Event_ts { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; }
    }
}
