using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PrismaCloudReport.Structures
{
    public class RiskDetail
    {
        [JsonPropertyName("policyScores")]
        public List<PolicyRiskScore> PolicyScores { get; set; }

        [JsonPropertyName("rating")]
        public string Rating { get; set; }

        [JsonPropertyName("riskScore")]
        public Score RiskScore { get; set; }

        [JsonPropertyName("score")]
        public string Score { get; set; }
    }
}
