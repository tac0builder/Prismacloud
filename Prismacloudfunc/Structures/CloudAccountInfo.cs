using System.Text.Json.Serialization;

namespace PrismaCloudFunc.Structures
{
    internal class CloudAccountInfo
    {
        [JsonPropertyName("accountId")]
        public string AccountId { get; set; }
        [JsonPropertyName("cloudType")]
        public string CloudType { get; set; }
        [JsonPropertyName("lastModifiedBy")]
        public string LastModifiedBy { get; set; }
    }
}
