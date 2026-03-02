using LY_WebApi.Application;
using LY_WebApi.Application.MediatR.Handler;
using LY_WebApi.Common.AppsettingConfig;
using LY_WebApi.Common.SerilogExt;
using MediatR;
using Microsoft.Extensions.Options;

namespace LY_WebApi.Services.Background
{
    /// <summary>
    /// 独立配置监听服务（负责：监听配置变更 → 发送MediatR启停指令）
    /// </summary>
    public class AppsettingConfigMonitor : BackgroundService
    {
        private readonly IMediator _mediator;
        private readonly IOptionsMonitor<BackgroundTaskConfig> _config;
        private readonly CustomLogger _logger; // 增加日志
        private bool _lastConfigState; // 记录上一次状态，防重复

        /// <summary>
        /// 构造函数注入 MediatR_配置监控_日志
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="config"></param>
        /// <param name="logger"></param>
        public AppsettingConfigMonitor(IMediator mediator, IOptionsMonitor<BackgroundTaskConfig> config, CustomLogger logger)
        {
            _mediator = mediator;
            _config = config;
            _logger = logger;
           
        }

        /// <summary>
        /// 后台服务主逻辑
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                // 1. 启动时获取初始配置
                _lastConfigState= _config.CurrentValue.IsEnabled;

                // 2. 改用IOptionsMonitor的回调监听配置变更
                _config.OnChange(async (newConfig) =>
                {
                    // 配置变更且状态不同时，发送指令
                    if (newConfig.IsEnabled != _lastConfigState)
                    {
                        await SendTaskControlCommand(newConfig.IsEnabled, stoppingToken);
                        _lastConfigState = newConfig.IsEnabled;
                        _logger.LogInfo("MediatR", $"配置变更触发指令发送完毕：IsEnabled={newConfig.IsEnabled}");
                    }
                });

                // 3. 保持后台服务运行（回调模式下，只需阻塞即可）
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // 服务正常停止时的取消异常，无需处理
                _logger.LogInfo("MediatR", "配置监听服务已正常停止");
            }
            catch (Exception ex)
            {
                // 捕获所有异常，保证服务不崩溃
                _logger.LogError("MediatR", ex, "配置监听服务发生未处理异常");
                // 可选：重试逻辑，避免单次失败就终止
                await Task.Delay(3000, stoppingToken);
                await ExecuteAsync(stoppingToken);
            }
        }


        #region 私有方法

        /// <summary>
        /// 发送MediatR指令（带异常捕获，避免单次失败影响整体）
        /// </summary>
        private async Task SendTaskControlCommand(bool enable, CancellationToken stoppingToken)
        {
            try
            {
                // 发送指令（带超时控制，避免阻塞）
                //await _mediator.Send(new TaskControlCommand { Enable = enable }, stoppingToken);
                // 发布事件
                await _mediator.Publish(new TaskControlEvent { Enable = enable }, stoppingToken);
                _logger.LogInfo("MediatR", $"MediatR事件发送完毕：Enable={enable}");
            }
            catch (Exception ex)
            {
                _logger.LogError("MediatR", ex, $"MediatR事件发送失败：Enable={enable}");
                // 可选：重试1次
                await Task.Delay(500, stoppingToken);
                try
                {
                    //await _mediator.Send(new TaskControlCommand { Enable = enable }, stoppingToken);
                    await _mediator.Publish(new TaskControlEvent { Enable = enable }, stoppingToken);
                }
                catch (Exception retryEx)
                {
                    _logger.LogError("MediatR", retryEx, $"MediatR事件重试也失败：Enable={enable}");
                }
            }
        }
        #endregion


    }

}