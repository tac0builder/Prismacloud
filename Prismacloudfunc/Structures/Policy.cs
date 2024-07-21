using System.Text.Json.Serialization;

namespace PrismaCloudFunc.Structures
{
    public class Policy
    {
        [JsonPropertyName("cloudType")]
        public string CloudType { get; set; }
        [JsonPropertyName("complianceMetadata")]
        public List<ComplianceMetadata> ComplianceMetadata { get; set; }
        [JsonPropertyName("createdBy")]
        public string CreatedBy { get; set; }
        [JsonPropertyName("createdOn")]
        public long CreatedOn { get; set; }
        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }
        [JsonPropertyName("labels")]
        public List<string> Labels { get; set; }
        [JsonPropertyName("lastModifiedBy")]
        public string LastModifiedBy { get; set; }
        [JsonPropertyName("lastModifiedOn")]
        public long LastModifiedOn { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("overridden")]
        public bool Overridden { get; set; }
        [JsonPropertyName("policyId")]
        public string PolicyId { get; set; }
        [JsonPropertyName("policySubTypes")]
        public List<string> PolicySubTypes { get; set; }
        [JsonPropertyName("policyType")]
        public string PolicyType { get; set; }
        [JsonPropertyName("policyUpi")]
        public string PolicyUpi { get; set; }
        [JsonPropertyName("recommendation")]
        public string Recommendation { get; set; }
        [JsonPropertyName("remediable")]
        public bool Remediable { get; set; }
        [JsonPropertyName("remediation")]
        public Remediation Remediation { get; set; }
        [JsonPropertyName("restrictAlertDismissal")]
        public bool RestrictAlertDismissal { get; set; }
        [JsonPropertyName("rule")]
        public Rule Rules { get; set; }
        [JsonPropertyName("ruleLastModifiedOn")]
        public long RuleLastModifiedOn { get; set; }
        [JsonPropertyName("severity")]
        public string Severity { get; set; }
        [JsonPropertyName("systemDefault")]
        public bool SystemDefault { get; set; }
    }
}
