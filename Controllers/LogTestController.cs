using LY_WebApi.options;
using LY_WebApi_SwaggerSetting.SwaggerExt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LY_WebApi.Controllers
{
    /// <summary>
    /// 控制器注释
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = ApiVersionInfo.V1)]
    public class LogTestController : ControllerBase
    {
        private readonly ILogger<LogTestController> _logger;
        private readonly BaseSettingOptions _baseSettingOptions;
        public LogTestController(ILogger<LogTestController> logger,IOptionsMonitor<BaseSettingOptions> options)
        {
            _logger = logger;
            _baseSettingOptions = options.CurrentValue;
            Console.WriteLine($"_baseSettingOptions.LanguageType:{_baseSettingOptions.LanguageType}");
            _logger.LogInformation("这是测试构造函数---");
        }

        // ✅ 新增Action：手动访问触发日志
        [HttpGet("TestLog")]
        public IActionResult TestLog()
        {
            _logger.LogInformation("这是TestLog Action的测试日志---");
            _logger.LogError(new Exception("测试异常"), "这是带异常的错误日志---");
            return Ok("日志已触发");
        }

    }
}
