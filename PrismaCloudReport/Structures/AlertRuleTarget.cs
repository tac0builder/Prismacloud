using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PrismaCloudReport.Structures
{
    public class AlertRuleTarget
    {
        [JsonPropertyName("accountGroups")]
        public List<string> AccountGroups { get; set; }
        [JsonPropertyName("excludedAccounts")]
        public List<string> ExcludedAccounts { get; set; }
        [JsonPropertyName("regions")]
        public List<string> Regions { get; set; }
        [JsonPropertyName("tags")]
        public List<TargetTag> Tags { get; set; }
    }
}
