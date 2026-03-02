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
<summary>## 过滤器</summary>

1. 什么是过滤器？
    ```
    过滤器(Filter)是ASP.NET Core MVC中的组件，用于在请求处理的不同阶段执行特定的逻辑。
    它们可以在控制器方法执行之前或之后运行，以实现跨切面关注点（如日志记录、授权、异常处理等）的处理。
    过滤器可以应用于控制器类或具体的控制器方法。

2. 过滤器种类及作用

    授权过滤器(Authorization Filters)
    ```
    在执行控制器方法之前运行，用于验证用户是否有权限访问特定资源。
    ```

    资源过滤器(Resource Filters)
    ```
    在授权过滤器之后运行，可以用于缓存响应或修改请求。
    示例：
    public class CustomAsyncResourceFilter : IAsyncResourceFilter
    {
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            // 在控制器方法执行之前运行的异步逻辑

            var executedContext = await next();

            // 在控制器方法执行之后运行的异步逻辑
        }
    }
    ```

    操作过滤器(Action Filters)
    ```
    在控制器方法执行之前和之后运行，可以用于日志记录、性能监控等。
    示例：
    public class CustomAsyncActionFilter : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 在控制器方法执行之前运行的异步逻辑

            var resultContext = await next();

            // 在控制器方法执行之后运行的异步逻辑
        }
    }
    ```

    异常过滤器(Exception Filters)
    ```
    在控制器方法抛出异常时运行，用于处理异常并返回自定义错误响应。
    示例：
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        public void OnException(ExceptionContext context)
        {
            // 处理异常逻辑
            var response = new
            {
                Message = "An error occurred while processing your request.",
                Details = context.Exception.Message
            };
            context.Result = new JsonResult(response)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            context.ExceptionHandled = true; // 标记异常已处理
        }
    }
    ```

    结果过滤器(Result Filters)
    ```
    在操作结果生成之后运行，可以用于修改响应数据或添加额外的响应头。
    示例：
    public class CustomAsyncResultFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            // 在操作结果生成之前运行的异步逻辑
            var executedContext = await next();
            // 在操作结果生成之后运行的异步逻辑
        }
    }
    ```

</details>

<details>
<summary>## FluentValidation校验器</summary>
todoly:校验器的实现

</details>

<details>
<summary>## MediatR</summary>

1. 什么是MediatR？
    ```
    MediatR是一个开源的.NET库，实现了中介者模式(Mediator Pattern)，用于简化应用程序中的对象间通信。
    它通过将请求和处理程序解耦，使得代码更加模块化、可维护和可测试。
    MediatR允许你定义请求（命令或查询）和相应的处理程序，而不需要直接引用处理程序，从而减少了类之间的依赖关系。
    ```
2. MediatR的使用
    ```
    1. 注册MediatR服务
        自动注册是默认 Transient 瞬态生命周期，想单例的话可以手动注册
        builder.Services.AddCustomMediatR();

    2. 定义 请求 类，请求类有两种类型：指令(Command)和事件(Event)
        /// 指令定义 可定义多个，处理器类可以选择性实现对应的指令类接口
        public class TaskControlCommand : IRequest<Unit>
        {
            public bool Enable { get; set; }
        }

        /// 事件定义（Event/Notification）：用于「发布-订阅」，一对多（一个事件可被多个处理器订阅）
        public class TaskControlEvent : INotification
        {
            public bool Enable { get; set; }
        }

    3. 定义 处理器 类，可以实现对应指令类的接口，就会只处理对应的请求
        可以注入其他服务/类来处理请求逻辑

        (普通类，继承两个接口并实现对应的Handle方法)
        public class Test : INotificationHandler<TaskControlEvent>, IRequestHandler<TaskControlCommand, Unit>
       
    4.绑定请求和处理程序
        MediatR会自动扫描程序集，绑定请求和处理程序，无需手动注册（默认瞬态生命周期，可以手动注册为单例）

    5.发送请求
    // 注入IMediator
        private readonly IMediator _mediator;
        public ShirtsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // 发送指令请求
        await _mediator.Send(new TaskControlCommand { Enable = true });
        // 发布事件请求
        await _mediator.Publish(new TaskControlEvent { Enable = true });

    6. 处理请求
        (在处理器类中实现对应的Handle方法，注入服务层逻辑，处理请求逻辑)

    7. 运行应用程序
        (MediatR会根据发送的请求，自动调用对应的处理程序，完成请求处理逻辑)
    
    8. MR的管道行为
        类似于中间件，可以在请求处理前后执行额外逻辑（如日志记录、性能监控等），通过实现IPipelineBehavior<TRequest, TResponse>接口来定义管道行为。

        //命令管道行为
        public class AppsettingConfigMonitorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>where TRequest : IRequest<TResponse>
        
        //广播管道行为
        public class AppsettingConfigMonitorNotificationPublisher : INotificationPublisher

        //注册
        // 注册命令管道行为
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AppsettingConfigMonitorBehavior<,>));

        // 注册广播管道行为
        cfg.NotificationPublisherType = typeof(AppsettingConfigMonitorNotificationPublisher);

        通常情况下，管道行为所有请求都会经过，可以在管道行为中根据请求类型或其他条件来决定是否执行特定逻辑，或者直接放行请求到下一个处理程序。
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

    分支中间件（根据条件选择路径）
    ```
    // 简单分支中间件
    app.Map("/branch", branchApp =>
    {
        branchApp.Run(async context =>
        {
            await context.Response.WriteAsync("This is the branch middleware.\r\n");
        });
    });
    
    // 根据请求查询参数决定是否进入分支中间件
    app.MapWhen(context => context.Request.Query.ContainsKey("admin"),appBuilder => 
    
    {
    appBuilder.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("Admin middleware: ");
        await next();
    });

    appBuilder.Run(async context =>
    {
        await context.Response.WriteAsync("Hello Admin!");
    });
    }

    );

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
4. 属性通知(性能优化)
    ```
    有属性通知：SaveChangesAsync() 直接用 “实时标记的变更”，不扫就更新；
    无属性通知：SaveChangesAsync() 先 “扫一遍找变更”，再更新；

            public async Task Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "修改的实体数据不能为空");
            }
            //_db.Set<T>().Update(entity);
            await _db.SaveChangesAsync();
        }

        //_db.Set<T>().Update(entity);是强制更新所有字段
        如果有属性通知的话 await _db.SaveChangesAsync();就可以更新字段到数据库内，性能更高

        属性通知示例：
         public string? Color
        {
            get => _color;
            set
            {
                // 仅值变化时触发通知（避免EF Core无效标记）
                if (_color != value)
                {
                    _color = value;
                    // 触发PropertyChanged事件，EF Core会实时捕获
                    OnPropertyChanged();
                }
            }
        }

        OnPropertyChanged();要符合EF标准
    ```


</details>

<details>
<summary>## 架构_分层</summary>

```
控制器层(controller) ---->应用层(application)  ----> 业务层(services)  ----> 仓储层(Repository) 
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

5. try-catch的重要性？
    ```
    try-catch用于捕获代码执行过程中可能发生的异常，防止程序崩溃。

6. using语句的作用？
    ```
    using语句用于引入命名空间；
    using语句可以创建一个代码块，在代码块结束时自动调用IDisposable接口的Dispose方法，释放资源。
    using (var scope = _scopeFactory.CreateScope()) // 创建作用域
    {
            // 使用 scope
    var service = scope.ServiceProvider.GetRequiredService<SomeService>();
    }       // 自动调用 scope.Dispose()，释放资源

7. 键控注册服务
    ```
    //注册
    builder.Services.AddTransient<IService, ServiceA>("ServiceA");
    builder.Services.AddTransient<IService, ServiceB>("ServiceB");
    
    //依赖注入
    public AppsettingConfigMonitorHandler([FromKeyedServices("ServiceA")] IService taskController)
    {
        _taskController = taskController;
    }
    ```




</details>


<details>
<summary>## 开发日志</summary>

    1. before 2026-01-19
        - 创建项目
        - 配置swagger
        - 配置EFcore连接MySQL
        - 创建Shirts实体类和AppDbContext
        - 生成迁移文件并更新数据库
        - 创建ShirtsController实现CRUD接口
        - 优化响应格式
        - 添加请求参数验证过滤器
        - 添加全局异常过滤器
        - 分层架构重构（控制器-业务-仓储-数据访问）
        - 优化swagger配置，添加版本控制
        - 自定义中间件日志记录请求响应信息
        - 优化异常过滤器，记录异常日志
        - 添加种子数据初始化数据库
        - 优化EFcore操作，添加异步方法支持
        - 学习理解中间件和过滤器概念
        - 优化代码结构，添加注释说明
        - 测试接口功能，修复bug
        - 优化日志记录格式
    2. 2026-01-20
        - serilog按需写入文件夹，文件夹为固定命名，可添加(LY_WebApi\Common\SerilogExt)
        - 读取appsetting配置(LY_WebApi\Common\Config\ConfigExtensions.cs)
    3. 2026-01-25
        - 根据配置去开启不同的后台任务或者服务（只用MediatR）
        - MediatR多实例问题解决
    4. 2026-01-26
        - 优化MediatR请求处理，添加命令和事件示例
        - 理解MediatR逻辑,这个最好用于流程控制 依赖注入之后用handler去控制服务层，而不是用handler去做服务层 
        - github提交代码不增加小绿点,需要邮箱一致


        
        

    TODO:
        
</details>
