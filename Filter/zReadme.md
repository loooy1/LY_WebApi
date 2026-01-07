# ValidationProblemDetails 用于标准化API错误响应

```
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


