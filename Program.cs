using LY_WebApi.Data;
using LY_WebApi_SwaggerSetting.SwaggerExt;
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
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 添加控制器服务
            builder.Services.AddControllers();

            //添加数据库连接服务
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                //参数1是连接字符串 参数2是mysql的版本号
                options.UseMySql(builder.Configuration.GetConnectionString("MysqlConnection"), new MySqlServerVersion(new Version(8, 0, 36)));
            }
            );

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            //封装Swagger拓展
            builder.Services.AddSwaggerExt();

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
