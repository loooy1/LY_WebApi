using LY_WebApi.Services.ExternalService.ExternalServiceBase;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace LY_WebApi.Services.ExternalService
{
    /// <summary>
    /// 示例：调用外部 API 获取 posts 数据
    /// </summary>
    public class TestExternalService
    {
        private readonly IBaseService? _httpService;

        /// <summary>
        /// 构造函数，通过 IServiceProvider 获取 HTTP 协议服务
        /// </summary>
        /// <param name="provider">服务提供者</param>
        public TestExternalService(IServiceProvider provider)
        {
            // 获取 key 为 "Http" 的实现
            _httpService = provider.GetKeyedService<IBaseService>("Http");
        }

        /// <summary>
        /// 获取所有 posts 数据（数组）
        /// </summary>
        /// <returns>PostDto 列表</returns>
        public async Task<List<PostDto>?> GetAllExternalAsync()
        {
            return await _httpService.SendAsync<List<PostDto>>("https://jsonplaceholder.typicode.com/posts", null, "GET");
        }

        /// <summary>
        /// 根据 id 获取单个 post 数据
        /// </summary>
        /// <param name="id">post 主键</param>
        /// <returns>PostDto 或 null</returns>
        public async Task<PostDto?> GetPostByIdExternalAsync(int id)
        {
            return await _httpService.SendAsync<PostDto>($"https://jsonplaceholder.typicode.com/posts/{id}", null, "GET");
        }
    }

    /// <summary>
    /// 对应 posts 接口的 DTO
    /// </summary>
    public class PostDto
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
    }
}
