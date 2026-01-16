using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace LY_WebApi.Services.ExternalService
{
    /// <summary>
    /// 外部服务类，用于访问第三方接口
    /// </summary>
    public class ExternalService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// 构造函数，注入 HttpClient
        /// </summary>
        /// <param name="httpClient">HttpClient 实例</param>
        public ExternalService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// 示例：根据 id 从外部接口获取 Shirt 数据
        /// </summary>
        /// <param name="id">Shirt 主键</param>
        /// <returns>Shirt DTO 或 null</returns>
        //public async Task<ShirtDto?> GetShirtByIdAsync(int id)
        //{
        //    // 假设外部接口地址如下
        //    var url = $"https://api.example.com/shirts/{id}";
        //    return await _httpClient.GetFromJsonAsync<ShirtDto>(url);
        //}

        // 你可以继续扩展 Post/Put/Delete 等方法
    }
}