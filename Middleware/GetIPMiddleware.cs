namespace LY_WebApi.Middleware
{
    /// <summary>
    /// 静态类  作为扩展方法的容器
    /// </summary>
    public static class GetIPMiddlewareExt
    {
        /// <summary>
        /// 中间件扩展方法  获取请求的IP地址
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseGetIPMiddlewareExt(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GetIPMiddleware>();
        }
    }

    /// <summary>
    /// 获取请求的IP地址中间件
    /// </summary>
    public class GetIPMiddleware 
    {
        //请求委托 中间件next
        private readonly RequestDelegate _next;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="next">请求委托 中间件next </param>
        public GetIPMiddleware(RequestDelegate next)
        {
                _next = next;
        }

        /// <summary>
        /// 中间件执行方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString();
            Console.WriteLine($"请求的IP地址为：{ip}");
            // 调用下一个中间件
            await _next(context);
        }
    }
}
