using Microsoft.AspNetCore.Mvc.Filters;

namespace LY_WebApi.Filters
{
    /// <summary>
    /// 是一个特性
    /// 实现IActionFilter接口 
    /// 作用 在控制器前后加逻辑 不包裹控制器构造函数 和ResourceFilter有区别
    /// </summary>
    public class CustomActionFilterAttribute : Attribute, IActionFilter
    {
        /// <summary>
        /// 在XX方法前执行
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("CustomActionFilterAttribute.OnActionExecuting");
        }

        /// <summary>
        /// 在XX方法后执行
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("CustomActionFilterAttribute.OnActionExecuted");
        }
    }

    /// <summary>
    /// ActionFilter的异步方法
    /// </summary>
    public class CustomAsyncActionFilterAttribute : Attribute, IAsyncActionFilter
    {
        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            throw new NotImplementedException();
        }
    }


    //public class CustomAsyncActionNewFilterAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuted(ActionExecutedContext context)
    //    {
    //        base.OnActionExecuted(context);
    //    }
    //}
}
