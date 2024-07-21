namespace PrismaCloudFunc.Dto
{
    public class AlertDto
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string CloudType { get; set; }
        public string Severity { get; set; }
        public string Description { get; set; }
        public string DescriptionShort { get; set; }
        public string ResourceType { get; set; }
        public string ResourceId { get; set; }
        public string ResourceRegion { get; set; }
        public string Account { get; set; }
    }
}
