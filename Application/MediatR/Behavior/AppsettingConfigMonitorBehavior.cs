using System.Threading;
using System.Threading.Tasks;
using LY_WebApi.Common.SerilogExt;
using MediatR;
using System.Collections.Generic;

namespace LY_WebApi.Application.MediatR.Behavior
{
    /// <summary>
    /// 极简版MediatR命令管道行为：记录请求执行日志
    /// </summary>
    /// <typeparam name="TRequest">MediatR请求类型（命令/查询）</typeparam>
    /// <typeparam name="TResponse">MediatR响应类型</typeparam>
    public class AppsettingConfigMonitorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        // 仅保留核心依赖：日志组件
        private readonly CustomLogger _logger; // 增加日志

        // 极简构造函数：仅注入日志
        public AppsettingConfigMonitorBehavior(CustomLogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 管道核心执行方法（极简逻辑）
        /// </summary>
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // 1. 执行请求前：记录开始日志
            var requestName = typeof(TRequest).Name;
            _logger.LogInfo("MediatR", $"开始执行请求：{requestName}");

            try
            {
                // 2. 执行核心业务逻辑（调用下一个管道/处理器）
                var response = await next();

                // 3. 执行成功：记录结束日志
                _logger.LogInfo("MediatR", $"请求 {requestName} 执行完成");
                return response;
            }
            catch (Exception ex)
            {
                // 4. 执行异常：记录错误日志并抛异常
                _logger.LogError("MediatR", ex, $"请求 {requestName} 执行失败");
                throw;
            }
        }
    }

    /// <summary>
    /// 通知发布器：Publish（INotification）前后日志
    /// </summary>
    public class AppsettingConfigMonitorNotificationPublisher : INotificationPublisher
    {
        private readonly CustomLogger _logger;

        public AppsettingConfigMonitorNotificationPublisher(CustomLogger logger)
        {
            _logger = logger;
        }

        public async Task Publish(
            IEnumerable<NotificationHandlerExecutor> handlerExecutors,
            INotification notification,
            CancellationToken cancellationToken)
        {
            var notificationName = notification.GetType().Name;
            _logger.LogInfo("MediatR", $"开始执行广播：{notificationName}");

            try
            {
                foreach (var executor in handlerExecutors)
                {
                    await executor.HandlerCallback(notification, cancellationToken);
                }

                _logger.LogInfo("MediatR", $"广播 {notificationName} 执行完成");
            }
            catch (Exception ex)
            {
                _logger.LogError("MediatR", ex, $"广播 {notificationName} 执行失败");
                throw;
            }
        }
    }
}