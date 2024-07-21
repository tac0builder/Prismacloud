using PrismaCloudFunc.Structures;

namespace PrismaCloudReport
{
    internal class QueryString
    {
        /// <summary>Time type.</summary>
        public TimeType TimeType { get; set; } = TimeType.undefined;
        /// <summary>Number of units.</summary>
        public string TimeAmount { get; set; }
        /// <summary>Unit used in amount.</summary>
        public TimeUnit TimeUnit { get; set; }
        public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }

        /// <summary>Cloud account / subscription.</summary>
        public string CloudAccount { get; set; } = string.Empty;
        public List<string> AccountGroup { get; set; } = null;
        public CloudType CloudType { get; set; } = CloudType.undefined;
        /// <summary>Cloud region.</summary>
        public List<string> CloudRegion { get; set; } = null;
        public List<string> ComplianceStandard { get; set; } = null;

        public string GroupBy { get; set; } = null;
        /// <summary>true = Return detailed alert data.</summary>
        public bool? Detailed { get; set; }
        public PolicySeverity PolicySeverity { get; set; } = PolicySeverity.undefined;
        public string PolicyType { get; set; } = string.Empty;
        public AlertStatus AlertStatus { get; set; } = AlertStatus.undefined;

        public string PageToken { get; set; } = string.Empty;
        public short Limit { get; set; } = 0;

        public string Generate()
        {
            return $"?cloud.type={CloudType}" +
                (string.IsNullOrWhiteSpace(CloudAccount) ? "" : $"&cloud.account={CloudAccount}") +
                ConcatStringList(AccountGroup, "account.group") +
                ConcatStringList(CloudRegion, "cloud.region") +
                ConcatStringList(ComplianceStandard, "policy.complianceStandard") +
                (string.IsNullOrWhiteSpace(GroupBy) ? "" : $"&groupBy={GroupBy}") +
                (string.IsNullOrWhiteSpace(TimeAmount) ? "" : $"&timeType={TimeType}&timeAmount={TimeAmount}&timeUnit={TimeUnit}") +
                (StartTime.HasValue ? $"&timeType={TimeType}&startTime={StartTime.Value.ToUnixTimeMilliseconds()}&endTime={EndTime.Value.ToUnixTimeMilliseconds()}" : "") +
                (PolicySeverity == PolicySeverity.undefined ? "" : ConcatFlagsEnum(PolicySeverity, "policy.severity")) +
                (AlertStatus == AlertStatus.undefined ? "" : ConcatFlagsEnum(AlertStatus, "alert.status")) +
                (string.IsNullOrWhiteSpace(PolicyType) ? "" : $"&policy.type={PolicyType}") +
                (Detailed.HasValue ? $"&detailed={Detailed}" : "") +
                (PageToken == string.Empty ? "" : $"&pageToken={PageToken}") +
                (Limit < 1 ? "" : $"&limit={Limit}");
        }

        private string ConcatFlagsEnum(Enum input, string propertyName)
        {
            var enums = Enum.GetValues(input.GetType()).Cast<Enum>().Where(input.HasFlag);
            return string.Join(string.Empty, enums.Select(e => $"&{propertyName}={e}"));
        }
        private string ConcatStringList(List<string> input, string propertyName)
        {
            if (input == null || input.Count == 0) return string.Empty;
            return string.Join(string.Empty, input.Select(i => $"&{propertyName}={i}"));
        }
    }
}
