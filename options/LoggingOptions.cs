namespace LY_WebApi.options
{
    /// <summary>
    /// 日志配置（对应配置文件 Logging 节点）
    /// </summary>
    public class LoggingOptions
    {
        /// <summary>
        /// 配置节点名
        /// </summary>
        public const string SectionName = "Logging";

        /// <summary>
        /// 日志级别配置
        /// </summary>
        public LogLevelOptions LogLevel { get; set; } = new LogLevelOptions();
    }

    /// <summary>
    /// 日志级别子配置
    /// </summary>
    public class LogLevelOptions
    {
        /// <summary>
        /// 默认日志级别
        /// </summary>
        public string Default { get; set; } = "Information";

        /// <summary>
        /// ASP.NET Core 框架日志级别
        /// </summary>
        public string MicrosoftAspNetCore { get; set; } = "Warning";
    }
}
