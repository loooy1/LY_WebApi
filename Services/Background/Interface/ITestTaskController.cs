namespace LY_WebApi.Services.Background.Interface
{
    /// <summary>
    /// 控制器接口：Handler 注入此接口来控制后台任务
    /// </summary>
    public interface ITestTaskController
    {
        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="hostCancellation"></param>
        void Start(CancellationToken hostCancellation = default);

        /// <summary>
        /// 停止任务
        /// </summary>
        /// <returns></returns>
        Task StopAsync();

        /// <summary>
        /// 是否正在运行
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// 获取任务运行的取消令牌
        /// </summary>
        /// <returns></returns>
        CancellationToken GetRunToken();
    }
}
