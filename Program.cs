using LY_WebApi.options;
using LY_WebApi_SwaggerSetting.SwaggerExt;

namespace Ly_WebApi
{
    //IOC容器：用于创建实例。先注入容器，再用容器获得实例
    //如果实例A依赖于实例B  IOC会自动处理依赖关系，前提是A B都注入到容器中
    //构造函数注入  方法注入(需要标注[FromServices])
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            //封装Swagger拓展
            builder.Services.AddSwaggerExt();

            #region IOC容器
            {
                //builder.Services.AddTransient();
            }
            //生命周期
            // 瞬时生命周期-每一次创建的实例都是一个全新实例 AddTransient  TryAddTransient
            // 单例生命周期-每一次创建的实例都是同一个实例   AddSingleton  TryAddSingleton
            // 作用域生命周期-不同的servicesProvider获取的实例不同 AddScoped TryAddScoped

            //单抽象多实现
            //集合获取/别名注册获取
            #endregion

            #region 日志系统

            //Log4net配置 
            //nuget:    log4net Microsoft.Extensions.Logging.Log4Net.AspNetCore
            {
                builder.Logging.AddLog4Net("LogSetting/log4net.Config");
            }

            //Nlog配置 

            #endregion

            #region options手动注册读取配置文件 TODO:利用反射去做批量注册
            {
                // 注册基础配置
                builder.Services.AddOptions<BaseSettingOptions>()
                    .Bind(builder.Configuration.GetSection(BaseSettingOptions.SectionName))
                    .ValidateDataAnnotations()
                    .ValidateOnStart();

                // 注册教学信息配置
                builder.Services.AddOptions<TeachInfoOptions>()
                    .Bind(builder.Configuration.GetSection(TeachInfoOptions.SectionName))
                    .ValidateDataAnnotations()
                    .ValidateOnStart();

                // 注册数据库连接配置（核心）
                builder.Services.AddOptions<DbConnectionOptions>()
                    .Bind(builder.Configuration.GetSection(DbConnectionOptions.SectionName))
                    .ValidateDataAnnotations()
                    // 自定义验证：读库列表至少有1个配置
                    .Validate(config => config.ReadConnectionList.Count >= 1, "读库连接列表不能为空")
                    .ValidateOnStart();

                // 注册日志配置（可选）
                builder.Services.AddOptions<LoggingOptions>()
                    .Bind(builder.Configuration.GetSection(LoggingOptions.SectionName));
            }
            #endregion

            var app = builder.Build();



            //使用Swagger拓展
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerExt();
            }


            //app.MapGet("/GetShirts", () => {

            //    return "获取所有衬衫";
            //});



            app.UseAuthorization();

            app.MapControllers(); //将控制器

            app.Run();
        }
    }
}
