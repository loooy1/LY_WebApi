using LY_WebApi.Common.AppsettingConfig;
using LY_WebApi.Common.MediatR;
using MediatR;
using Microsoft.Extensions.Options;


namespace LY_WebApi.Services.Background
{
    /// <summary>
    /// 独立配置监听服务（负责：监听配置变更 → 发送MediatR启停指令）
    /// </summary>
    public class TaskConfigMonitor : BackgroundService
    {
        private readonly IMediator _mediator;
        private readonly IOptionsMonitor<BackgroundTaskConfig> _config; // 实时读配置

        public TaskConfigMonitor(IMediator mediator,IOptionsMonitor<BackgroundTaskConfig> config)
        {
            _mediator = mediator;
            _config = config;
        }

        // 监听配置变更，发送MediatR指令
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // ========== 核心修复：启动时主动同步初始配置 ==========
            var initialConfig = _config.CurrentValue;
            // 初始值为True → 立即发送启动指令；为False → 发送停止指令
            await _mediator.Send(new TaskControlCommand { Enable = initialConfig.IsEnabled }, stoppingToken);
            Console.WriteLine($"📌 初始配置同步完成：IsEnabled={initialConfig.IsEnabled}");

            // 记录上一次配置状态（初始值已同步，后续只监听变更）
            bool lastConfigState = initialConfig.IsEnabled;

            // 持续监听配置（1秒检查一次）
            while (!stoppingToken.IsCancellationRequested)
            {
                var currentConfig = _config.CurrentValue;
                // 配置发生变化时，发送MediatR指令
                if (currentConfig.IsEnabled != lastConfigState)
                {
                    await _mediator.Send(new TaskControlCommand { Enable = currentConfig.IsEnabled }, stoppingToken);
                    lastConfigState = currentConfig.IsEnabled; // 更新状态
                }
                await Task.Delay(1000, stoppingToken); // 检查间隔，可调整
            }
        }
    }
}
