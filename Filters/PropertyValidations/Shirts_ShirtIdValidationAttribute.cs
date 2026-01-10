using System.ComponentModel.DataAnnotations;
using LY_WebApi.Models;

namespace LY_WebApi.Filter.PropertyValidations
{

    /// <summary>
    /// 验证ShirtId属性
    /// </summary>
    public class Shirts_ShirtIdValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var shirts = validationContext.ObjectInstance as Shirts;
            if (shirts != null)
            {
                if (shirts.ShirtsId <= 0)
                {
                    return new ValidationResult("错误：ShirtsId必须大于0");
                }
                return ValidationResult.Success;
            }
            return ValidationResult.Success;
        }
    }
}
