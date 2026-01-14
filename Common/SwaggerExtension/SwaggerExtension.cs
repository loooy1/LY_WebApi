using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

namespace LY_WebApi.Common.SwaggerExtension
{
    /// <summary>
    /// 静态类  作为扩展方法的容器
    /// </summary>
    public static class SwaggerExtension
    {
        /// <summary>
        /// 扩展方法  封装Swagger
        /// </summary>
        /// <param name="Services"></param>
        public static void AddSwaggerExt(this IServiceCollection Services) 
        {
            //接口版本控制的 核心配置
            Services.AddApiVersioning(options =>
            {
                // 配置项1：未指定版本时，使用默认版本
                options.AssumeDefaultVersionWhenUnspecified = true;
                // 配置项2：默认版本号为v1.0
                options.DefaultApiVersion = new ApiVersion(1, 0);
                // 配置项3：在http响应头中返回当前接口的版本信息
                options.ReportApiVersions = true;
                // 配置项4：从URL路径中读取版本号（如 /api/v1/Shirt）
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            // 链式调用：注册Swagger适配的版本探索器
            .AddApiExplorer(options =>
            {
                // 配置项A：版本分组格式为 "v1"、"v2"（固定写法，规范）
                options.GroupNameFormat = "'v'VVV";
                // 配置项B：替换URL中的版本占位符（v{version:apiVersion} → v1）
                options.SubstituteApiVersionInUrl = true;
            });

            //向ASP.NET Core 的服务容器中注册「API 端点元数据探索器」服务 采集接口的所有信息
            Services.AddEndpointsApiExplorer();

            //Swagger 的核心注册方法
            Services.AddSwaggerGen(options =>
            {
                //获取 当前程序集生成xml文档
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

                //拼接路径
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                //把xml文档(「控制器 + 接口 + 模型类的所有注释」)加载到swagger页面
                options.IncludeXmlComments(xmlPath, true);

                //获取项目中所有的版本信息
                var provider = Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                //遍历项目中所有的 API 版本
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    //为 Swagger「新增一份独立的接口文档」
                    options.SwaggerDoc(description.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "LY_WebApi 测试接口",
                        Version = description.ApiVersion.ToString(),

                        //[ApiVersion("1.0", Deprecated = true)] 是否废弃当前版本
                        Description = description.IsDeprecated ? "⚠️ 该版本已废弃，请升级！" : "✅ 该版本正常可用"
                    });
                }
            });


        }

        /// <summary>
        /// 扩展方法 封装使用swagger
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerExt(this WebApplication app)
        {
            //注册 Swagger 的文档生成 中间件,只生成原始的 JSON 数据文档
            app.UseSwagger();

            //渲染成友好的、可视化的、可交互的网页界面
            app.UseSwaggerUI(options =>
            {
                //获取项目中所有的版本信息
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

                //遍历版本信息 生成对应的文档和下拉框
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    //文档名称和下拉框选项名称
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"LYwebapi {description.GroupName}");
                }

                //设置不展开实体类
                options.DefaultModelsExpandDepth(-1);
            });
        }
    }
}
