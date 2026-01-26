// 仅保留必需命名空间
using System;
using System.IO;
using System.Text;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

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
            // 启用 SelfLog（排查 Serilog 内部错误）
            Serilog.Debugging.SelfLog.Enable(msg =>
            {
                try
                {
                    File.AppendAllText("serilog-selflog.txt", $"{DateTime.Now:O} {msg}{Environment.NewLine}");
                }
                catch (Exception ex)
                {
                    // 使用项目自定义的线程安全控制台打印，避免与其它控制台输出冲突
                    GeneralMethod.PrintError($"Serilog SelfLog 写入失败: {ex.Message}. 原始SelfLog: {msg}");
                }
            });

            // 简化日志模板（去掉 SourceContext）
            var logTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] " +
                              "[{Level:u3}] " +
                              "[应用:{Application}] " +  // 应用名
                              "{Message:lj}{NewLine}{Exception:Detailed}";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                // 过滤冗余日志
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .MinimumLevel.Override("System.Net.Http", LogEventLevel.Error)
                .MinimumLevel.Override("System", LogEventLevel.Error)
                .Enrich.WithProperty("Application", "LY_WebApi") // 应用名
                 // 控制台输出
                .WriteTo.Sink(new GeneralMethodSink())
                // 后台任务日志（predicate 改为安全的模式匹配，避免强转异常）
                .WriteTo.Conditional(e =>
                    e.Properties.TryGetValue("Folder", out var v) &&
                    (v is ScalarValue sv && sv.Value?.ToString() == "后台任务"),
                    wt => wt.File("Logs/后台任务/log-.txt",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: logTemplate,
                        retainedFileCountLimit: 30,
                        encoding: Encoding.UTF8))
                // 前端任务日志
                .WriteTo.Conditional(e =>
                    e.Properties.TryGetValue("Folder", out var v) &&
                    (v is ScalarValue sv2 && sv2.Value?.ToString() == "MediatR"),
                    wt => wt.File("Logs/MediatR/log-.txt",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: logTemplate,
                        retainedFileCountLimit: 30,
                        encoding: Encoding.UTF8))
                // 数据库操作日志
                .WriteTo.Conditional(e =>
                    e.Properties.TryGetValue("Folder", out var v) &&
                    (v is ScalarValue sv3 && sv3.Value?.ToString() == "TestTask"),
                    wt => wt.File("Logs/TestTask/log-.txt",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: logTemplate,
                        retainedFileCountLimit: 30,
                        encoding: Encoding.UTF8))
                // API调用日志
                .WriteTo.Conditional(e =>
                    e.Properties.TryGetValue("Folder", out var v) &&
                    (v is ScalarValue sv4 && sv4.Value?.ToString() == "API调用"),
                    wt => wt.File("Logs/API调用/log-.txt",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: logTemplate,
                        retainedFileCountLimit: 30,
                        encoding: Encoding.UTF8))
                // 默认日志（当没有 Folder 属性时写默认）
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
            // 使用 ForContext 写入，确保属性随该日志事件一起发送到 sinks
            Log.ForContext("Folder", folder)
               .ForContext("UserId", userId)
               .Error(ex, message, args);
        }

        /// <summary>
        /// Warning级别日志
        /// </summary>
        public void LogWarn(string folder, string message, string userId = "", params object[] args)
        {
            WriteLogToFolder(folder, LogEventLevel.Warning, message, userId, args);
        }

        /// <summary>
        /// 核心方法：写入指定文件夹（使用 ForContext，避免作用域/线程问题）
        /// </summary>
        private void WriteLogToFolder(string folder, LogEventLevel level, string message, string userId = "", params object[] args)
        {
            // 采用 ForContext 绑定属性到这个写入调用，可靠且线程/异步安全
            var logger = Log.ForContext("Folder", folder).ForContext("UserId", userId);
            logger.Write(level, message, args);
        }
    }

    /// <summary>
    /// 自定义 Serilog sink：把日志通过 GeneralMethod 打印到控制台（线程安全、一致格式）
    /// </summary>
    internal class GeneralMethodSink : ILogEventSink, IDisposable
    {
        private readonly IFormatProvider? _formatProvider;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="formatProvider">_formatProvider 用来控制日志消息中参数（数字、日期、货币等）
        /// 的区域性/格式化行为</param>
        public GeneralMethodSink(IFormatProvider? formatProvider = null)
        {
            _formatProvider = formatProvider;
        }

        /// <summary>
        /// 日志事件处理
        /// </summary>
        /// <param name="logEvent"></param>
        public void Emit(LogEvent logEvent)
        {
            if (logEvent == null) return;

            // Render message (包含模板渲染的参数)
            string message;
            try
            {
                message = logEvent.RenderMessage(_formatProvider);
            }
            catch
            {
                // 回退到简单拼接
                message = logEvent.MessageTemplate?.Text ?? string.Empty;
            }

            // 包含异常信息（若有）
            if (logEvent.Exception != null)
            {
                message = $"{message} | Exception: {logEvent.Exception}";
            }

            // 把格式化好的信息交给 GeneralMethod，按级别选择打印方法
            switch (logEvent.Level)
            {
                case LogEventLevel.Error:
                    GeneralMethod.PrintError($"{message}");
                    break;
                case LogEventLevel.Warning:
                    GeneralMethod.PrintWarning($"{message}");
                    break;
                case LogEventLevel.Information:
                    GeneralMethod.PrintInfo($"{message}");
                    break;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            // nothing to dispose for now
        }
    }
}