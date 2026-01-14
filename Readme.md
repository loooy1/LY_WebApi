# 此项目为学习Web_API而创建的示例项目。

<details>
<summary>## 理解概念</summary>

```
app.UseAuthorization(); 所有带use的都是中间件

MVC也是一个中间件。
客户端发送 POST/GET 请求 → 中间件管道（跨域、认证等）；
路由匹配到 ShirtsController 的 AddShirts Action（控制器）；
过滤器执行校验（控制器方法过滤器，实体类属性验证过滤器，异常过滤器）；
控制器调用 Model（Shirts 类、业务逻辑）处理数据；
控制器返回 IActionResult 响应（无 View 渲染）；
响应经过滤器、中间件返回客户端。

swagger用于可视化接口信息 在线调试 版本控制


```

</details>

<details>
<summary>## 引用结构</summary>

1. 包(运行在框架上)  
    ```
    EF_Core相关
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.5">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="9.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.5">

    swagger相关
    <PackageReference Include="Swashbuckle.AspNetCore" Version="7.0.0" />
    <PackageReference Include="Asp.Versioning.Mvc" Version="8.1.1" />
    <PackageReference Include="Asp.Versioning.Mvc.ApiExplorer" Version="8.1.1" />
    ```

2. 框架
    ```
    底层框架
    .NET9.0

    web上层框架
    ASP.NET.Core
    ```
3. 项目引用
    ```
    <ItemGroup>
    <ProjectReference Include="类库项目\LY_WebApi_SwaggerSetting\LY_WebApi_SwaggerSetting.csproj" />
    <ProjectReference Include="类库项目\LY_WebAPI_Test\LY_WebAPI_Test.csproj" />
    </ItemGroup>
    ```

4. 分析器/编译器/VS
    ```
    框架或者包自带
    用于检查代码质量问题，报 warning 
    编译器.NET SDK自带，检查代码错误问题，报 error
    VS 集成开发工作台
    ```

</details>

<details>
<summary>## 中间件</summary>

1. 什么是中间件？

    ```
    中间件(Middleware)是ASP.NET Core请求处理管道中的组件，用于处理HTTP请求和响应。
    它们按顺序链接在一起，形成一个处理链，每个中间件可以对请求进行处理、修改或传递给下一个中间件。
    中间件就是函数调用

    ```
2. 中间件种类及作用
    
    内置中间件（框架提供，常用）
    ```
    认证/授权：如 app.UseAuthorization()（验证用户身份/权限，保护端点）。
    路由/端点映射：如 app.MapControllers()（将请求路由到控制器）。
    静态文件服务：app.UseStaticFiles()（提供 CSS/JS/图片等静态资源）。
    Swagger：app.UseSwaggerExt()（生成 API 文档界面）。
    异常处理：app.UseExceptionHandler()（捕获全局异常，返回友好错误页面）。
    CORS：app.UseCors()（允许跨域请求）。
    HTTPS 重定向：app.UseHttpsRedirection()（强制 HTTPS）。
    ```
    自定义中间件

    #内联中间件(临时简单/不好复用)：
    ```
    //#1
    app.Use(async (HttpContext context, RequestDelegate next) =>
    {
        await context.Response.WriteAsync("middle ware#1,before next\r\n");
        await next(context);
        await context.Response.WriteAsync("middle ware#1,after next\r\n");
    });

    //#2
    app.Use(async (HttpContext context, RequestDelegate next) =>
    {
        await context.Response.WriteAsync("middle ware#2,before next\r\n");
        await next(context);
        await context.Response.WriteAsync("middle ware#3,after next\r\n");
    });

    输出：
    middle ware#1,before next
    middle ware#2,before next
    middle ware#3,after next
    middle ware#1,after next

    解释：
    1. 请求进来时，先经过中间件#1，输出"before next"，然后调用next(context)传递给下一个中间件
    2. 中间件#2接收到请求，输出"before next"，然后调用next(context)传递给下一个中间件
    3. 没有更多中间件了，请求开始返回，先执行中间件#2的"after next"，然后返回到中间件#1
    
    ps：内联中间件如果不调用 next(context)，请求就不会传递到下一个中间件，形成终结中间件。
    ```


    #专用中间件(独立类，易复用/维护)：
    todo


    终结中间件（不调用下游）
    ```
    app.Run(async (HttpContext context) =>
    {
        await context.Response.WriteAsync("This is the terminal middleware.\r\n");
    });
    ```



</details>


<details>
<summary>## EFcore操作数据库</summary>

1. 需要nuget相关的包
    ```
    Pomelo.EntityFrameworkCore.MySql 9.0.5
    Microsoft.EntityFrameworkCore.Tools 9.0.5
    Microsoft.EntityFrameworkCore.Design 9.0.5
    ```
2. 步骤
    ```
    1. appsettings.json  → 存放MySQL连接字符串（账号、密码、库名）
    2. Program.cs        → 读取连接字符串 + 注册AppDbContext + 配置MySQL驱动
    3. AppDbContext      → 接收配置 + 映射Shirts表 + 配置种子数据
    4. 迁移命令          → 执行AppDbContext的配置，在MySQL创建表+插入种子数据
    5. 控制器Controller  → 注入AppDbContext，调用它的方法操作数据库
    6. MySQL数据库       → 接收SQL指令，返回数据结果
    ```
3. 迁移命令
    ```
    1. Add-Migration Init1 // 生成迁移文件
    2. update-database // 更新数据库
    
    ps1:如果想修改数据库的结构，就要先修改实体类，然后重新生成迁移文件，再更新数据库
    ps2:更新数据库结构的话记得修改数据库的种子数据
    ps3:修改主键的话要麻烦一些，要修改迁移文件(因为efcore会默认删除主键添加修改后的主键)
    ```

</details>

<details>
<summary>## 架构_分层</summary>

```
控制器层(controller) ----> 业务层(services)  ----> 仓储层(Repository) 
----> 数据访问层(appContext) ----> 数据库(Mysql)

```

</details>

<details>
<summary>## FAQ</summary>

1. 程序包管理器控制台和PowerShell控制台有什么区别？
    ```
    程序包管理器控制台：微软专为VS开发适配的工具，天生就「认识」所有 NuGet 包命令、EF Core 迁移命令（Add-Migration/Update-Database），不用任何额外配置、不用装全局工具
    PowerShell控制台：Windows 系统自带的通用终端，VS 里的「终端」只是把系统的 PowerShell 嵌入到 VS 里了，通用工具

    EFcore迁移命令：
    =============== 目标操作 ================
    1. 生成EF迁移文件
    2. 更新数据库到MySQL（创建表+种子数据）

    =============== 包管理器控制台 (PMC) ================ 
    直接写，无任何前缀，原生支持
    Add-Migration Init2
    Update-Database

    =============== PowerShell / VS终端 ================
    必须加【dotnet ef】前缀，这是通用终端的EF命令标准写法
    dotnet ef migrations add Init2
    dotnet ef database update
    ```
2.  为什么http响应的json数据是驼峰法命名，和定义的属性名不同？
    ```
    因为默认json序列化是驼峰法，此处设置会关闭默认命名
    // 添加控制器服务
    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        // ========== 核心配置1：关闭自动驼峰命名转换 → C#属性名 原样输出到JSON ==========
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    }
    );

3. .net框架默认返回的响应格式冗余
    ```
    可在这修改默认的400响应，用中间件去修改其他默认的响应
        builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
    {
        // 覆盖框架默认的400响应逻辑
        options.InvalidModelStateResponseFactory = context =>
        {
            var errorMsg = string.Join("；", context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            var result = ApiResponse.Fail(msg:$"请求参数错误：{errorMsg}");
            return new BadRequestObjectResult(result);
        };
    });

4. 如何切换生产环境和开发环境？
    ```
    if (app.Environment.IsDevelopment())
    {
    .....
    .....
    }

    本地调试时，默认就是Development，在项目的launchSettings.json文件中配置
    部署到服务器时，通过环境变量ASPNETCORE_ENVIRONMENT=Production配置，项目会自动识别

</details>