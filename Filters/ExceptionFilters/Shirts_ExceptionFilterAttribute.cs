using LY_WebApi.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LY_WebApi.Filter.ExceptionFilters
{
    /// <summary>
    /// 自定义全局异常过滤器(控制器/方法级别)
    /// 作用：捕获指定控制器/方法的异常，返回统一格式错误信息，包含控制器+方法+异常详情
    /// </summary>
    public class Shirts_ExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 重写异常处理方法
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            // 1. 获取控制器+方法名
            string controllerName = context.RouteData.Values["controller"]?.ToString() ?? "未知控制器";
            string actionName = context.RouteData.Values["action"]?.ToString() ?? "未知方法";
            string controllerAction = $"({controllerName}Controller)->({actionName})方法";

            // 2.读取真实的异常信息，自动穿透内部异常
            Exception ex = context.Exception;
            // 如果有内部异常，就取内部异常（真实错误），没有就用外层异常
            if (ex.InnerException != null)
            {
                ex = ex.InnerException;
                // 防止有多层内部异常，一直取到最内层的真实异常
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
            }

            // 3. 现在拿到的就是【真实的错误信息】
            string? exceptionType = ex.GetType().FullName; //异常类型
            string? exceptionMsg  = ex.Message;            //异常消息
            string? stackTrace    = ex.StackTrace;         //堆栈信息

            // 4. 封装响应
              var  errorResult = ApiResponse.Fail(
                                                code:500, 
                                                msg: "程序内部发生未处理异常", 
                                                data: new
                                                    {
                                                    //触发位置
                                                    ControllerAndAction = controllerAction,
                                                    //异常类型
                                                    ExceptionType = exceptionType,
                                                    //异常消息
                                                    ExceptionMessage = exceptionMsg 
                                                    }
                                                );
            // 5. 标记异常已处理
            context.Result = new JsonResult(errorResult)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            GeneralMethod.PrintWarning($"程序内部异常:[触发位置]:{controllerAction},[异常类型]:({exceptionType}),[异常消息]:({exceptionMsg})");

            context.ExceptionHandled = true;

            base.OnException(context);
        }
    }
}
