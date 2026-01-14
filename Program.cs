using System.Reflection;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using LY_WebApi.Common.SwaggerExtension;
using LY_WebApi.Data;
using LY_WebApi.Models.Repository;
using Microsoft.EntityFrameworkCore;

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

            // 添加控制器服务
            builder.Services.AddControllers();

            //注册数据库连接服务
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                //参数1是连接字符串 参数2是mysql的版本号
                options.UseMySql(builder.Configuration.GetConnectionString("MysqlConnection"), new MySqlServerVersion(new Version(8, 0, 36)));
            }
            );

            // 【说明：当前是Controller开发，此行可删，不影响运行；为兼容MinimalAPI+官方规范，保留】
            builder.Services.AddEndpointsApiExplorer();

            //注册sql操作仓库服务
            builder.Services.AddScoped(typeof(SqlRepository<>));

            //注册版本控制服务
            builder.Services.AddSwaggerExt();

            //服务注册结束 启用服务
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                //启用swagger服务
                app.UseSwaggerExt();
            }



            app.UseAuthorization();

            app.MapControllers(); //将控制器映射到路由

            app.Run();
        }
    }
}
