namespace PrismaCloudFunc.Structures
{
    public class AlertV2
    {
        public List<string> dynamicColumns { get; set; }
        public List<Alert> items { get; set; }
        public string nextPageToken { get; set; }
        public List<string> sortAllowedColumns { get; set; }
        public long totalRows { get; set; }
    }
}
