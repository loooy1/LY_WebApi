
using System.ComponentModel.DataAnnotations;
using LY_WebApi.Models;


// 自定义校验特性类：命名建议简化为 ShirtsGenderSizeValidationAttribute（更语义化）
public class Shirts_GenderSizeValidationAttribute : ValidationAttribute
{
    // 核心重写方法：框架会调用这个方法执行校验逻辑
    // 参数1：value → 特性贴在哪个属性上，这个值就是该属性的当前值（⚠️ 注意这个参数的坑）
    // 参数2：validationContext → 校验上下文，包含整个模型实例、属性信息等核心数据
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // 步骤1：从校验上下文获取整个 Shirts 对象实例
        // validationContext.ObjectInstance → 绑定的整个模型（这里是 Shirts 对象）
        // 强转为 Shirts 类型，方便访问 Gender 和 Size 属性
        var shirts = validationContext.ObjectInstance as Shirts;

        // 步骤2：前置判空，避免空引用异常
        // 条件：Shirts 对象不为空 + Gender 不为空/空白（非必填字段，无值则无需校验）
        if (shirts != null && !string.IsNullOrEmpty(shirts.Gender))
        {
            // 步骤3：男士衬衫校验逻辑
            // Equals(..., StringComparison.OrdinalIgnoreCase) → 忽略大小写（支持"男"/"MAN"/"male"等）
            if (shirts.Gender.Equals("男", StringComparison.OrdinalIgnoreCase) && shirts.Size < 8)
            {
                // 校验失败：返回包含错误信息的 ValidationResult
                return new ValidationResult("错误：男士衬衫尺寸必须大于等于8");
            }
            // 步骤4：女士衬衫校验逻辑
            else if (shirts.Gender.Equals("女", StringComparison.OrdinalIgnoreCase) && shirts.Size < 6)
            {
                return new ValidationResult("错误：女士衬衫尺寸必须大于等于6");
            }
            else
            {
                return new ValidationResult("错误：性别 必须是‘男’或者‘女’");
            }
        }

        // 步骤5：校验通过 → 返回 ValidationResult.Success（框架识别为无错误）
        return ValidationResult.Success;
    }
}