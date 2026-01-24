using MediatR;
namespace LY_WebApi.Common.MediatR
{
    /// <summary>
    /// MediatR启停指令（一对一，控制任务状态）
    /// </summary>
    public class TaskControlCommand : IRequest
    {
        /// <summary>
        /// 是否启用任务 true=启动，false=停止
        /// </summary>
        public bool Enable { get; set; } 
    }
}
