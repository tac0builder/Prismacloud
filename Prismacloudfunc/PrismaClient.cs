using Microsoft.Extensions.Logging;
using PrismaCloudFunc.Structures;
using PrismaCloudReport;
using System.Text;
using System.Text.Json;

namespace PrismaCloudFunc
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

        public AssetInventory GetAssetInventory(QueryString query)
        {
            if (query == null) return null;

            return GetRequest<AssetInventory>($"/v3/inventory{query.Generate()}");
        }

        public List<Alert> GetAlertsV2(QueryParameter query)
        {
            var alerts = new List<Alert>();
            query.Limit = 9500;

            do
            {
                var response = PostRequest<AlertV2>($"/v2/alert{query.GenerateQueryString()}", query.GenerateBodyObject());
                if (response != null)
                {
                    query.PageToken = response.nextPageToken;
                    alerts.AddRange(response.items);
                }
                else
                    break;
            } while (!string.IsNullOrWhiteSpace(query.PageToken));

            return alerts;
        }

        public List<AccountGroup> GetAccountGroups(bool excludeDetailInfo = false)
        {
            return GetRequest<List<AccountGroup>>($"/cloud/group?excludeAccountGroupDetails={excludeDetailInfo}");
        }
        public bool UpdateAccountGroup(string id, object data)
        {
            return PutRequest($"/cloud/group/{id}", data);
        }
        public bool DeleteAccountGroup(string id)
        {
            return DeleteRequest($"/cloud/group/{id}");
        }
        public string CreateAccountGroup(object data)
        {
            return PostRequest("/cloud/group", data);
        }

        public AwsCloudAccount GetAwsCloudAccount(string id)
        {
            return GetRequest<AwsCloudAccount>($"/cloud/aws/{id}");
        }
        public AzureCloudAccount GetAzureCloudAccount(string id)
        {
            return GetRequest<AzureCloudAccount>($"/cloud/azure/{id}");
        }
        public List<CloudAccount> GetCloudAccounts(bool excludeDetailInfo = false)
        {
            return GetRequest<List<CloudAccount>>($"/cloud?excludeAccountGroupDetails={excludeDetailInfo}");
        }
        public List<CloudAccount> GetOrgCloudAccounts(string cloudType, string accountId)
        {
            return GetRequest<List<CloudAccount>>($"/cloud/{cloudType}/{accountId}/project");
        }
        public bool UpdateCloudAccount(string cloudType, string id, object data)
        {
            return PutRequest($"/cloud/{cloudType}/{id}", data);
        }
        public bool PatchCloudAccount(string accountId, string cloudType, CloudAccountPatch data)
        {
            return PatchRequest($"/cloud/{cloudType}/{accountId}", data);
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

        private string PostRequest(string url, object data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return PostAsync(url, content).GetAwaiter().GetResult();
        }
        private T PostRequest<T>(string url, object data) where T : class
        {
            var response = PostRequest(url, data);
            if (string.IsNullOrWhiteSpace(response))
                return null;

            return JsonSerializer.Deserialize<T>(response);
        }
        private async Task<string> PostAsync(string url, HttpContent content)
        {
            return await HandleHttpRequest(() => _client.PostAsync(url, content)).ConfigureAwait(false);
        }

        private bool PutRequest(string url, object data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return PutAsync(url, content).GetAwaiter().GetResult();
        }
        private async Task<bool> PutAsync(string url, HttpContent content)
        {
            return await ExecuteHttpRequest(() => _client.PutAsync(url, content)).ConfigureAwait(false);
        }

        private bool PatchRequest(string url, object data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return PatchAsync(url, content).GetAwaiter().GetResult();
        }
        private async Task<bool> PatchAsync(string url, HttpContent content)
        {
            return await ExecuteHttpRequest(() => _client.PatchAsync(url, content)).ConfigureAwait(false);
        }

        private bool DeleteRequest(string url)
        {
            return DeleteAsync(url).GetAwaiter().GetResult();
        }
        private async Task<bool> DeleteAsync(string url)
        {
            return await ExecuteHttpRequest(() => _client.DeleteAsync(url)).ConfigureAwait(false);
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
        private async Task<bool> ExecuteHttpRequest(Func<Task<HttpResponseMessage>> requestMethod)
        {
            try
            {
                var result = await requestMethod.Invoke();
                if (result != null)
                {
                    var ctx = await result.Content.ReadAsStringAsync();
                    return result.IsSuccessStatusCode;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "PrismaClient.ExecuteHttpRequest");
            }

            return false;
        }
    }
}
