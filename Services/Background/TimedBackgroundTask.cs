using System;
using System.Threading;
using System.Threading.Tasks;
using LY_WebApi.Common.Config;
using LY_WebApi.Common.SerilogExt;
using LY_WebApi.Services.ExternalService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LY_WebApi.Services.Background
{
    /// <summary>
    /// 示例后台定时任务，每隔一分钟执行一次
    /// </summary>
    public class TimedBackgroundTask : BackgroundService
    {
        private readonly CustomLogger _customLogger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IOptionsMonitor<ApiConfig> _apiConfigMonitor;

        public TimedBackgroundTask(CustomLogger customLogger, IServiceScopeFactory scopeFactory, IOptionsMonitor<ApiConfig> apiConfigMonitor)
        {
            _customLogger = customLogger;
            _scopeFactory = scopeFactory;
            _apiConfigMonitor = apiConfigMonitor;
        }

        /// <summary>
        /// 后台任务主循环
        /// </summary>
        /// <param name="stoppingToken">取消令牌</param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // 创建作用域，获取 Scoped 服务
                using (var scope = _scopeFactory.CreateScope())
                {
                    var testService = scope.ServiceProvider.GetRequiredService<TestExternalService>();
                    // 调用业务逻辑
                    var posts = await testService.GetAllExternalAsync();
                    //_logger.LogInformation($"后台任务获取到 post 数据：ID={posts?.id}, Title={posts?.title},body={posts?.body}");
                    _customLogger.LogInfo("后台任务", $"后台任务获取到 {posts?.Count ?? 0} 条 posts 数据");
                    _customLogger.LogDebug("后台任务", $"返回WeChat.AppId：{_apiConfigMonitor.CurrentValue.WeChat.AppId}");
                    _customLogger.LogDebug("后台任务", $"返回WeChat.Payment.TestArray的第一个值：{_apiConfigMonitor.CurrentValue.Payment.TestArray[0]}");

                    //_customLogger.LogDebug("前端任务", $"后台任务获取到 {posts?.Count ?? 0} 条 posts 数据");
                }

                //5s执行一次，实际可根据需求调整时间间隔
                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
            }
        }
    }
}