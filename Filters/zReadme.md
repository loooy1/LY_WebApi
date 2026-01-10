# ValidationProblemDetails 用于标准化API错误响应
```示例
代码示例
context.ModelState.AddModelError("id", "衬衫id必须大于等于0");
//用于标准化API错误响应
var problemDetails = new ValidationProblemDetails(context.ModelState)
{
                        Status = StatusCodes.Status400BadRequest
};

context.Result = new BadRequestObjectResult(problemDetails);

响应结果：
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "id": [
      "衬衫id必须大于等于0"
    ]
  }
}
```

# 个人认为过滤器可以分为 控制器方法过滤器(继承自ActionAttribute) 和 实体类属性过滤器(继承自ValidationAttribute,非.NET框架也可以使用)。
``` 示例
    /// <summary>
    /// 验证ShirtId属性
    /// </summary>
    public class Shirts_ShirtIdValidationAttribute : ValidationAttribute
    {
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    }

    /// <summary>
    /// 方法过滤器 仅用于修饰控制器方法
    /// </summary>
    public class Shirts_ValidateShirtIdFilterAttribute : ActionFilterAttribute
    {
    //方法执行前执行
    public override void OnActionExecuting(ActionExecutingContext context)

    //方法执行后执行
    public override void OnActionExecuted(ActionExecutedContext context)
    }
```

# 自动短路
```
在控制器上标记了 [ApiController],之后 会先进行属性验证，验证失败直接返回http响应
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "GuidId": [
      "The GuidId field is required."
    ]
  },
  "traceId": "00-66d226af9475d974f9e5c2aa4a239742-907873e4cc53fdc2-00"
}

如果想返回更标准的http错误响应，就要设置：
builder.Services.AddControllers(options =>
{
    // 关闭 [ApiController] 的自动验证短路
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    options.SuppressModelStateInvalidFilter = true; // 核心：禁用自动400返回
});



