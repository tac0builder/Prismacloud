using System.Collections.Generic;

namespace PrismaCloudReport.Dto
{
    public class AlertPolicyDataDto
    {
        public AlertPolicyDataDto(List<AlertPolicyDto> azure, List<AlertPolicyDto> aws)
        {
            Azure = azure;
            AWS = aws;
        }

        public List<AlertPolicyDto> Azure { get; set; }
        public List<AlertPolicyDto> AWS { get; set; }
    }
}
