// 仅保留必需命名空间
using Serilog;
using Serilog.Events;
using Serilog.Context;
using System.Text;

namespace LY_WebApi.Common.SerilogExt
{
    /// <summary>
    /// Serilog扩展方法
    /// </summary>
    public static class SerilogExtensions
    {
        /// <summary>
        /// 配置Serilog（简化版，去掉 SourceContext）
        /// </summary>
        public static void ConfigureSerilogExt(this WebApplicationBuilder builder)
        {
            // 简化日志模板（去掉 SourceContext）
            var logTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] " +
                              "[{Level:u3}] " +
                              "[应用:{Application}] " +  // 应用名
                              "{Message:lj}{NewLine}{Exception:Detailed}{NewLine}";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                // 过滤冗余日志
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .MinimumLevel.Override("System.Net.Http", LogEventLevel.Error)
                .MinimumLevel.Override("System", LogEventLevel.Error)
                .Enrich.WithProperty("Application", "LY_WebApi") // 应用名
                // 控制台输出
                .WriteTo.Console(outputTemplate: logTemplate)
                // 后台任务日志
                .WriteTo.Conditional(e => e.Properties.ContainsKey("Folder") && ((ScalarValue)e.Properties["Folder"]).Value.ToString() == "后台任务",
                    wt => wt.File("Logs/后台任务/log-.txt",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: logTemplate,
                        retainedFileCountLimit: 30,
                        encoding: Encoding.UTF8))
                // 前端任务日志
                .WriteTo.Conditional(e => e.Properties.ContainsKey("Folder") && ((ScalarValue)e.Properties["Folder"]).Value.ToString() == "前端任务",
                    wt => wt.File("Logs/前端任务/log-.txt",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: logTemplate,
                        retainedFileCountLimit: 30,
                        encoding: Encoding.UTF8))
                // 数据库操作日志
                .WriteTo.Conditional(e => e.Properties.ContainsKey("Folder") && ((ScalarValue)e.Properties["Folder"]).Value.ToString() == "数据库操作",
                    wt => wt.File("Logs/数据库操作/log-.txt",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: logTemplate,
                        retainedFileCountLimit: 30,
                        encoding: Encoding.UTF8))
                // API调用日志
                .WriteTo.Conditional(e => e.Properties.ContainsKey("Folder") && ((ScalarValue)e.Properties["Folder"]).Value.ToString() == "API调用",
                    wt => wt.File("Logs/API调用/log-.txt",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: logTemplate,
                        retainedFileCountLimit: 30,
                        encoding: Encoding.UTF8))
                // 默认日志
                .WriteTo.Conditional(e => !e.Properties.ContainsKey("Folder"),
                    wt => wt.File("Logs/default/log-.txt",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: logTemplate,
                        retainedFileCountLimit: 30,
                        encoding: Encoding.UTF8))
                .CreateLogger();

            builder.Host.UseSerilog();
            // 注册自定义日志类
            builder.Services.AddSingleton<CustomLogger>();
        }
    }

    /// <summary>
    /// 自定义日志类（简化版，支持文件夹和用户ID）
    /// </summary>
    public class CustomLogger
    {
        /// <summary>
        /// Info级别日志
        /// </summary>
        /// <param name="folder">子文件夹</param>
        /// <param name="message">日志内容</param>
        /// <param name="userId">可选：用户ID</param>
        /// <param name="args">格式化参数</param>
        public void LogInfo(string folder, string message, string userId = "", params object[] args)
        {
            WriteLogToFolder(folder, LogEventLevel.Information, message, userId, args);
        }

        /// <summary>
        /// Debug级别日志
        /// </summary>
        public void LogDebug(string folder, string message, string userId = "", params object[] args)
        {
            WriteLogToFolder(folder, LogEventLevel.Debug, message, userId, args);
        }

        /// <summary>
        /// Error级别日志（带异常）
        /// </summary>
        public void LogError(string folder, Exception ex, string message, string userId = "", params object[] args)
        {
            using (LogContext.PushProperty("Folder", folder))
            using (LogContext.PushProperty("UserId", userId))
            {
                Log.Error(ex, message, args);
            }
        }

        /// <summary>
        /// Warning级别日志
        /// </summary>
        public void LogWarn(string folder, string message, string userId = "", params object[] args)
        {
            WriteLogToFolder(folder, LogEventLevel.Warning, message, userId, args);
        }

        /// <summary>
        /// 核心方法：写入指定文件夹
        /// </summary>
        private void WriteLogToFolder(string folder, LogEventLevel level, string message, string userId = "", params object[] args)
        {
            using (LogContext.PushProperty("Folder", folder))
            using (LogContext.PushProperty("UserId", userId))
            {
                Log.Write(level, message, args);
            }
        }
    }
}