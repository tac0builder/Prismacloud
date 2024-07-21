using System.Text.Json.Serialization;

namespace PrismaCloudFunc.Structures
{
    internal class ParentInfo
    {
        [JsonPropertyName("autoCreated")]
        public bool AutoCreated { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
