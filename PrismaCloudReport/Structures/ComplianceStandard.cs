using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PrismaCloudReport.Structures
{
    public class ComplianceStandard
    {
        [JsonPropertyName("cloudType")]
        public List<string> CloudType { get; set; }
        [JsonPropertyName("createdBy")]
        public string CreatedBy { get; set; }
        [JsonPropertyName("createdOn")]
        public long CreatedOn { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("lastModifiedBy")]
        public string LastModifiedBy { get; set; }
        [JsonPropertyName("lastModifiedOn")]
        public long LastModifiedOn { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("policiesAssignedCount")]
        public int PoliciesAssignedCount { get; set; }
        [JsonPropertyName("systemDefault")]
        public bool SystemDefault { get; set; } = true;

        [JsonIgnore]
        public List<ComplianceRequirement> ComplianceRequirements { get; set; }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Name) ? base.ToString() : Name;
        }
    }
}
