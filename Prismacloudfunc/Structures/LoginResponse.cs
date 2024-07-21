using System.Text.Json.Serialization;

namespace PrismaCloudFunc.Structures
{
    public class LoginResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("customerNames")]
        public List<LoginCustomer> CustomerNames { get; set; }
    }
}
