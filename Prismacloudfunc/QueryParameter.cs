using PrismaCloudFunc.Structures;
using System.Dynamic;

namespace PrismaCloudFunc
{
    public class QueryParameter
    {
        // Query string
        public bool? Detailed { get; set; }

        // Body
        public Dictionary<string, List<string>> Filters { get; set; } = new();
        public List<string> GroupBy { get; set; }
        public short Limit { get; set; } = 0;
        public string PageToken { get; set; } = string.Empty;
        public TimeType TimeType { get; set; } = TimeType.undefined;
        public string TimeAmount { get; set; }
        public TimeUnit TimeUnit { get; set; }
        public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }

        public void SetPolicyType(params string[] value)
        {
            Filters.Remove("policy.type");
            Filters.Add("policy.type", value.ToList());
        }
        public void SetPolicySeverity(params PolicySeverity[] value)
        {
            Filters.Remove("policy.severity");
            Filters.Add("policy.severity", value.Select(a => a.ToString()).ToList());
        }
        public void SetAlertStatus(params AlertStatus[] value)
        {
            Filters.Remove("alert.status");
            Filters.Add("alert.status", value.Select(a => a.ToString()).ToList());
        }
        public void SetCloudType(params CloudType[] value)
        {
            Filters.Remove("cloud.type");
            Filters.Add("cloud.type", value.Select(a => a.ToString()).ToList());
        }
        public void SetCloudRegion(params string[] value)
        {
            Filters.Remove("cloud.region");
            Filters.Add("cloud.region", value.ToList());
        }

        public string GenerateQueryString()
        {
            return (Detailed.HasValue ? $"?detailed={Detailed}" : string.Empty);
        }

        public object GenerateBodyObject()
        {
            dynamic body = new ExpandoObject();

            if (Filters != null && Filters.Count > 0)
            {
                body.filters = Filters.SelectMany(f => f.Value.Select(v => new
                {
                    name = f.Key,
                    @operator = "=",
                    value = v
                })).ToList();
            }

            if (GroupBy != null && GroupBy.Count > 0)
                body.groupBy = GroupBy;

            if (Limit > 0)
                body.limit = Limit;

            if (!string.IsNullOrWhiteSpace(PageToken))
                body.pageToken = PageToken;

            if (TimeType == TimeType.relative)
            {
                body.timeRange = new
                {
                    type = TimeType.ToString(),
                    value = new
                    {
                        amount = TimeAmount,
                        unit = TimeUnit.ToString(),
                    }
                };
            }
            else if (TimeType == TimeType.absolute)
            {
                body.timeRange = new
                {
                    type = TimeType.ToString(),
                    value = new
                    {
                        startTime = StartTime.HasValue ? StartTime.Value.ToUnixTimeMilliseconds() : 0,
                        endTime = EndTime.HasValue ? EndTime.Value.ToUnixTimeMilliseconds() : 0,
                    }
                };
            }

            return body;
        }
    }
}
