using LY_WebApi.Common;
using LY_WebApi.Common.AppsettingConfig;
using LY_WebApi.Common.SerilogExt;
using LY_WebApi.Services.Background.Interface;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LY_WebApi.Services.Background
{
    /// <summary>
    /// 业务后台任务：作为 BackgroundService 运行，同时实现控制接口供 Handler 使用
    /// </summary>
    public class TestTask : BackgroundService, ITestTaskController
    {
        private readonly CustomLogger _logger;
        private readonly IOptionsMonitor<BackgroundTaskConfig> _config;

        private readonly object _lock = new();
        private CancellationTokenSource? _taskCts;
        private CancellationToken _hostStoppingToken = CancellationToken.None;
        private volatile bool _isRunning;

        /// <summary>
        /// 构造函数注入 日志 和 配置监控
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="config"></param>
        public TestTask(CustomLogger logger, IOptionsMonitor<BackgroundTaskConfig> config)
        {
            _logger = logger;
            _config = config;
            _logger.LogInfo("TestTask", $"构造 TestTask 实例 ID:{GetHashCode()}");
        }

        /// <summary>
        /// ITestTaskController 实现 —— 运行标志
        /// </summary>
        public bool IsRunning
        {
            get { return _isRunning; }
        }

        /// <summary>
        /// BackgroundService 的执行循环：读取控制器状态并执行业务
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInfo("TestTask", "TestTask服务 ExecuteAsync开始执行");
            _hostStoppingToken = stoppingToken;

            // 根据配置初始化运行状态（可选）
            var initial = _config.CurrentValue;
            if (initial.IsEnabled)
            {
                Start(stoppingToken);
            }
            _logger.LogWarn("TestTask", $"TestTask服务 业务功能默认{(initial.IsEnabled ? "已开启" : "未开启")}");
            // 主循环
            while (!stoppingToken.IsCancellationRequested)
            {
                if (IsRunning)
                {
                    // 获取当前运行的取消令牌
                    var runToken = GetRunToken();
                    try
                    {
                        _logger.LogInfo("TestTask", $"业务执行中,需要2s （实例 {GetHashCode()}）");
                        // 示例工作：用可取消的 token 等待或执行你的任务逻辑
                        await Task.Delay(2000, CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, runToken).Token);
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogInfo("TestTask", "业务执行被取消");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("TestTask", ex, "业务执行异常");
                    }
                }
                else
                {
                    await Task.Delay(500, stoppingToken);
                }
            }

            _logger.LogInfo("TestTask", "后台循环任务未被执行");
        }

        /// <summary>
        /// 停止时调用，后台循环任务重写的 StopAsync
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInfo("TestTask", "Host 正在停止，触发 Stop");
            await StopAsync().ConfigureAwait(false);
            await base.StopAsync(cancellationToken).ConfigureAwait(false);
        }
        #region 本地私有方法

        /// <summary>
        /// ITestTaskController 实现 —— 线程安全管理 CTS 与运行标志
        /// </summary>
        /// <param name="hostCancellation"></param>
        public void Start(CancellationToken hostCancellation = default)
        {
            lock (_lock)
            {
                if (_isRunning)
                {
                    _logger.LogWarn("TestTask", "Start 被调用但TestTask服务(后台任务)已在运行");
                    return;
                }

                // hostCancellation 优先，否则使用 _hostStoppingToken（如果已在 ExecuteAsync 中设置）
                var hostToken = hostCancellation != default ? hostCancellation : _hostStoppingToken;
                _taskCts = CancellationTokenSource.CreateLinkedTokenSource(hostToken);
                _isRunning = true;
                _logger.LogInfo("TestTask", "TestTask服务已启动(后台任务)");
            }
        }

        /// <summary>
        /// ITestTaskController 实现 —— 线程安全停止任务
        /// </summary>
        /// <returns></returns>
        public async Task StopAsync()
        {
            lock (_lock)
            {
                if (!_isRunning)
                {
                    _logger.LogWarn("TestTask", "Stop 被调用但TestTask服务(后台任务)已是停止状态");
                    return;
                }

                _isRunning = false;
                try
                {
                    _taskCts?.Cancel();
                }
                catch { /* ignore */ }
                finally
                {
                    _taskCts?.Dispose();
                    _taskCts = null;
                }
                _logger.LogInfo("TestTask", "TestTask服务已停止(后台任务)");
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// ITestTaskController 实现 —— 获取运行取消令牌
        /// </summary>
        /// <returns></returns>
        public CancellationToken GetRunToken()
        {
            lock (_lock)
            {
                return _taskCts?.Token ?? CancellationToken.None;
            }
        }

        #endregion
    }
}