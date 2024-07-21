using Microsoft.Extensions.Logging;
using PrismaCloudReport.Structures;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PrismaCloudReport
{
    internal class PrismaClient
    {
        private ILogger _logger;
        private readonly HttpClient _client = new();

        public PrismaClient(ILogger logger)
        {
            _logger = logger;
        }

        public void Initialize(string access, string secret)
        {
            _client.Timeout = TimeSpan.FromMinutes(2);
            _client.BaseAddress = new Uri("https://api.prismacloud.io");

            var accessCls = new { username = access, password = secret };
            var json = JsonSerializer.Serialize(accessCls);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = PostAsync("/login", content).GetAwaiter().GetResult();
            if (string.IsNullOrWhiteSpace(response))
                throw new InvalidOperationException("No response!");

            var login = JsonSerializer.Deserialize<LoginResponse>(response);
            if (login == null)
                throw new InvalidOperationException("No login deserialization!");

            _client.DefaultRequestHeaders.Add("x-redlock-auth", login.Token);
        }

        public void Close()
        {
            _client.Dispose();
        }

        public List<ComplianceStandard> GetComplianceStandards()
        {
            return GetRequest<List<ComplianceStandard>>("/compliance");
        }

        public CompliancePosture GetCompliancePosture(QueryString query)
        {
            if (query == null) return null;

            return GetRequest<CompliancePosture>($"/compliance/posture{query.Generate()}");
        }

        public List<AlertPolicy> GetAlertsByPolcies(QueryString query)
        {
            if (query == null) return null;

            return GetRequest<List<AlertPolicy>>($"/alert/policy{query.Generate()}");
        }

        private T GetRequest<T>(string url) where T : class
        {
            var json = GetAsync(url).GetAwaiter().GetResult();
            if (string.IsNullOrWhiteSpace(json))
                return null;

            try
            {
                var item = JsonSerializer.Deserialize<T>(json);
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "PrismaClient.GetRequest");
            }

            return null;
        }

        private async Task<string> GetAsync(string url)
        {
            return await HandleHttpRequest(() => _client.GetAsync(url)).ConfigureAwait(false);
        }

        private async Task<string> PostAsync(string url, HttpContent content)
        {
            return await HandleHttpRequest(() => _client.PostAsync(url, content)).ConfigureAwait(false);
        }

        private async Task<string> HandleHttpRequest(Func<Task<HttpResponseMessage>> requestMethod)
        {
            try
            {
                var result = await requestMethod.Invoke();
                if (result == null) return string.Empty;

                if (!result.IsSuccessStatusCode)
                {
                    _logger.LogError($"{(int)result.StatusCode}: {result.ReasonPhrase}");

                    return string.Empty;
                }

                return await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "PrismaClient.HandleHttpRequest");
            }

            return string.Empty;
        }
    }
}
