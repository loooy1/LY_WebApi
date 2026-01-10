using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LY_WebApi.Filter.ExceptionFilters
{
    /// <summary>
    /// 异常过滤器
    /// </summary>
    public class Shirts_UpdateExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);

            // 创建一个通用的错误响应
            var result = new ObjectResult(new { Message = "发生了一个异常，请检查。" })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            // 将响应结果设置到异常上下文中
            context.Result = result;
        }

    }
}
