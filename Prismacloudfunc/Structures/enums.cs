namespace PrismaCloudFunc.Structures
{
    public enum TimeType
    {
        undefined,
        relative,
        absolute,
    }

    public enum TimeUnit
    {
        minute,
        hour,
        day,
        week,
        month,
        year
    }

    public enum AccountType
    {
        account,
        organization,
        tenant,
        management_group,
    }

    public enum CloudType
    {
        undefined = 1,
        aws = 2,
        azure = 4,
        gcp = 8,
        oci = 16,
    }

    [Flags]
    public enum PolicySeverity
    {
        undefined = 1,
        informational = 2,
        low = 4,
        medium = 8,
        high = 16,
        critical = 32,
    }

    [Flags]
    public enum AlertStatus
    {
        undefined = 1,
        open = 2,
        dismissed = 4,
        snoozed = 8,
        resolved = 16,
    }
}
