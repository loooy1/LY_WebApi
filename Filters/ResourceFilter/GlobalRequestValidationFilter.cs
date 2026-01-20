using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IO;
using System.Text;
using LY_WebApi.Common.Response;

namespace LY_WebApi.Filters.ResourceFilter
{
    /// <summary>
    /// 全局资源过滤器：校验 POST/PUT 请求的 Content-Type、编码规范
    /// </summary>
    public class GlobalRequestValidationFilter : IAsyncResourceFilter
    {
        /// <summary>
        /// 在资源执行前校验请求的 Content-Type、编码规范及请求体
        /// </summary>
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var request = context.HttpContext.Request;
            var method = request.Method.ToUpper();
            // ========== ↓↓↓ 极简请求头校验 ↓↓↓ ==========
            if (method == "POST" || method == "PUT")
            {
                var contentType = request.ContentType?.ToLower() ?? "";
                var charset = request.Headers["charset"].ToString().ToLower() ?? "";

                if (!(contentType == "application/json"))
                {
                    context.Result = new BadRequestObjectResult(ApiResponse.Fail(msg: "请求头 Content-Type 必须为 application/json", 400));
                    return;
                }

                if (!(charset == "utf-8"))
                {
                    context.Result = new BadRequestObjectResult(ApiResponse.Fail(msg: "请求头 charset 必须指定 utf-8 编码", 400));
                    return;
                }
            }
            await next();
        }
    }
}