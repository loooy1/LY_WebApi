using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace LY_WebApi.Common.AppsettingConfig
{
    /// <summary>
    /// 配置类扩展方法
    /// </summary>
    public static class ConfigExtensions
    {
        /// <summary>
        /// 绑定并注册所有配置类，支持热更新
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configuration">配置对象</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddAllConfigs(this IServiceCollection services, IConfiguration configuration)
        {
            // 注册 ApiConfig，支持热更新
            services.Configure<ApiConfig>(configuration.GetSection("ApiConfig"));
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptionsMonitor<ApiConfig>>().CurrentValue);

            // 注册 DatabaseConfig，支持热更新
            services.Configure<BackgroundTaskConfig>(configuration.GetSection("BackgroundTask"));
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptionsMonitor<BackgroundTaskConfig>>().CurrentValue);


            return services;
        }
    }



    /// <summary>
    /// 主配置类（对应 appsettings 的 ApiConfig 节点）
    /// </summary>
    public class ApiConfig
    {
        /// <summary>
        /// 微信子配置（对应 WeChat 节点）
        /// </summary>
        public WeChatConfig WeChat { get; set; } = new WeChatConfig();

        /// <summary>
        /// 支付子配置（对应 Payment 节点）
        /// </summary>
        public PaymentConfig Payment { get; set; } = new PaymentConfig();
    }

    /// <summary>
    /// 微信配置子类（嵌套）
    /// </summary>
    public class WeChatConfig
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; } = string.Empty;

        /// <summary>
        /// 应用密钥
        /// </summary>
        public string AppSecret { get; set; } = string.Empty;
    }

    /// <summary>
    /// 支付配置子类（嵌套）
    /// </summary>
    public class PaymentConfig
    {
        /// <summary>
        /// 商户号
        /// </summary>
        public string MerchantId { get; set; } = string.Empty;

        /// <summary>
        /// API密钥
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;

        /// <summary>
        /// 测试字符串数组
        /// </summary>
        public List<string> TestArray { get; set; } = new List<string>();
    }

    /// <summary>
    /// 后台任务配置类
    /// </summary>
    public class BackgroundTaskConfig
    {
        /// <summary>
        /// 任务是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 任务执行间隔（秒）
        /// </summary>
        public int IntervalSeconds { get; set; }
    }
}
