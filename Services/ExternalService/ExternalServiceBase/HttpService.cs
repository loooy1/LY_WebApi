using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace LY_WebApi.Services.ExternalService.ExternalServiceBase
{
    /// <summary>
    /// 泛型外部服务类，实现 IExternalService，所有增删改查方法私有化，由 SendAsync 统一分发
    /// </summary>
    public class HttpService<T> : IBaseService where T : class
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// 构造函数，注入 HttpClient
        /// </summary>
        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// 统一协议方法，根据 method 参数分发到具体 HTTP 操作
        /// </summary>
        public async Task<TResponse?> SendAsync<TResponse>(string endpoint, object? payload = null, string method = "GET") where TResponse : class
        {
            switch (method.ToUpperInvariant())
            {
                case "GET":
                    return await GetAsync<TResponse>(endpoint);
                case "POST":
                    return await PostAsync<TResponse>(endpoint, payload);
                case "PUT":
                    return await PutAsync<TResponse>(endpoint, payload);
                case "DELETE":
                    return await DeleteAsync<TResponse>(endpoint);
                default:
                    throw new NotSupportedException($"Method {method} is not supported.");
            }
        }

        /// <summary>
        /// 私有 GET 方法
        /// </summary>
        private async Task<TResponse?> GetAsync<TResponse>(string url) where TResponse : class
        {
            return await _httpClient.GetFromJsonAsync<TResponse>(url);
        }

        /// <summary>
        /// 私有 POST 方法
        /// </summary>
        private async Task<TResponse?> PostAsync<TResponse>(string url, object? data) where TResponse : class
        {
            var response = await _httpClient.PostAsJsonAsync(url, data);
            if (!response.IsSuccessStatusCode)
                return null;
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        /// <summary>
        /// 私有 PUT 方法
        /// </summary>
        private async Task<TResponse?> PutAsync<TResponse>(string url, object? data) where TResponse : class
        {
            var response = await _httpClient.PutAsJsonAsync(url, data);
            if (!response.IsSuccessStatusCode)
                return null;
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        /// <summary>
        /// 私有 DELETE 方法
        /// </summary>
        private async Task<TResponse?> DeleteAsync<TResponse>(string url) where TResponse : class
        {
            var response = await _httpClient.DeleteAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }
    }
}