# 此项目为学习Web_API而创建的示例项目。

<details>
<summary>## 概述</summary>

app.UseAuthorization(); 所有带use的都是中间件

MVC也是一个中间件。
客户端发送 POST/GET 请求 → 中间件管道（跨域、认证等）；
路由匹配到 ShirtsController 的 AddShirts Action（控制器）；
过滤器执行校验（控制器方法过滤器，实体类属性验证过滤器，异常过滤器）；
控制器调用 Model（Shirts 类、业务逻辑）处理数据；
控制器返回 IActionResult 响应（无 View 渲染）；
响应经过滤器、中间件返回客户端。

</details>

<details>
<summary>## 引用结构</summary>

包(运行在框架上)  
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
    ```

框架
    底层框架
    .NET9.0

    web上层框架
    ASP.NET.Core

项目引用
    <ItemGroup>
    <ProjectReference Include="类库项目\LY_WebApi_SwaggerSetting\LY_WebApi_SwaggerSetting.csproj" />
    <ProjectReference Include="类库项目\LY_WebAPI_Test\LY_WebAPI_Test.csproj" />
    </ItemGroup>

分析器/编译器/VS
    框架或者包自带
    用于检查代码质量问题，报 warning 
    编译器.NET SDK自带，检查代码错误问题，报 error
    VS 集成开发工作台

</details>

<details>
<summary>## EFcore操作数据库</summary>

需要nuget相关的包
1. appsettings.json  → 存放MySQL连接字符串（账号、密码、库名）
2. Program.cs        → 读取连接字符串 + 注册AppDbContext + 配置MySQL驱动
3. AppDbContext      → 接收配置 + 映射Shirts表 + 配置种子数据
4. 迁移命令          → 执行AppDbContext的配置，在MySQL创建表+插入种子数据
5. 控制器Controller  → 注入AppDbContext，调用它的方法操作数据库
6. MySQL数据库       → 接收SQL指令，返回数据结果

</details>

<details>
<summary>## FAQ</summary>

### 1. 程序包管理器控制台和PowerShell控制台有什么区别？
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

</details>