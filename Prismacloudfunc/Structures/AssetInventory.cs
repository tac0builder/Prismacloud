using System.Text.Json.Serialization;

namespace PrismaCloudFunc.Structures
{
    public class AssetInventory
    {
        [JsonPropertyName("groupedAggregates")]
        public List<AssetInventoryGroupedAggregate> GroupedAggregates { get; set; }
        [JsonPropertyName("requestedTimestamp")]
        public long RequestedTimestamp { get; set; }
        [JsonPropertyName("summary")]
        public AssetInventorySummary Summary { get; set; }
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }
    }
}
