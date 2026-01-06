using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

namespace LY_WebApi_SwaggerSetting.SwaggerExt
{
    /// <summary>
    /// 静态类  作为扩展方法的容器
    /// </summary>
    public static class SwaggerExtension
    {
        /// <summary>
        /// 扩展方法  封装Swagger
        /// </summary>
        /// <param name="Service"></param>
        public static void AddSwaggerExt(this IServiceCollection Service) 
        {


            //swagger设置
            Service.AddSwaggerGen(option =>
            {

                #region 在swagger网页中展示注释  

                //获取程序运行的根目录下的指定 xml文件
                option.IncludeXmlComments($@"{AppDomain.CurrentDomain.BaseDirectory}Ly_WebApi.xml");

                #endregion


                #region 支持Swagger版本控制
                //通过反射 获取ApiVersionInfo类的公共静态字段
                foreach (FieldInfo field in typeof(ApiVersionInfo).GetFields())
                {
                    //临时变量 获取字段的值
                    var Value = field.GetValue(null).ToString();

                    //param: 1:生成的文档名 2:Swagger 文档的元数据配置（标题、版本号、描述等），会显示在 Swagger UI 页面上
                    option.SwaggerDoc(Value, new OpenApiInfo()
                    {
                        Title = $"{field.Name}:LY_WebApi_Test~",
                        Version = Value,
                        Description = $"LYWebApi {field.Name} 版本"
                    });
                }
                #endregion

                #region 支持token传值
                {
                    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                    {
                        Description = "请输入token,格式为 Bearer xxxxxxxx（注意中间必须有空格）",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        BearerFormat = "JWT",
                        Scheme = "Bearer"
                    });

                    //添加安全要求
                    option.AddSecurityRequirement(new OpenApiSecurityRequirement
                   {
                      {
                            new OpenApiSecurityScheme
                            {
                                Reference =new OpenApiReference()
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id ="Bearer"
                                }
                            },
                            new string[]{ }
                        }
                   });

                }
                #endregion

            });


        }

        /// <summary>
        /// 扩展方法 封装使用swagger
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerExt(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                foreach (FieldInfo field in typeof(ApiVersionInfo).GetFields())
                {
                    //临时变量 获取字段的值
                    var Value = field.GetValue(null).ToString();

                    //将当前版本的 Swagger 文档关联到 Swagger UI，用户可通过下拉框切换查看不同版本的接口
                    option.SwaggerEndpoint($"/swagger/{Value}/swagger.json", $"{Value}");
                }
            });

        }


    }
}
