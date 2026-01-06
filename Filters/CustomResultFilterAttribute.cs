using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LY_WebApi.Filters
{
    /// <summary>
    /// ResultFilter 在返回结果前和返回结果后触发
    /// </summary>
    public class CustomResultFilterAttribute : Attribute, IResultFilter
    { 
        public void OnResultExecuting(ResultExecutingContext context)
        {
            Console.WriteLine("CustomResultFilterAttribute.OnResultExecuting");
            // 主动获取并打印响应内容

            // 1.判断结果类型是 OkObjectResult（对应 return Ok(...)）
            if (context.Result is OkObjectResult okResult)
                {
                    // 2. 读取原有响应内容（可选）
                    var oldContent = okResult.Value?.ToString();
                    Console.WriteLine($"【原有响应内容】：{oldContent}");

                    // 3. 修改响应内容（直接赋值 Value 属性）
                    okResult.Value = "修改后的响应内容：Test控制器触发成功！";

                    //// 4. 可选：修改其他属性（如状态码、响应头）
                    //okResult.StatusCode = 200; // 可改成其他合法状态码（如 201）
                    //context.HttpContext.Response.Headers.Add("X-Modified", "true"); // 添加响应头
                }
            
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            Console.WriteLine("CustomResultFilterAttribute.OnResultExecuted");
        } 
    }


    /// <summary>
    /// 异步版本的实现
    /// </summary>
    public class CustomAsyncResultFilterAttribute : Attribute, IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            await next();
        }
    }
}
