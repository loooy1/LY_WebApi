using System.Threading.Tasks;

namespace LY_WebApi.Services.ExternalService.ExternalServiceBase
{
    /// <summary>
    /// 外部服务协议抽象接口，兼容多种协议（如 HTTP、gRPC、WebSocket 等）
    /// </summary>
    public interface IBaseService
    {
        /// <summary>
        /// 发送请求到外部服务并获取响应
        /// </summary>
        /// <typeparam name="TResponse">响应类型</typeparam>
        /// <param name="endpoint">外部服务地址或标识</param>
        /// <param name="payload">请求体数据（可选）</param>
        /// <param name="method">请求方法（如 GET/POST/PUT/DELETE/自定义）</param>
        /// <returns>反序列化后的响应数据</returns>
        Task<TResponse?> SendAsync<TResponse>(string endpoint, object? payload = null, string method = "GET") where TResponse : class;
    }
}