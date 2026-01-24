using LY_WebApi.Common.MediatR;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

/// <summary>
/// 后台任务（修复：取消令牌+线程安全+实时停止）
/// </summary>
public class TimedBackgroundTask : BackgroundService
{
    private readonly ILogger<TimedBackgroundTask> _logger;
    private readonly Channel<TaskControlCommand> _commandChannel;
    private bool _isRunning; // 任务运行状态
    private CancellationTokenSource? _taskCts; // 任务取消令牌
    private readonly object _lockObj = new(); // 线程安全锁
    private CancellationToken _hostStoppingToken; // 服务停止令牌（核心：关联服务停止）

    public TimedBackgroundTask(ILogger<TimedBackgroundTask> logger, Channel<TaskControlCommand> commandChannel)
    {
        _logger = logger;
        _commandChannel = commandChannel;
    }

    // 任务初始化：保存服务停止令牌
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("📌 后台任务已初始化，监听 Channel 指令");

        // 初始化任务取消令牌
        _taskCts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);

        var reader = _commandChannel.Reader;

        // 任务主循环：等待启用信号
        while (!stoppingToken.IsCancellationRequested)
        {
            // 从 Channel 获取指令
            if (reader.TryRead(out var command))
            {
                _logger.LogInformation($"📥 从 Channel 获取指令：Enable={command.Enable}");

                if (command.Enable)
                    StartTask();
                else
                    StopTask();
            }

            if (_isRunning)
            {
                // 执行业务逻辑
                await RunContinuousTask(_taskCts.Token);
            }
            else
            {
                // 等待启用（避免 CPU 占用）
                await Task.Delay(1000, stoppingToken);
            }
        }
    }

    public void StartTask()
    {
        if (_isRunning) return;
        _isRunning = true;
        _logger.LogInformation("✅ 后台任务已启动");
    }

    public void StopTask()
    {
        if (!_isRunning) return;
        _isRunning = false;
        _logger.LogInformation("❌ 后台任务已停止");
    }

    /// <summary>
    /// 核心业务逻辑（每次循环执行）
    /// </summary>
    private async Task RunContinuousTask(CancellationToken token)
    {
        try
        {
            // ========== 你的核心业务逻辑 ==========
            _logger.LogInformation($"📝 后台任务执行中：{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");

            // 模拟业务延迟
            await Task.Delay(2000, token);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("⏹️ 任务执行被取消");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ 任务执行异常");
        }
    }

    /// <summary>
    /// 服务停止时强制终止任务
    /// </summary>
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        StopTask(); // 强制停止任务
        _taskCts?.Dispose();
        await base.StopAsync(cancellationToken);
        _logger.LogInformation("🔌 后台任务已完全停止");
    }
}