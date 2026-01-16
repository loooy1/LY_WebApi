using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using System.Reflection;
using LY_WebApi.Common;

namespace LY_WebApi.Filters.ResourceFilter
{
    /// <summary>
    /// 严格字段校验资源过滤器。
    /// 用于校验请求体中的字段必须且只能包含 TDto 类型中定义的所有属性，
    /// 否则返回 400 错误。
    /// </summary>
    /// <typeparam name="TDto">用于校验的 DTO 类型</typeparam>
    public class StrictFieldValidationFilter<TDto> : IAsyncResourceFilter where TDto : class
    {
        /// <summary>
        /// 资源执行前校验请求体字段。
        /// </summary>
        /// <param name="context">资源执行上下文</param>
        /// <param name="next">下一个资源过滤器委托</param>
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            // 获取当前 HTTP 请求对象
            var request = context.HttpContext.Request;

            // 启用请求体流的多次读取功能，防止流被读取后无法再次访问
            request.EnableBuffering();

            // 创建 StreamReader 读取请求体内容，leaveOpen: true 保证读取后流不被关闭
            using var reader = new StreamReader(request.Body, leaveOpen: true);

            // 异步读取请求体的全部内容到字符串变量 body
            var body = await reader.ReadToEndAsync();

            // 读取完毕后，将请求体流的位置重置到起始位置，便于后续处理中间件或过滤器再次读取
            request.Body.Position = 0;

            // 校验请求体是否为空
            if (string.IsNullOrWhiteSpace(body))
            {
                context.Result = new BadRequestObjectResult(ApiResponse.Fail(msg: "请求体不能为空", 400));
                return;
            }

            // 获取 TDto 所有公开属性名（小写）
            var allowedFields = typeof(TDto)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => p.Name.ToLower())
                .ToHashSet();

            // 解析请求体 JSON 字段名（小写）
            var json = JsonDocument.Parse(body);
            var root = json.RootElement;
            var fields = root.EnumerateObject().Select(p => p.Name.ToLower()).ToHashSet();

            // 校验字段是否完全一致
            if (!allowedFields.SetEquals(fields))
            {
                context.Result = new BadRequestObjectResult
                    (ApiResponse.Fail(msg: $"字段不符合要求，必须且只能包含：{string.Join(",", allowedFields)}", 400));

                return;
            }

            await next();
        }
    }
}