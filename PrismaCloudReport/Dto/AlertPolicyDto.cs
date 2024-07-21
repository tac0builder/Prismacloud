namespace PrismaCloudReport.Dto
{
    public class AlertPolicyDto
    {
        public string PolicyId { get; set; }
        public string PolicyName { get; set; }
        public string CloudType { get; set; }
        public string Severity { get; set; }
        public string Description { get; set; }
        public string PolicyType { get; set; }
        public long AlertCount { get; set; }
    }
}
