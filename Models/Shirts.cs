using System.ComponentModel.DataAnnotations;

namespace LY_WebApi.Models
{
    public class Shirts
    {
        //id
        public int ShirtsId { get; set; }

        // 唯一标识（UUID）
        [Required]
        public Guid? GuidId { get; set; }

        //品牌
        [Required]
        public string? Brand { get; set; }

        //颜色
        [Required]
        public string? Color { get; set; }

        //尺寸
        [Required]
        [Shirts_GenderSizeValidation]
        public int? Size { get; set; }

        //性别
        [Required]
        public string? Gender { get; set; }

        //价格
        [Required] //[Required]表示 此数据不可为null 否则抛出异常 建议结合 ?可空使用，使必须传此参数
        public int? MyProperty { get; set; }

        // 重写ToString()，拼接所有属性
        public override string ToString()
        {
            return $"id为{ShirtsId}，品牌：{Brand ?? "未填写"}，颜色：{Color ?? "未填写"}，尺寸：{Size}，性别：{Gender ?? "未填写"}，价格：{MyProperty}元，GuidId：{GuidId}";
        }
    }
}
