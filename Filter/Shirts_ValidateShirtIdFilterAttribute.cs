using LY_WebApi.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LY_WebApi.Filter
{
    public class Shirts_ValidateShirtIdFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
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

                    //用于标准化API错误响应
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };

                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                //如果衬衫内存仓库不存在对应id的衬衫
                else if (!Shirt_Repository.ShirtExists(shirtId.Value))
                {
                    context.ModelState.AddModelError("id", $"不存在id为{shirtId.Value}的衬衫");
                    context.Result = new NotFoundObjectResult(context.ModelState);
                }

            }
        }
    }
}
