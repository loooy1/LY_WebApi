using LY_WebApi.Filters;
using LY_WebApi.options;
using LY_WebApi_SwaggerSetting.SwaggerExt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using LY_WebApi.Filters;
namespace LY_WebApi.Controllers
{
    /// <summary>
    /// 控制器注释
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = ApiVersionInfo.V1)]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly BaseSettingOptions _baseSettingOptions;
        public TestController(ILogger<TestController> logger,IOptionsMonitor<BaseSettingOptions> options)
        {
            _logger = logger;
            _baseSettingOptions = options.CurrentValue;
            _logger.LogInformation("这是Test构造函数---");
        }

        // ✅ 新增Action：手动访问触发日志
        //[CustomResourceFilter]
        //[CustomCacheAsyncResourceFilter]
        [HttpGet("TestLog")]
        [CustomResultFilterAttribute]
        
        public IActionResult TestLog()
        {
            _logger.LogInformation("这是TestLog Action的测试日志---");
            return Ok("Test控制器已触发");
        }

    }
}
