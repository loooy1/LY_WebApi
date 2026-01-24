using System.Threading.Channels;
using global::MediatR;
using LY_WebApi.Common.SerilogExt;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LY_WebApi.Common.MediatR
{
    // 全局任务状态（线程安全，供后台任务和处理器共享）
    public static class TaskStatus
    {
        // 初始值和appsettings一致，后续由MediatR修改
        public static bool IsRunning { get; set; } = true;
    }

    // MediatR处理器：处理启停指令，更新全局状态
    public class TaskControlCommandHandler : IRequestHandler<TaskControlCommand>
    {
        private readonly CustomLogger _logger;
        private readonly Channel<TaskControlCommand> _commandChannel;

        public TaskControlCommandHandler(CustomLogger logger, Channel<TaskControlCommand> commandChannel)
        {
            _logger = logger;
            _commandChannel = commandChannel;
        }

        public async Task Handle(TaskControlCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInfo("MediatR测试", $"📢 MediatR 指令接收：Enable={request.Enable}");

            // 将指令放入 Channel，供 TimedBackgroundTask 获取
            await _commandChannel.Writer.WriteAsync(request, cancellationToken);

            _logger.LogInfo("MediatR测试", $"📢 MediatR 指令已放入 Channel：{(request.Enable ? "启动" : "停止")} 任务");
        }
    }
}
