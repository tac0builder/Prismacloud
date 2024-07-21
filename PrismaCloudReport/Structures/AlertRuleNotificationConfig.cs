using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PrismaCloudReport.Structures
{
    public class AlertRuleNotificationConfig
    {
        [JsonPropertyName("dayOfMonth")]
        public int DayOfMonth { get; set; }
        [JsonPropertyName("daysOfWeek")]
        public List<WeekDay> DaysOfWeek { get; set; }
        [JsonPropertyName("detailedReport")]
        public bool DetailedReport { get; set; } = true;
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = true;
        [JsonPropertyName("frequency")]
        public string Frequency { get; set; }
        [JsonPropertyName("frequencyFromRRule")]
        public string FrequencyFromRRule { get; set; }
        [JsonPropertyName("hourOfDay")]
        public int? HourOfDay { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("includeRemediation")]
        public bool IncludeRemediation { get; set; } = true;
        [JsonPropertyName("lastUpdated")]
        public long LastUpdated { get; set; }
        [JsonPropertyName("last_sent_ts")]
        public long LastSentTS { get; set; }
        [JsonPropertyName("recipients")]
        public List<string> Recipients { get; set; }
        [JsonPropertyName("rruleSchedule")]
        public string RRuleSchedule { get; set; }
        [JsonPropertyName("templateId")]
        public string TemplateId { get; set; }
        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("withCompression")]
        public bool WithCompression { get; set; } = true;
    }
}
