using System.Reflection;
using System.Threading.Channels;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using LY_WebApi.Common;
using LY_WebApi.Common.AppsettingConfig;
using LY_WebApi.Common.MediatR;
using LY_WebApi.Common.SerilogExt;
using LY_WebApi.Common.SwaggerExtension;
using LY_WebApi.Data;
using LY_WebApi.MiddleWare;
using LY_WebApi.Repository;
using LY_WebApi.Services;
using LY_WebApi.Services.Background;
using LY_WebApi.Services.ExternalService;
using LY_WebApi.Services.ExternalService.ExternalServiceBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Serilog;

namespace Ly_WebApi
{

    /// <summary>
    /// IOC容器：用于创建实例。先注入容器，再用容器获得实例
    /// 如果实例A依赖于实例B  IOC会自动处理依赖关系，前提是A B都注入到容器中
    /// 构造函数注入  方法注入(需要标注[FromServices])
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 主程序入口
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region  框架服务注册
            // 添加控制器服务
            builder.Services.AddControllers();

            //配置Serilog服务
            builder.ConfigureSerilogExt(); 

            // 注册appsetting配置
            builder.Services.AddAllConfigs(builder.Configuration);

            // 注册 MediatR
            builder.Services.AddMediatR(); 

            //注册数据库连接服务
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                //参数1是连接字符串 参数2是mysql的版本号
                options.UseMySql(builder.Configuration.GetConnectionString("MysqlConnection"), new MySqlServerVersion(new Version(8, 0, 36)));
            }
            );

            // 【说明：当前是Controller开发，此行可删，不影响运行；为兼容MinimalAPI+官方规范，保留】
            builder.Services.AddEndpointsApiExplorer();

            //注册版本控制服务
            builder.Services.AddSwaggerExt();

            // 注册AutoMapper服务
            builder.Services.AddAutoMapper(cfg => {
                // 这里可以手动添加映射，也可以什么都不写，自动扫描 Profile
            }, typeof(AutoMapperProfile).Assembly);

            // 服务层依赖HttpClient
            builder.Services.AddHttpClient(); 
            #endregion


            #region 自定义服务注册
            //注册sql操作仓库服务
            builder.Services.AddScoped(typeof(SqlRepository<>));

            //注册聚合业务服务层
            builder.Services.AddScoped<ExampleLocalService>();

            //注册内部业务服务层
            builder.Services.AddScoped(typeof(LocalService<>));

            //注册外部业务服务层
            builder.Services.AddScoped<TestExternalService>();


            // 服务层协议注入
            // 键："Http" → 对应 Http协议实现类
            builder.Services.AddKeyedScoped<IBaseService, HttpService<object>>("Http");
            //可增加其他协议实现类

            // 注册后台定时任务服务
            builder.Services.AddHostedService<TimedBackgroundTask>();

            // 注册配置文件监控服务
            builder.Services.AddHostedService<TaskConfigMonitor>();

            builder.Services.AddSingleton(Channel.CreateUnbounded<TaskControlCommand>());
            #endregion




            //服务注册结束 启用服务和配置中间件
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                //启用swagger服务
                app.UseSwaggerExt();
            }

            //启用 ASP.NET Core 的授权中间件，用于检查用户身份和权限
            app.UseAuthorization();

            //启用控制器处理请求
            app.MapControllers();

            #region 自定义中间件配置
            app.UseGetIPMiddlewareExt(); //获取客户端IP中间件
            #endregion

            //运行应用程序
            app.Run();
        }
    }
}
