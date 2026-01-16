using System.ComponentModel.DataAnnotations;
using LY_WebApi.Filter.PropertyValidations;

namespace LY_WebApi.Models
{
    /// <summary>
    /// 衬衫实体类，仅包含属性，不包含数据验证特性
    /// </summary>
        public class Shirts
    {
        // id
        public int Id { get; set; }

        // 唯一标识（UUID）
        public Guid? GuidId { get; set; }

        // 品牌
        public string? Brand { get; set; }

        // 颜色
        public string? Color { get; set; }

        // 尺寸
        public int? Size { get; set; }

        // 性别
        public string? Gender { get; set; }

        // 价格
        public int? MyProperty { get; set; }

        // 重写ToString()，拼接所有属性
        public override string ToString()
        {
            return $"id为{Id}，品牌：{Brand ?? "未填写"}，颜色：{Color ?? "未填写"}，尺寸：{Size}，性别：{Gender ?? "未填写"}，价格：{MyProperty}元，GuidId：{GuidId}";
        }
    }
}
