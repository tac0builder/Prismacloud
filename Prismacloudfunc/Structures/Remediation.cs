using System.Text.Json.Serialization;

namespace PrismaCloudFunc.Structures
{
    public class Remediation
    {
        [JsonPropertyName("actions")]
        public List<RemediationAction> Actions { get; set; }
        [JsonPropertyName("cliScriptTemplate")]
        public string CliScriptTemplate { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
