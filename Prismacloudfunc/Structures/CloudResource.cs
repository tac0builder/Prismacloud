using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrismaCloudFunc.Structures
{
    public class CloudResource
    {
        [JsonPropertyName("account")]
        public string Account { get; set; }
        [JsonPropertyName("accountId")]
        public string AccountId { get; set; }
        [JsonPropertyName("additionalInfo")]
        public object AdditionalInfo { get; set; }
        [JsonPropertyName("cloudAccountAncestors")]
        public List<string> CloudAccountAncestors { get; set; }
        [JsonPropertyName("cloudAccountGroups")]
        public List<string> CloudAccountGroups { get; set; }
        [JsonPropertyName("cloudAccountOwners")]
        public List<string> CloudAccountOwners { get; set; }
        [JsonPropertyName("cloudType")]
        public string CloudType { get; set; }
        [JsonPropertyName("data")]
        public JsonElement Data { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("region")]
        public string Region { get; set; }
        [JsonPropertyName("regionId")]
        public string RegionId { get; set; }
        [JsonPropertyName("resourceApiName")]
        public string ResourceApiName { get; set; }
        [JsonPropertyName("resourceTags")]
        public KeyValuePair<string, string> ResourceTags { get; set; }
        [JsonPropertyName("resourceType")]
        public string ResourceType { get; set; }
        [JsonPropertyName("rrn")]
        public string Rrn { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
