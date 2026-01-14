using System.ComponentModel.DataAnnotations;
using LY_WebApi.Models;

namespace LY_WebApi.Filter.PropertyValidations
{

    /// <summary>
    /// 验证ShirtId属性
    /// </summary>
    public class Shirts_ShirtIdValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// 验证ShirtId属性
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var shirts = validationContext.ObjectInstance as Shirts;
            if (shirts != null)
            {
                if (shirts.Id <= 0)
                {
                    return new ValidationResult("错误：Id必须大于0");
                }
                return ValidationResult.Success;
            }
            return ValidationResult.Success;
        }
    }
}
