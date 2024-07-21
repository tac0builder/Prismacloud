using System.Text.Json.Serialization;

namespace PrismaCloudFunc.Structures
{
    public class WeekDay
    {
        [JsonPropertyName("day")]
        public string Day { get; set; }
        [JsonPropertyName("offset")]
        public int Offset { get; set; }
    }
}
