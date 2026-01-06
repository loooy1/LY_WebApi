using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LY_WebApi.Filters
{
    /// <summary>
    /// 作用：第一次访问后，后续请求直接返回结果 不再执行Action中的逻辑
    /// - Attribute：可作为特性标记在Controller/Action上
    /// - IAsyncResourceFilter：异步资源过滤器（请求管道中执行最早的过滤器）
    /// </summary>
    public class CustomCacheAsyncResourceFilterAttribute : Attribute, IAsyncResourceFilter
    {
        // 2. 内存缓存容器：静态字典（全局共享，应用重启后清空）
        // key：请求路径（如 /api/Test/TestLog），value：Action的响应结果（IActionResult）
        private static Dictionary<string, object> CacheDictionary = new Dictionary<string, object>();

        // 3. 异步资源过滤器核心方法（整合“执行前/执行后”逻辑）
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            // 步骤1：生成缓存Key（以请求路径作为唯一标识，如 /api/Test/TestLog）
            string key = context.HttpContext.Request.Path;

            // 步骤2：判断缓存是否存在
            if (!CacheDictionary.ContainsKey(key)) // 无缓存 → 执行Action并缓存结果
            {
                // 执行后续请求管道（核心：调用next()才会执行Action逻辑）
                // resourceExecutedContext：Action执行完成后的上下文（包含Action的响应结果）
                ResourceExecutedContext resourceExecutedContext = await next.Invoke();

                // 将Action的响应结果存入缓存字典
                CacheDictionary[key] = resourceExecutedContext.Result;

                // 将Action的结果赋值给当前上下文（确保第一次请求能正常返回结果）
                context.Result = resourceExecutedContext.Result;
            }
            else // 有缓存 → 直接返回缓存结果，不执行Action
            {
                // 从缓存中取出结果，转换为IActionResult并赋值给上下文
                // 此时next()不会执行，Action逻辑被跳过
                context.Result = CacheDictionary[key] as IActionResult;
            }

            // 异步方法收尾：返回已完成的Task（无实际逻辑，仅满足异步语法）
            await Task.CompletedTask;
        }
    }
}
