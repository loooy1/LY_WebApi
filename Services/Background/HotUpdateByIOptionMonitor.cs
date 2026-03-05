using LY_WebApi.Common.AppsettingConfig;
using Microsoft.Extensions.Options;

namespace LY_WebApi.Services.Background
{
    /// <summary>
    /// 采用直接监视配置的方式实现热更新的后台服务示例
    /// </summary>
    public class HotUpdateByIOptionMonitor : BackgroundService
    {
        // 注入IOptionsMonitor
        private readonly IOptionsMonitor<BackgroundTaskConfig> _configMonitor;
        private readonly ILogger<HotUpdateByIOptionMonitor> _logger;

        // 构造函数注入IOptionsMonitor（核心！）
        public HotUpdateByIOptionMonitor(IOptionsMonitor<BackgroundTaskConfig> configMonitor, ILogger<HotUpdateByIOptionMonitor> logger)
        {
            _configMonitor = configMonitor;
            _logger = logger;
        }

        // 可选：监听配置变更事件（主动感知更新）
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            // 配置更新时触发回调
            _configMonitor.OnChange(config =>
            {
                _logger.LogInformation("配置已更新：IsEnabled2={IsEnabled2}, IntervalSeconds={IntervalSeconds}",
                    config.IsEnabled2, config.IntervalSeconds);
                // 这里可以写配置更新后的逻辑（比如重置连接、重新初始化等）
            });
            return base.StartAsync(cancellationToken);
        }

        // 后台任务核心逻辑：每次执行都取最新配置
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // 关键：每次使用时都取CurrentValue（最新值）
                var currentConfig = _configMonitor.CurrentValue;

                _logger.LogInformation("当前配置：IsEnabled2={IsEnabled2}，IntervalSeconds={IntervalSeconds}秒",
                    currentConfig.IsEnabled2, currentConfig.IntervalSeconds);

                // 执行你的业务逻辑（用最新配置）
                await DoBusinessLogic(currentConfig);
                    
                await Task.Delay(1000, stoppingToken); // 每秒执行一次
            }
        }

        private async Task DoBusinessLogic(BackgroundTaskConfig config)
        {
            // 业务逻辑：使用实时配置
            await Task.CompletedTask;
        }
    }
}
