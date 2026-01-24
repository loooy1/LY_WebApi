using LY_WebApi.Common.MediatR;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace LY_WebApi.Common.MediatR
{
    /// <summary>
    /// MediatR 扩展类，用于注册中介者服务
    /// </summary>
    public static class MediatR_Extensions
    {
        /// <summary>
        /// 注册 MediatR 服务，支持请求/响应和通知模式
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddMediatR(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(MediatR_Extensions).Assembly));
            return services;
        }
    }
}