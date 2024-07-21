using System.Text.Json.Serialization;

namespace PrismaCloudReport.Structures
{
    public class Score
    {
        [JsonPropertyName("maxScore")]
        public long MaxScore { get; set; }
        [JsonPropertyName("score")]
        public long ScoreValue { get; set; }
    }
}
