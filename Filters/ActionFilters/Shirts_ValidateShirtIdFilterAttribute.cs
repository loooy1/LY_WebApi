using LY_WebApi.Common;
using LY_WebApi.Data;
using LY_WebApi.Models;
using LY_WebApi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LY_WebApi.Filter.ActionValidations
{
    /// <summary>
    /// 方法过滤器 仅用于修饰控制器方法
    /// </summary>
    public class Shirts_ValidateShirtIdFilterAttribute : ActionFilterAttribute
    {
        private readonly SqlRepository<Shirts> repository;

        /// <summary>
        /// 构造函数 注入[[sql操作仓库服务]]
        /// </summary>
        public Shirts_ValidateShirtIdFilterAttribute(SqlRepository<Shirts> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// 动作执行前验证衬衫id
        /// </summary>
        /// <param name="context"></param>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            base.OnActionExecuting(context);

            var shirtId = context.ActionArguments["id"] as int?;

            //如果进入方法前有传入id参数
            if (shirtId.HasValue)
            {
                //如果衬衫id小于等于0
                if (shirtId.Value <= 0)
                {
                    context.ModelState.AddModelError("id", "衬衫id必须大于等于0");

                    context.Result = new BadRequestObjectResult(ApiResponse.Fail("衬衫id必须大于等于0", 400));

                    return;
                }
                else
                {
                    //从数据库中获取衬衫
                    var shirt = await repository.GetById(shirtId.Value);

                    context.HttpContext.Items["shirt"] = shirt;

                    await next();
                }
            }
        }
    }
}