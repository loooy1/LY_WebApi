using System.ComponentModel.DataAnnotations;
using LY_WebApi.Filter.PropertyValidations;

namespace LY_WebApi.Models
{
    public class Shirts
    {
        //id
        [Shirts_ShirtIdValidation]
        public int ShirtsId { get; set; }

        // 唯一标识（UUID）
        [Required(ErrorMessage = "此项不能为空")]
        public Guid? GuidId { get; set; }

        //品牌
        [Required(ErrorMessage = "此项不能为空")]
        public string? Brand { get; set; }

        //颜色
        [Required(ErrorMessage = "此项不能为空")]
        public string? Color { get; set; }

        //尺寸
        [Required(ErrorMessage = "此项不能为空")]
        [Shirts_GenderSizeValidation]
        public int? Size { get; set; }

        //性别
        [Required(ErrorMessage = "此项不能为空")]
        public string? Gender { get; set; }

        //价格
        [Required(ErrorMessage = "此项不能为空")] //[Required]表示 此数据不可为null 否则抛出异常 建议结合 ?可空使用，使必须传此参数
        public int? MyProperty { get; set; }

        // 重写ToString()，拼接所有属性
        public override string ToString()
        {
            return $"id为{ShirtsId}，品牌：{Brand ?? "未填写"}，颜色：{Color ?? "未填写"}，尺寸：{Size}，性别：{Gender ?? "未填写"}，价格：{MyProperty}元，GuidId：{GuidId}";
        }
    }

}
