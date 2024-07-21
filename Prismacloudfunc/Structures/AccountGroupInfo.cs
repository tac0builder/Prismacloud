using System.Text.Json.Serialization;

namespace PrismaCloudFunc.Structures
{
    internal class AccountGroupInfo
    {
        [JsonPropertyName("autoCreated")]
        public bool AutoCreated { get; set; }
        [JsonPropertyName("groupId")]
        public string GroupId { get; set; }
        [JsonPropertyName("groupName")]
        public string GroupName { get; set; }
        [JsonPropertyName("lastModifiedBy")]
        public string LastModifiedBy { get; set; }
    }
}
