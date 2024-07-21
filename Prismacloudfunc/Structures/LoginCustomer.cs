using System.Text.Json.Serialization;

namespace PrismaCloudFunc.Structures
{
    public class LoginCustomer
    {
        [JsonPropertyName("customerName")]
        public string CustomerName { get; set; }
        [JsonPropertyName("tosAccepted")]
        public bool ToSAccepted { get; set; } = true;
    }
}
