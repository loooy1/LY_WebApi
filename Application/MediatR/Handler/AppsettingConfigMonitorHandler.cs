using LY_WebApi.Common.SerilogExt;
using LY_WebApi.Services.Background;
using LY_WebApi.Services.Background.Interface;
using MediatR;

namespace LY_WebApi.Application.MediatR.Handler
{
    #region 定义Command/Event
    /// <summary>
    /// 命令（Command/Request）：用于「请求-响应」，一对一（一个请求对应一个处理器）
    /// </summary>
    public class TaskControlCommand : IRequest<Unit>
    {
        /// <summary>
        /// 启用或禁用任务
        /// </summary>
        public bool Enable { get; set; }
    }

    /// <summary>
    /// 事件（Event/Notification）：用于「发布-订阅」，一对多（一个事件可被多个处理器订阅）
    /// </summary>
    public class TaskControlEvent : INotification
    {
        /// <summary>
        /// 启用或禁用任务
        /// </summary>
        public bool Enable { get; set; }
    }
    #endregion


    #region 定义处理器

    /// <summary>
    /// Appsetting 配置变更监控 处理器
    /// </summary>
    public class AppsettingConfigMonitorHandler : IRequestHandler<TaskControlCommand, Unit>, INotificationHandler<TaskControlEvent>
    {
        // 日志记录器
        private readonly CustomLogger _log;

        // 业务后台任务
        private readonly ITestTaskController _taskController;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AppsettingConfigMonitorHandler(CustomLogger log, [FromKeyedServices("TestTask")] ITestTaskController taskController)
        {
            _log = log;
            _taskController = taskController;
        }

        /// <summary>
        /// 处理 Command
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Unit> Handle(TaskControlCommand request, CancellationToken cancellationToken)
        {
            _log.LogWarn("MediatR", $"MediatR_Handler处理器已收到命令，后台任务已{(request.Enable ? "启用" : "禁用")}，准备下发给服务层");

            if (request.Enable)
                _taskController.Start(cancellationToken);
            else
                _taskController.StopAsync().GetAwaiter().GetResult();

            return Task.FromResult(Unit.Value);
        }

        /// <summary>
        /// 处理 Event 去控制实际逻辑
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task Handle(TaskControlEvent notification, CancellationToken cancellationToken)
        {
            _log.LogWarn("MediatR", $"MediatR_Handler处理器已收到广播，后台任务已{(notification.Enable ? "启用" : "禁用")}，准备下发给服务层");

            if (notification.Enable) 
                _taskController.Start(cancellationToken);
            else 
                _taskController.StopAsync().GetAwaiter().GetResult();

            return Task.CompletedTask;
        }


    }

    /// <summary>
    /// 纯广播事件处理器：测试是否能同时收到广播事件
    /// </summary>
    public class CustomNotificationHandler : INotificationHandler<TaskControlEvent>
    {
        // 【自定义】注入你的依赖（日志、业务服务等）
        private readonly CustomLogger _log; // 复用你封装的日志
                                            // private readonly IYourService _yourService; // 示例：你的业务服务

        /// <summary>
        /// 构造函数（依赖注入）
        /// 【自定义】添加/修改你需要的依赖
        /// </summary>
        public CustomNotificationHandler(CustomLogger log)
        {
            _log = log;
            // _yourService = yourService;
        }

        /// <summary>
        /// 广播事件处理核心方法
        /// 【自定义】补充你的业务逻辑
        /// </summary>
        public Task Handle(TaskControlEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                // 1. 基础日志（保留你的风格）
                _log.LogWarn("MediatR", $"收到纯广播事件 | Enable：{notification.Enable}");

                // 2. 【自定义】添加你的核心业务逻辑
                // 示例：if (notification.Enable) { _yourService.Start(); }
                // 示例：else { _yourService.StopAsync().GetAwaiter().GetResult(); }

                // 3. 成功日志（可选）
                _log.LogInfo("MediatR", "纯广播事件处理完成");
            }
            catch (Exception ex)
            {
                // 4. 异常处理（保留你的风格）
                _log.LogError("MediatR", ex, "纯广播事件处理失败");
                throw; // 按需决定是否抛出（上层捕获/仅记录日志）
            }

            // 纯广播固定返回：无返回值
            return Task.CompletedTask;
        }
    }
    #endregion
}
