using LY_WebApi.Services.Background;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace LY_WebApi.Common.MediatR
{
    /// <summary>
    /// MediatR 扩展类，用于注册中介者服务（单程序集场景）
    /// </summary>
    public static class MediatR_Extensions
    {
        /// <summary>
        /// 注册 MediatR 服务（核心服务 + 手动注册处理器）
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddCustomMediatR(this IServiceCollection services)
        {
            // 注册 MediatR 服务，扫描当前程序集中的所有处理器
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(MediatR_Extensions).Assembly);
            });

            return services;
        }
    }
}