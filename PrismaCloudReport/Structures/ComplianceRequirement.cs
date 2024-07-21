using System.Text.Json.Serialization;

namespace PrismaCloudReport.Structures
{
    public class ComplianceRequirement
    {
        [JsonPropertyName("complianceId")]
        public string ComplianceId { get; set; }
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
        [JsonPropertyName("requirementId")]
        public string RequirementId { get; set; }
        [JsonPropertyName("standardName")]
        public string StandardName { get; set; }
        [JsonPropertyName("systemDefault")]
        public bool SystemDefault { get; set; } = true;
        [JsonPropertyName("viewOrder")]
        public int ViewOrder { get; set; }
    }
}
