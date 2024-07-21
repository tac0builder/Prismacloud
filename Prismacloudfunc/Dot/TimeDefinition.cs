using PrismaCloudFunc.Structures;

namespace PrismaCloudFunc.Dto
{
    internal class TimeDefinition
    {
        public TimeType TimeType { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
    }
}
