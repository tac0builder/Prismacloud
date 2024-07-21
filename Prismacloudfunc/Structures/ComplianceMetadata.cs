using System.Text.Json.Serialization;

namespace PrismaCloudFunc.Structures
{
    public class ComplianceMetadata
    {
        [JsonPropertyName("complianceId")]
        public string ComplianceId { get; set; }
        [JsonPropertyName("customAssigned")]
        public bool CustomAssigned { get; set; }
        [JsonPropertyName("policyId")]
        public string PolicyId { get; set; }
        [JsonPropertyName("requirementDescription")]
        public string RequirementDescription { get; set; }
        [JsonPropertyName("requirementId")]
        public string RequirementId { get; set; }
        [JsonPropertyName("requirementName")]
        public string RequirementName { get; set; }
        [JsonPropertyName("sectionDescription")]
        public string SectionDescription { get; set; }
        [JsonPropertyName("sectionId")]
        public string SectionId { get; set; }
        [JsonPropertyName("sectionLabel")]
        public string SectionLabel { get; set; }
        [JsonPropertyName("standardDescription")]
        public string StandardDescription { get; set; }
        [JsonPropertyName("standardName")]
        public string StandardName { get; set; }
    }
}
