此项目为学习Web_API而创建的示例项目。




app.UseAuthorization(); 所有带use的都是中间件

MVC也是一个中间件。
客户端发送 POST/GET 请求 → 中间件管道（跨域、认证等）；
路由匹配到 ShirtsController 的 AddShirts Action（控制器）；
过滤器执行校验（如 Gender/Size 规则）；
控制器调用 Model（Shirts 类、业务逻辑）处理数据；
控制器返回 IActionResult 响应（无 View 渲染）；
响应经过滤器、中间件返回客户端。