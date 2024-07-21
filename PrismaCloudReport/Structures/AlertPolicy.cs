using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrismaCloudReport.Structures
{
    public class AlertPolicy
    {
        [JsonPropertyName("alertAdditionalInfo")]
        public JsonElement AlertAdditionalInfo { get; set; }
        [JsonPropertyName("alertAttribution")]
        public AlertAttribution AlertAttribution { get; set; }
        [JsonPropertyName("alertCount")]
        public long AlertCount { get; set; }
        [JsonPropertyName("alertRules")]
        public List<AlertRule> AlertRules { get; set; }
        [JsonPropertyName("alertTime")]
        public long AlertTime { get; set; }
        [JsonPropertyName("dismissalDuration")]
        public string DismissalDuration { get; set; }
        [JsonPropertyName("dismissalNote")]
        public string DismissalNote { get; set; }
        [JsonPropertyName("dismissalUntilTs")]
        public long DismissalUntilTs { get; set; }
        [JsonPropertyName("dismissedBy")]
        public string DismissedBy { get; set; }
        [JsonPropertyName("eventOccurred")]
        public long EventOccurred { get; set; }
        [JsonPropertyName("firstSeen")]
        public long FirstSeen { get; set; }
        [JsonPropertyName("history")]
        public List<History> History { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("lastSeen")]
        public long LastSeen { get; set; }
        [JsonPropertyName("metadata")]
        public JsonElement Metadata { get; set; }
        [JsonPropertyName("policy")]
        public Policy Policy { get; set; }
        [JsonPropertyName("policyId")]
        public string PolicyId { get; set; }
        [JsonPropertyName("reason")]
        public string Reason { get; set; }
        [JsonPropertyName("resource")]
        public CloudResource Resource { get; set; }
        [JsonPropertyName("riskDetail")]
        public RiskDetail RiskDetail { get; set; }
        [JsonPropertyName("saveSearchId")]
        public string SaveSearchId { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("triggeredBy")]
        public string TriggeredBy { get; set; }
    }
}
