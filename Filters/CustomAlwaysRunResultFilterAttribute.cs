using Microsoft.AspNetCore.Mvc.Filters;

namespace LY_WebApi.Filters
{
    /// <summary>
    /// 区别于ResultFilter 这个无论如何都会在结果前后执行
    /// </summary>
    public class CustomAlwaysRunResultFilterAttribute : Attribute, IAlwaysRunResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
             
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
             
        }
    }
}
