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
    .NET9.0  跨平台 开源 以前叫.NET Core，2025年改名为.NET，和之前的.NET Framework区分开了

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

8. 怎么实时获得appsettings.json的配置值？
    ```
    1. 注册配置类并绑定appsettings.json的配置节（1.找到 appsettings.json 中 ApiConfig 这个节点 2.把节点下的 属性 自动赋值给 ApiConfig 类的对应属性 3.自动注册 IOptions<ApiConfig>/IOptionsSnapshot<ApiConfig>/IOptionsMonitor<ApiConfig> 这三个服务）
    services.Configure<ApiConfig>(configuration.GetSection("ApiConfig"));

    2.注入IOptionsMonitor接口 获取当前配置值
    private readonly IOptionsMonitor<BackgroundTaskConfig> _config;
    public AppsettingConfigMonitorHandler(IOptionsMonitor<BackgroundTaskConfig> config)
    {
        _config = config;
    }
    _config.CurrentValue // 获取当前配置值

    ps：
    IOptions<T>：一次性读死，程序启动后配置改了也不生效；
    IOptionsSnapshot<T>：每次拿最新快照，改配置后 “下次用就生效”；
    IOptionsMonitor<T>：盯着配置实时更，改配置后 “立刻生效” 还能主动通知。


9. 工厂模式和键控模式
   ```
   工厂模式：通过工厂类根据条件创建不同的实例，工厂类负责实例化对象，客户端通过工厂获取实例。
   键控模式：直接在依赖注入容器中注册多个实现类，并通过键值区分，在需要使用时通过键值获取对应的实例。

   根据负责条件创建实例的话，工厂模式更合适；如果需要在多个地方使用不同的实现类/只根据名字获取实例，键控模式更方便.

10. typeof的用法
    lytodo

11. 发布
    1. 在VS中右键项目 → 发布 → 选择发布目标（文件夹/云服务等） → 配置发布设置 → 发布
    2. 发布配置
        - 发布模式：Release（优化性能，去除调试信息）
        - 目标框架：net9.0
        - 生成文件夹：指定发布后的输出路径
        - 独立部署：选择是否包含.NET运行时（独立部署包含运行时，适用于目标环境没有安装.NET的情况；框架依赖部署需要目标环境预装.NET）
        - 可移植性：选择是否生成linux 苹果等版本
        - 发布单个文件：将所有内容打包成一个可执行文件，方便部署和分发
        - ReadyToRun：预编译应用程序，减少首次启动时间 但是会增加发布包体积
        - 裁剪未使用的代码：移除未使用的代码和资源，减小发布包体积。 不建议勾选，会影响反射等功能，导致运行时错误
        
 12. 泛型和动态创建实例
     var obj = Activator.CreateInstance(repositoryType, _dbContext) ?? throw new Exception($"不支持实例化{repositoryType}");

 13. channel
     channel是一个线程安全的队列，提供了生产者-消费者模式的实现，可以在不同线程之间安全地传递数据。

</details>

<details>

<summary>## 杨中科学习</summary>      

1. 尽量使用异步方法
    ```
   用async修饰的方法
       返回Task(无返回值)或Task<T>(有返回值)
       使用时await调用，等待异步操作完成
       调用异步方法的方法也要标记为async，并返回Task或Task<T>

   async本质
       async的方法会被编译器转换成状态机，记录执行的状态和上下文。当前线程遇到await之后给操作系统api发完指令之后就返回线程池，等操作系统完成任务后再回来通知.net，然后从线程池调用新线程通过状态机记录的状态继续执行后续逻辑。
       像读写文件/http请求/数据库操作等是操作系统的工作，.net调用操作系统的API发出指令后就可以继续执行其他代码了，等操作系统完成任务后再回来通知.net继续执行后续逻辑。
       同步的话，.net线程调用操作系统API发出指令后就一直等着，然后操作系统线程给cpu发完指令之后就不管了。但是.NET线程会一直等待。
       异步的话，.net线程调用操作系统API发出指令后就可以继续执行其他代码了，等操作系统完成任务后再回来通知.net继续执行后续逻辑，增强了.net线程的使用效率

   异步代码并不会自动创建新线程，没有 Task.Run，没有新线程，异步方法里的代码就是调用线程一路往下跑，直到碰到真正需要调用操作系统异步IO，线程才交还线程池。
   例如 文件读写：File.ReadAllTextAsync
        网络请求：HttpClient.GetAsync
        数据库：xxxAsync
        缓存：Redis.GetAsync
        消息队列：RabbitMQ.ConsumeAsync
        流操作：Stream.ReadAsync
  
   async方法的缺点： 
       异步方法会生成一个状态机的类，效率低；可能会占用更多的线程内存。
       如果直接是 异步操作的转发，或者没有后续逻辑需要执行，可以直接返回Task或Task<T>，不使用async/await，避免生成状态机类，提高性能。 避免异步包装
       例如 public Task<string> GetDataAsync() => File.ReadAllTextAsync("data.txt");
       而不是 public async Task<string> GetDataAsync() => await File.ReadAllTextAsync("data.txt");

   暂停线程
       用Task.Delay而不是Thread.Sleep，Task.Delay不会阻塞线程，而Thread.Sleep会阻塞线程，导致性能问题
       task.Delay(1000); 会通知操作系统在1000毫秒后通知.NET继续执行后续逻辑，而Thread.Sleep(1000);会直接阻塞当前线程1000毫秒，期间无法处理其他请求，性能较差。

   取消异步任务 
       用CancellationToken，提供取消功能，避免不必要的资源消耗
       例如
                CancellationTokenSource cts = new CancellationTokenSource();
                cts.CancelAfter(5000); // 设置5秒后取消
                await ReadFileAsync("http://www.baidus.com",100, cts.Token);

   Task类的常用方法：
        WhenAll：等待多个任务全部完成，返回一个新的Task，当所有任务完成时完成。
        WhenAny：等待多个任务中任意一个完成，返回一个新的Task，当任意一个任务完成时完成。
        FromResult：创建一个已完成的Task，直接返回结果，不执行异步操作。

   yield
       yield return：用于生成器方法，允许方法一次返回一个值。然后调用处校验 找到合适的值之后就结束yield return的迭代。节省内存
       yield break：结束生成器方法的迭代，停止返回值。 方法剩余的代码不再执行。
       yield方法返回值必须是IEnumerable类型的；调用方使用 foreach或LINQ等方式迭代获取值。
       成立一个数据然后返回 再处理再返回 直到满足条件结束


   Note:
       1. 接口中定义异步方法时，直接返回Task或Task<T>，不能加async
    ```
2. LINQ
    ```
    委托
        委托是方法的类型，指向方法。和赋值变量一样。
        委托可以指向一个方法，也可以指向多个方法（多播委托）。调用委托时，会依次调用所有指向的方法。
        委托可以指向静态方法/实例方法/匿名方法。
        自定义委托：public delegate int MyDelegate(string input); 一个返回值一个参数的委托
        但是一般不自定义委托了，直接用系统内置的Func和Action就行了
        Action<T>：定义一个无返回值的委托，接受一个或多个参数，执行一些操作但不返回结果。
        Func<T, TResult>：定义一个返回值的委托，接受一个或多个参数，返回一个结果。

    匿名方法和lambda表达式
        匿名方法：没有名称的方法，可以直接赋值给委托变量，使用delegate关键字定义。不能独立存在，必须赋值给委托变量。
        Lambda表达式：匿名方法的简化语法，使用=>符号定义，左边是参数列表，右边是方法体。也不能独立存在，必须赋值给委托变量。
        例如：
            Action<string> print = delegate(string message) { Console.WriteLine(message); };
            Action<string> printLambda = message => Console.WriteLine(message);
   
    自定义linq中的where方法
         public static IEnumerable<int> MyWhere(IEnumerable<int> ints, Func<int, bool> func)
        {
            List<int> ints1 = new List<int>();
            foreach (var item in ints)
            {
                if (func(item))
                {

                    ints1.Add(item);
                }
            }
            return ints1;
        
        }
        =====================================================================================
           public static IEnumerable<int> MyWhere2(IEnumerable<int> ints, Func<int, bool> func)
        {
            foreach (var item in ints)
            {
                if (func(item))
                {

                    yield return item;
                }
            }
        }

    linq常用方法
        Where：过滤集合中的元素，返回满足条件的元素集合。
        Join：连接两个集合，根据指定的键将它们关联起来，返回一个新的集合，其中每个元素是通过将两个集合中的元素进行匹配得到的。
        Distinct：去除集合中的重复元素，返回一个新的集合，其中只包含唯一的元素。
        Any：确定集合中是否存在满足条件的元素，返回一个布尔值。
        All：确定集合中的所有元素是否都满足条件，返回一个布尔值。
        Count：计算集合中满足条件的元素数量，返回一个整数值。

        //获取一条数据
        Single/SingleOrDefault：从集合中返回满足条件的唯一元素，如果没有满足条件的元素或有多个满足条件的元素，则抛出异常或返回默认值。
        First/FirstOrDefault：从集合中返回满足条件的第一个元素，如果没有满足条件的元素，则抛出异常或返回默认值。

        //排序得到一个新集合
        OrderBy/OrderByDescending：对集合中的元素进行排序，返回一个新的集合，其中元素按照指定的键进行升序或降序排列。
           var b = list.OrderBy(i => i.Age).ThenBy(i => i.Salary); 先根据年龄排序再根据工资排序，年龄相同的情况下工资高的排前面

        //操作结果集
        skip/take：跳过集合中的前n个元素或返回集合中的前n个元素，返回一个新的集合。

        //分组得到一个新集合
        GroupBy：将集合中的元素分组，返回一个新的集合，其中每个元素是一个分组对象，包含一个键和一个元素集合。
            var result = list.GroupBy(e => e.Age);

            foreach (var group in result)
            {
                Console.WriteLine($"Age: {group.Key}");
                foreach (var employee in group)
                {
                    Console.WriteLine(employee);
                }
            }

        //投影
        Select /SelectMany：将集合中的元素投影到一个新的形式，返回一个新的集合.和原来的类型不同了
        var result = list.GroupBy(e => e.Age).Select(g =>   new { Age = g.Key, MaxS = g.Max(e => e.Salary) });

        //匿名类型
        var aa = new { Name = "Alice", Age = 30 }; //创建一个匿名类型的对象
        var result = list.Select(i => new { Name = i.Name, Age = i.Age }); //投影到一个匿名类型的集合

        //转换方法
        ToList：将集合转换为List<T>类型。
        ToArray：将集合转换为数组。

        //聚合方法
        Sum：计算集合中满足条件的元素的总和，返回一个数值。
        Min：计算集合中满足条件的元素的最小值，返回一个数值。
        Max：计算集合中满足条件的元素的最大值，返回一个数值。
        Average：计算集合中满足条件的元素的平均值，返回一个数值。
        Count：计算集合中满足条件的元素数量，返回一个整数值。

        例子1：
        从员工列表中：筛选出 ID>2 的员工 → 按年龄分组 → 按年龄升序排序 → 取前 3 个年龄组 → 统计每组的年龄、人数、平均工资 → 打印输出。

        // 创建员工列表
            List<Employee> list = new List<Employee>();

            // 添加员工数据
            list.Add(new Employee { Id = 1, Name = "jerry", Age = 28, Gender = true, Salary = 5000 });
            list.Add(new Employee { Id = 2, Name = "jim", Age = 33, Gender = true, Salary = 3000 });
            list.Add(new Employee { Id = 3, Name = "lily", Age = 35, Gender = false, Salary = 9000 });
            list.Add(new Employee { Id = 4, Name = "lucy", Age = 16, Gender = false, Salary = 2000 });
            list.Add(new Employee { Id = 5, Name = "kimi", Age = 25, Gender = true, Salary = 1000 });
            list.Add(new Employee { Id = 6, Name = "nancy", Age = 35, Gender = false, Salary = 8000 });
            list.Add(new Employee { Id = 7, Name = "zack", Age = 35, Gender = true, Salary = 12000 });
            list.Add(new Employee { Id = 8, Name = "jack", Age = 33, Gender = true, Salary = 8000 });


            list.Where(e => e.Id > 2).GroupBy(e => e.Age).OrderBy(e => e.Key).Take(3).Select(e => new { Age=e.Key,People=e.Count(),Avg=e.Average(e=>e.Salary)}).ToList().ForEach(e=> Console.WriteLine($"Age:{e.Age},People:{e.People},Avg:{e.Avg}"));

        例子2：
        统计这个字符串中每个字母出现的频率(忽略大小写)，然后按照从高到低的顺序输出出现频率高于2次的字母和其出现的频率
        
        string content = "Hello World! Abc 123, Test@Sys.";

        content.Where(e=>char.IsLetter(e)).Select(e=>char.ToLower(e)).GroupBy(e=>e).Select(e=>new {Char=e.Key,Count=e.Count() }).OrderByDescending(e => e.Count).Where(e => e.Count > 2).ToList().ForEach(e => Console.WriteLine($"Char: {e.Char}, Count: {e.Count}"));

3. 依赖注入
    ```
    控制反转=>一种设计原则，指将对象的创建和依赖关系的管理交给外部容器，而不是由对象自己负责。
    依赖注入=>一种实现方式
    
    生命周期 
    singleton：整个应用程序生命周期内只创建一个实例，所有请求都共享这个实例。适用于无状态服务或需要共享状态的服务。
    transient：每次请求都创建一个新的实例，不共享实例。适用于轻量级、无状态的服务。
    scoped：每个请求创建一个实例，同一请求内共享实例，不同请求之间不共享。适用于需要在请求范围内维护状态的服务。

    如果一个类实现了IDisposable接口，在using块中创建该类的实例，using块结束时会自动调用Dispose方法，释放资源。




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


        
        

    lytodo:
        2026/3/3
        接口 抽象类 实现类

        2026/3/4
        仓储类加工作单元
        
</details>
