using System.Text.Json.Serialization;

namespace PrismaCloudFunc.Structures
{
    internal class CloudAccountPatch
    {
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }
        [JsonPropertyName("groupIds")]
        public List<string> GroupIds { get; set; }
        [JsonPropertyName("updateChildrenStatus")]
        public bool UpdateChildrenStatus { get; set; }
    }
}
