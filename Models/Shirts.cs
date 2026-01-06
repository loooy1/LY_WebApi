using System.ComponentModel.DataAnnotations;

namespace LY_WebApi.Models
{
    public class Shirts
    {
        //id
        public int ShirtsId { get; set; }

        //品牌
        public string? Brand { get; set; }

        //颜色
        public string? Color { get; set; }

        //尺寸
        [Shirts_GenderSizeValidation]
        public int Size { get; set; }

        //性别
        public string? Gender { get; set; }

        //价格
        [Required] //[Required]表示 此数据不可为null 否则抛出异常 建议结合 ?可空使用，使必须传此参数
        public int? MyProperty { get; set; }

        // 重写ToString()，拼接所有属性
        public override string ToString()
        {
            return $"id为{ShirtsId}，品牌：{Brand ?? "未填写"}，颜色：{Color ?? "未填写"}，尺寸：{Size}，性别：{Gender ?? "未填写"}，价格：{MyProperty}元";
        }
    }
}
