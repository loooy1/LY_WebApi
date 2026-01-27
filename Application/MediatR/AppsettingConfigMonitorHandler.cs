using LY_WebApi.Common.SerilogExt;
using LY_WebApi.Services.Background;
using LY_WebApi.Services.Background.Interface;
using MediatR;

namespace LY_WebApi.Application
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
            throw new NotImplementedException();
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
    #endregion
}
