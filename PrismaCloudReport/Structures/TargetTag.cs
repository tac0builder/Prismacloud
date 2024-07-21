using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PrismaCloudReport.Structures
{
    public class TargetTag
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }
        [JsonPropertyName("values")]
        public List<string> Values { get; set; }
    }
}
