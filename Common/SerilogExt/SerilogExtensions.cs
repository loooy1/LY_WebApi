// 仅保留必需命名空间
using System;
using System.IO;
using System.Text;
using Serilog;
//using Serilog.Context;
//using Serilog.Core;
//using Serilog.Events;

//namespace LY_WebApi.Common.SerilogExt
//{
//    /// <summary>
//    /// Serilog扩展方法
//    /// </summary>
//    public static class SerilogExtensions
//    {
//        /// <summary>
//        /// 配置Serilog（简化版，去掉 SourceContext）
//        /// </summary>
//        public static void ConfigureSerilogExt(this WebApplicationBuilder builder)
//        {
//            // 启用 SelfLog（排查 Serilog 内部错误）
//            Serilog.Debugging.SelfLog.Enable(msg =>
//            {
//                try
//                {
//                    File.AppendAllText("serilog-selflog.txt", $"{DateTime.Now:O} {msg}{Environment.NewLine}");
//                }
//                catch (Exception ex)
//                {
//                    // 使用项目自定义的线程安全控制台打印，避免与其它控制台输出冲突
//                    GeneralMethod.PrintError($"Serilog SelfLog 写入失败: {ex.Message}. 原始SelfLog: {msg}");
//                }
//            });

//            // 简化日志模板（去掉 SourceContext）
//            var logTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] " +
//                              "[{Level:u3}] " +
//                              "[应用:{Application}] " +  // 应用名
//                              "{Message:lj}{NewLine}{Exception:Detailed}";

//            Log.Logger = new LoggerConfiguration()
//                .MinimumLevel.Debug()
//                // 过滤冗余日志
//                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
//                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
//                .MinimumLevel.Override("System.Net.Http", LogEventLevel.Error)
//                .MinimumLevel.Override("System", LogEventLevel.Error)
//                .Enrich.WithProperty("Application", "LY_WebApi") // 应用名
//                 // 控制台输出
//                .WriteTo.Sink(new GeneralMethodSink())
//                // 后台任务日志（predicate 改为安全的模式匹配，避免强转异常）
//                .WriteTo.Conditional(e =>
//                    e.Properties.TryGetValue("Folder", out var v) &&
//                    (v is ScalarValue sv && sv.Value?.ToString() == "后台任务"),
//                    wt => wt.File("Logs/后台任务/log-.txt",
//                        rollingInterval: RollingInterval.Day,
//                        outputTemplate: logTemplate,
//                        retainedFileCountLimit: 30,
//                        encoding: Encoding.UTF8))
//                // 前端任务日志
//                .WriteTo.Conditional(e =>
//                    e.Properties.TryGetValue("Folder", out var v) &&
//                    (v is ScalarValue sv2 && sv2.Value?.ToString() == "MediatR"),
//                    wt => wt.File("Logs/MediatR/log-.txt",
//                        rollingInterval: RollingInterval.Day,
//                        outputTemplate: logTemplate,
//                        retainedFileCountLimit: 30,
//                        encoding: Encoding.UTF8))
//                // 数据库操作日志
//                .WriteTo.Conditional(e =>
//                    e.Properties.TryGetValue("Folder", out var v) &&
//                    (v is ScalarValue sv3 && sv3.Value?.ToString() == "TestTask"),
//                    wt => wt.File("Logs/TestTask/log-.txt",
//                        rollingInterval: RollingInterval.Day,
//                        outputTemplate: logTemplate,
//                        retainedFileCountLimit: 30,
//                        encoding: Encoding.UTF8))
//                // API调用日志
//                .WriteTo.Conditional(e =>
//                    e.Properties.TryGetValue("Folder", out var v) &&
//                    (v is ScalarValue sv4 && sv4.Value?.ToString() == "API调用"),
//                    wt => wt.File("Logs/API调用/log-.txt",
//                        rollingInterval: RollingInterval.Day,
//                        outputTemplate: logTemplate,
//                        retainedFileCountLimit: 30,
//                        encoding: Encoding.UTF8))
//                // 默认日志（当没有 Folder 属性时写默认）
//                .WriteTo.Conditional(e => !e.Properties.ContainsKey("Folder"),
//                    wt => wt.File("Logs/default/log-.txt",
//                        rollingInterval: RollingInterval.Day,
//                        outputTemplate: logTemplate,
//                        retainedFileCountLimit: 30,
//                        encoding: Encoding.UTF8))
//                .CreateLogger();

//            builder.Host.UseSerilog();
//            // 注册自定义日志类
//            builder.Services.AddSingleton<CustomLogger>();
//        }
//    }

//    /// <summary>
//    /// 自定义日志类（简化版，支持文件夹和用户ID）
//    /// </summary>
//    public class CustomLogger
//    {
//        /// <summary>
//        /// Info级别日志
//        /// </summary>
//        /// <param name="folder">子文件夹</param>
//        /// <param name="message">日志内容</param>
//        /// <param name="userId">可选：用户ID</param>
//        /// <param name="args">格式化参数</param>
//        public void LogInfo(string folder, string message, string userId = "", params object[] args)
//        {
//            WriteLogToFolder(folder, LogEventLevel.Information, message, userId, args);
//        }

//        /// <summary>
//        /// Debug级别日志
//        /// </summary>
//        public void LogDebug(string folder, string message, string userId = "", params object[] args)
//        {
//            WriteLogToFolder(folder, LogEventLevel.Debug, message, userId, args);
//        }

//        /// <summary>
//        /// Error级别日志（带异常）
//        /// </summary>
//        public void LogError(string folder, Exception ex, string message, string userId = "", params object[] args)
//        {
//            // 使用 ForContext 写入，确保属性随该日志事件一起发送到 sinks
//            Log.ForContext("Folder", folder)
//               .ForContext("UserId", userId)
//               .Error(ex, message, args);
//        }

//        /// <summary>
//        /// Warning级别日志
//        /// </summary>
//        public void LogWarn(string folder, string message, string userId = "", params object[] args)
//        {
//            WriteLogToFolder(folder, LogEventLevel.Warning, message, userId, args);
//        }

//        /// <summary>
//        /// 核心方法：写入指定文件夹（使用 ForContext，避免作用域/线程问题）
//        /// </summary>
//        private void WriteLogToFolder(string folder, LogEventLevel level, string message, string userId = "", params object[] args)
//        {
//            // 采用 ForContext 绑定属性到这个写入调用，可靠且线程/异步安全
//            var logger = Log.ForContext("Folder", folder).ForContext("UserId", userId);
//            logger.Write(level, message, args);
//        }
//    }

//    /// <summary>
//    /// 自定义 Serilog sink：把日志通过 GeneralMethod 打印到控制台（线程安全、一致格式）
//    /// </summary>
//    internal class GeneralMethodSink : ILogEventSink, IDisposable
//    {
//        private readonly IFormatProvider? _formatProvider;

//        /// <summary>
//        /// 构造函数
//        /// </summary>
//        /// <param name="formatProvider">_formatProvider 用来控制日志消息中参数（数字、日期、货币等）
//        /// 的区域性/格式化行为</param>
//        public GeneralMethodSink(IFormatProvider? formatProvider = null)
//        {
//            _formatProvider = formatProvider;
//        }

//        /// <summary>
//        /// 日志事件处理
//        /// </summary>
//        /// <param name="logEvent"></param>
//        public void Emit(LogEvent logEvent)
//        {
//            if (logEvent == null) return;

//            // Render message (包含模板渲染的参数)
//            string message;
//            try
//            {
//                message = logEvent.RenderMessage(_formatProvider);
//            }
//            catch
//            {
//                // 回退到简单拼接
//                message = logEvent.MessageTemplate?.Text ?? string.Empty;
//            }

//            // 包含异常信息（若有）
//            if (logEvent.Exception != null)
//            {
//                message = $"{message} | Exception: {logEvent.Exception}";
//            }

//            // 把格式化好的信息交给 GeneralMethod，按级别选择打印方法
//            switch (logEvent.Level)
//            {
//                case LogEventLevel.Error:
//                    GeneralMethod.PrintError($"{message}");
//                    break;
//                case LogEventLevel.Warning:
//                    GeneralMethod.PrintWarning($"{message}");
//                    break;
//                case LogEventLevel.Information:
//                    GeneralMethod.PrintInfo($"{message}");
//                    break;
//            }
//        }

//        /// <summary>
//        /// 释放资源
//        /// </summary>
//        public void Dispose()
//        {
//            // nothing to dispose for now
//        }
//    }
//}


//=====================================================================================================



// 仅保留必需命名空间
using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace LY_WebApi.Common.SerilogExt
{
    /// <summary>
    /// Serilog配置实体（仅保留动态可配置参数）
    /// </summary>
    public class SerilogConfig
    {
        /// <summary>
        /// 应用名称
        /// </summary>
        public string ApplicationName { get; set; } = "LY_WebApi";

        /// <summary>
        /// 默认日志级别
        /// </summary>
        public LogEventLevel MinimumLevel { get; set; } = LogEventLevel.Debug;

        /// <summary>
        /// 日志文件保留天数
        /// </summary>
        public int RetainedFileCountLimit { get; set; } = 30;

        /// <summary>
        /// 日志输出模板
        /// </summary>
        public string OutputTemplate { get; set; } = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level:u3}] [应用:{Application}] {Message:lj}{NewLine}{Exception:Detailed}";

        /// <summary>
        /// 日志级别覆盖配置
        /// </summary>
        public Dictionary<string, LogEventLevel> LevelOverrides { get; set; } = new Dictionary<string, LogEventLevel>
        {
            { "Microsoft", LogEventLevel.Warning },
            { "Microsoft.EntityFrameworkCore", LogEventLevel.Warning },
            { "System.Net.Http", LogEventLevel.Error },
            { "System", LogEventLevel.Error }
        };
    }

    /// <summary>
    /// Serilog扩展方法（支持实时配置更新，folder硬编码保证业务稳定）
    /// </summary>
    public static class SerilogExtensions
    {
        /// <summary>
        /// 配置Serilog（支持从appsettings实时读取动态参数）
        /// </summary>
        public static void ConfigureSerilogExt(this WebApplicationBuilder builder)
        {
            // 绑定配置
            builder.Services.Configure<SerilogConfig>(builder.Configuration.GetSection("SerilogConfig"));

            // 启用 SelfLog（排查 Serilog 内部错误）
            Serilog.Debugging.SelfLog.Enable(msg =>
            {
                try
                {
                    File.AppendAllText("serilog-selflog.txt", $"{DateTime.Now:O} {msg}{Environment.NewLine}");
                }
                catch (Exception ex)
                {
                    GeneralMethod.PrintError($"Serilog SelfLog 写入失败: {ex.Message}. 原始SelfLog: {msg}");
                }
            });

            // 获取配置并初始化Logger
            var config = builder.Configuration.GetSection("SerilogConfig").Get<SerilogConfig>();
            InitializeLogger(config);

            // 后台任务 监听配置变化，实时更新Logger（仅更新动态参数）
            builder.Services.AddHostedService<SerilogConfigMonitor>();

            builder.Host.UseSerilog();
            // 注册自定义日志类
            builder.Services.AddSingleton<CustomLogger>();
        }

        /// <summary>
        /// 初始化/重新初始化Logger（folder路径硬编码，参数从配置读取）
        /// </summary>
        /// <param name="config">Serilog动态配置</param>
        public static void InitializeLogger(SerilogConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config), "Serilog配置不能为空");

            var loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Is(config.MinimumLevel)
                .Enrich.WithProperty("Application", config.ApplicationName);

            // 应用日志级别覆盖配置（动态）
            foreach (var overrideItem in config.LevelOverrides)
            {
                loggerConfig.MinimumLevel.Override(overrideItem.Key, overrideItem.Value);
            }

            // 控制台输出
            loggerConfig.WriteTo.Sink(new GeneralMethodSink());

            // 后台任务日志（folder硬编码，参数动态）
            loggerConfig.WriteTo.Conditional(e =>
                    e.Properties.TryGetValue("Folder", out var v) &&
                    (v is ScalarValue sv && sv.Value?.ToString() == "后台任务"),
                wt => wt.File("Logs/后台任务/log-.txt",
                    rollingInterval: RollingInterval.Day, //按天切分日志文件
                    outputTemplate: config.OutputTemplate, // 动态模板
                    retainedFileCountLimit: config.RetainedFileCountLimit, // 动态保留天数
                    encoding: Encoding.UTF8));

            // MediatR日志
            loggerConfig.WriteTo.Conditional(e =>
                    e.Properties.TryGetValue("Folder", out var v) &&
                    (v is ScalarValue sv2 && sv2.Value?.ToString() == "MediatR"),
                wt => wt.File("Logs/MediatR/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: config.OutputTemplate,
                    retainedFileCountLimit: config.RetainedFileCountLimit,
                    encoding: Encoding.UTF8));

            // TestTask日志
            loggerConfig.WriteTo.Conditional(e =>
                    e.Properties.TryGetValue("Folder", out var v) &&
                    (v is ScalarValue sv3 && sv3.Value?.ToString() == "TestTask"),
                wt => wt.File("Logs/TestTask/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: config.OutputTemplate,
                    retainedFileCountLimit: config.RetainedFileCountLimit,
                    encoding: Encoding.UTF8));

            // SerilogConfig
            loggerConfig.WriteTo.Conditional(e =>
                    e.Properties.TryGetValue("Folder", out var v) &&
                    (v is ScalarValue sv4 && sv4.Value?.ToString() == "SerilogConfig"),
                wt => wt.File("Logs/SerilogConfig/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: config.OutputTemplate,
                    retainedFileCountLimit: config.RetainedFileCountLimit,
                    encoding: Encoding.UTF8));

            // 默认日志
            loggerConfig.WriteTo.Conditional(e => !e.Properties.ContainsKey("Folder"),
                wt => wt.File("Logs/default/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: config.OutputTemplate,
                    retainedFileCountLimit: config.RetainedFileCountLimit,
                    encoding: Encoding.UTF8));

            // 替换全局Logger
            Log.CloseAndFlush();
            Log.Logger = loggerConfig.CreateLogger();
        }
    }

    /// <summary>
    /// Serilog配置监视器（仅在关键配置真的变化时打印日志）
    /// </summary>
    public class SerilogConfigMonitor : BackgroundService
    {
        private readonly IOptionsMonitor<SerilogConfig> _configMonitor;
        private readonly CustomLogger _log;
        // 保存上一次的配置快照，用于对比差异
        private SerilogConfig _lastConfig;

        public SerilogConfigMonitor(IOptionsMonitor<SerilogConfig> configMonitor, CustomLogger log)
        {
            _configMonitor = configMonitor;
            _log = log;
            // 初始化上次配置为当前配置
            _lastConfig = _configMonitor.CurrentValue;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // 监听配置变化
            _configMonitor.OnChange(newConfig =>
            {
                try
                {
                    // 对比配置是否真的发生变化（只对比关键参数）
                    if (IsConfigChanged(_lastConfig, newConfig))
                    {
                        // 重新初始化Logger
                        SerilogExtensions.InitializeLogger(newConfig);
                        // 仅在配置真变化时打印日志
                        _log.LogWarn("SerilogConfig", "Serilog动态配置已更新！");
                        // 更新上次配置快照
                        _lastConfig = newConfig;
                    }
                }
                catch (Exception ex)
                {
                    _log.LogError("SerilogConfig", ex, "更新Serilog配置失败");
                }
            });

            return Task.CompletedTask;
        }

        /// <summary>
        /// 对比两个配置是否真的发生变化（仅对比关键动态参数）
        /// </summary>
        /// <param name="oldConfig">旧配置</param>
        /// <param name="newConfig">新配置</param>
        /// <returns>是否变化</returns>
        private bool IsConfigChanged(SerilogConfig oldConfig, SerilogConfig newConfig)
        {
            if (oldConfig == null || newConfig == null) return true;

            // 1. 对比核心基础参数
            if (oldConfig.ApplicationName != newConfig.ApplicationName ||
                oldConfig.MinimumLevel != newConfig.MinimumLevel ||
                oldConfig.RetainedFileCountLimit != newConfig.RetainedFileCountLimit ||
                oldConfig.OutputTemplate != newConfig.OutputTemplate)
            {
                return true;
            }

            // 2. 对比 LevelOverrides（日志级别覆盖配置）
            if (oldConfig.LevelOverrides.Count != newConfig.LevelOverrides.Count) return true;

            foreach (var key in oldConfig.LevelOverrides.Keys)
            {
                // 键存在但值不同，或键不存在，都算变化
                if (!newConfig.LevelOverrides.ContainsKey(key) ||
                    oldConfig.LevelOverrides[key] != newConfig.LevelOverrides[key])
                {
                    return true;
                }
            }

            // 所有关键参数都没变化
            return false;
        }
    }

    // 以下 CustomLogger 和 GeneralMethodSink 保持你原有代码不变
    /// <summary>
    /// 自定义日志类（简化版，支持文件夹和用户ID）
    /// </summary>
    public class CustomLogger
    {
        /// <summary>
        /// Info级别日志
        /// </summary>
        /// <param name="folder">子文件夹（硬编码业务标识）</param>
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
        /// 核心方法：写入指定文件夹
        /// </summary>
        private void WriteLogToFolder(string folder, LogEventLevel level, string message, string userId = "", params object[] args)
        {
            var logger = Log.ForContext("Folder", folder).ForContext("UserId", userId);
            logger.Write(level, message, args);
        }
    }

    /// <summary>
    /// 自定义 Serilog sink：把日志通过 GeneralMethod 打印到控制台
    /// </summary>
    internal class GeneralMethodSink : ILogEventSink, IDisposable
    {
        private readonly IFormatProvider? _formatProvider;

        public GeneralMethodSink(IFormatProvider? formatProvider = null)
        {
            _formatProvider = formatProvider;
        }

        public void Emit(LogEvent logEvent)
        {
            if (logEvent == null) return;

            string message;
            try
            {
                message = logEvent.RenderMessage(_formatProvider);
            }
            catch
            {
                message = logEvent.MessageTemplate?.Text ?? string.Empty;
            }

            if (logEvent.Exception != null)
            {
                message = $"{message} | Exception: {logEvent.Exception}";
            }

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

        public void Dispose()
        {
            // 无需释放资源
        }
    }
}