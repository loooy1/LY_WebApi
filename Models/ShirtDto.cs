using System.ComponentModel.DataAnnotations;
using LY_WebApi.Filter.ActionValidations;
using LY_WebApi.Filter.PropertyValidations;

namespace LY_WebApi.Models
{
    /// <summary>
    /// 衬衫数据传输对象（DTO），用于接口数据传递和验证
    /// </summary>
    public class ShirtDto
    {
        /// <summary>
        /// id
        /// </summary>
        [Shirts_ShirtIdValidation]
        public int Id { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        [Required(ErrorMessage = "此项不能为空")]
        public string? Brand { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        [Required(ErrorMessage = "此项不能为空")]
        public string? Color { get; set; }

        /// <summary>
        /// 尺寸
        /// </summary>
        [Required(ErrorMessage = "此项不能为空")]
        public int? Size { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Required(ErrorMessage = "此项不能为空")]
        [Shirts_GenderSizeValidation]
        public string? Gender { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [Required(ErrorMessage = "此项不能为空")]
        public int? MyProperty { get; set; }
    }
}