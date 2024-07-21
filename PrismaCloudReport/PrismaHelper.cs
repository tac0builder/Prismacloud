using PrismaCloudReport.Dto;
using PrismaCloudReport.Structures;
using System.Collections.Generic;
using System.Linq;

namespace PrismaCloudReport
{
    internal class PrismaHelper
    {
        private readonly PrismaClient _prisma = null;
        private readonly List<string> _awsCloudRegions = new() { "AWS Virginia", "AWS Oregon", "AWS Ireland", "AWS Frankfurt", "AWS Tokyo", "AWS Sao Paulo" };

        public PrismaHelper(PrismaClient client)
        {
            _prisma = client;
        }

        public List<string> GetNNAccountGroups()
        {
            return new List<string>
            {
                "AWS",                     // 812ecb1f-f82d-4350-9a48-5fecefd42251
                "Azure",                   // 9fe3214d-1a98-4609-b11d-96ce7f6275b0
                "Default Account Group",   // 3f07293f-42e1-4b05-9032-16afdfdc0138
                "External account group",  // aabd19ff-818b-43b1-bcad-5a6bdefe2d93
                "Google GCP",              // 96334b40-970d-47cd-89e8-49e82b23f689
                "splunk_alerts",           // a31fe28f-510f-4447-922e-ae92eddd7ee2
            };
        }

        public List<string> GetNNComplianceStandards()
        {
            var complianceList = new List<string>
            {
                "04cefbae-09cc-4b90-b2f3-efdcdd2e7422", // NN Security Baseline (AWS)
                "cfe7cb98-41bc-47e1-8eee-c3abbb100334", // NN Security Baseline (Azure) v1.1
                "a0ea1077-424f-45fd-994e-4caef6d4d9de", // AWS Foundational Security Best Practices standard
                "d3d69560-5e87-40ca-83e2-a6b321484555", // CIS v1.2.0 (AWS)
            };

            var standards = _prisma.GetComplianceStandards();
            return standards.Where(s => complianceList.Contains(s.Id)).Select(s => s.Name).ToList();
        }

        public (CompliancePosture Azure, CompliancePosture AWS) GetCompliancePosture(QueryString query)
        {
            query.CloudType = CloudType.azure;
            var azure = _prisma.GetCompliancePosture(query);

            query.CloudType = CloudType.aws;
            query.CloudRegion = _awsCloudRegions;
            var aws = _prisma.GetCompliancePosture(query);

            return (azure, aws);
        }

        public (CompliancePosture Azure, CompliancePosture AWS) GetFullCompliancePosture(QueryString query)
        {
            query.CloudType = CloudType.azure;
            var azure = _prisma.GetCompliancePosture(query);

            query.CloudType = CloudType.aws;
            var aws = _prisma.GetCompliancePosture(query);

            return (azure, aws);
        }

        public AlertPolicyDataDto GetAlertsByPolicies(QueryString query)
        {
            query.CloudType = CloudType.azure;
            var azure = _prisma.GetAlertsByPolcies(query);

            query.CloudType = CloudType.aws;
            query.CloudRegion = _awsCloudRegions;
            var aws = _prisma.GetAlertsByPolcies(query);

            var azureDto = azure.Select(a => new AlertPolicyDto
            {
                PolicyId = a.PolicyId,
                PolicyName = a.Policy.Name,
                CloudType = "azure",
                Severity = a.Policy.Severity,
                Description = a.Policy.Description,
                PolicyType = a.Policy.PolicyType,
                AlertCount = a.AlertCount,
            }).ToList();
            var awsDto = aws.Select(a => new AlertPolicyDto
            {
                PolicyId = a.PolicyId,
                PolicyName = a.Policy.Name,
                CloudType = "aws",
                Severity = a.Policy.Severity,
                Description = a.Policy.Description,
                PolicyType = a.Policy.PolicyType,
                AlertCount = a.AlertCount,
            }).ToList();

            return new AlertPolicyDataDto(azureDto, awsDto);
        }
    }
}
