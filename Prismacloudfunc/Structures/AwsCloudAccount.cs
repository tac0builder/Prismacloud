using System.Text.Json.Serialization;

namespace PrismaCloudFunc.Structures
{
    internal class AwsCloudAccount : CloudAccount
    {
        [JsonPropertyName("assumeRoleAccount")]
        public string AssumeRoleAccount { get; set; }
        [JsonPropertyName("eventbridgeRuleNamePrefix")]
        public string EventbridgeRuleNamePrefix { get; set; }
        [JsonPropertyName("externalId")]
        public string ExternalId { get; set; }
        [JsonPropertyName("roleArn")]
        public string RoleArn { get; set; }
    }
}
