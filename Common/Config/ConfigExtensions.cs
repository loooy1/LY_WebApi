using LY_WebApi.Common.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LY_WebApi.Common.Config
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

            // 如果有其他配置类，类似添加
            // services.Configure<OtherConfig>(configuration.GetSection("OtherConfig"));
            // services.AddSingleton(resolver => resolver.GetRequiredService<IOptionsMonitor<OtherConfig>>().CurrentValue);

            return services;
        }
    }



    // 核心配置类（对应 ApiConfig 节点）
    public class ApiConfig
    {
        // 微信子配置（对应 WeChat 节点）
        public WeChatConfig WeChat { get; set; } = new WeChatConfig();
        // 支付子配置（对应 Payment 节点）
        public PaymentConfig Payment { get; set; } = new PaymentConfig();
    }

    // 微信配置子类（嵌套）
    public class WeChatConfig
    {
        public string AppId { get; set; } = string.Empty;
        public string AppSecret { get; set; } = string.Empty;
    }

    // 支付配置子类（嵌套）
    public class PaymentConfig
    {
        public string MerchantId { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        // 测试数组（对应 TestArray 节点）
        public List<string> TestArray { get; set; } = new List<string>();
    }
}
