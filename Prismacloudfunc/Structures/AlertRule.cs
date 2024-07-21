using System.Text.Json.Serialization;

namespace PrismaCloudFunc.Structures
{
    public class AlertRule
    {
        [JsonPropertyName("alertRuleNotificationConfig")]
        public List<AlertRuleNotificationConfig> AlertRuleNotificationConfig { get; set; }
        [JsonPropertyName("allowAutoRemediate")]
        public bool AllowAutoRemediate { get; set; } = true;
        [JsonPropertyName("delayNotificationMs")]
        public long DelayNotificationMs { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = true;
        [JsonPropertyName("lastModifiedBy")]
        public string LastModifiedBy { get; set; }
        [JsonPropertyName("lastModifiedOn")]
        public long LastModifiedOn { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("notificationChannels")]
        public List<string> NotificationChannels { get; set; }
        [JsonPropertyName("notifyOnDismissed")]
        public bool NotifyOnDismissed { get; set; } = true;
        [JsonPropertyName("notifyOnOpen")]
        public bool NotifyOnOpen { get; set; } = true;
        [JsonPropertyName("notifyOnResolved")]
        public bool NotifyOnResolved { get; set; } = true;
        [JsonPropertyName("notifyOnSnoozed")]
        public bool NotifyOnSnoozed { get; set; } = true;
        [JsonPropertyName("openAlertsCount")]
        public int OpenAlertsCount { get; set; }
        [JsonPropertyName("owner")]
        public string Owner { get; set; }
        [JsonPropertyName("policies")]
        public List<string> Policies { get; set; }
        [JsonPropertyName("policyLabels")]
        public List<string> PolicyLabels { get; set; }
        [JsonPropertyName("policyScanConfigId")]
        public string PolicyScanConfigId { get; set; }
        [JsonPropertyName("readOnly")]
        public bool ReadOnly { get; set; } = true;
        [JsonPropertyName("scanAll")]
        public bool ScanAll { get; set; } = true;
        [JsonPropertyName("target")]
        public AlertRuleTarget Target { get; set; }
    }
}
