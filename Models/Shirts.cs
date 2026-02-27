using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
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
        //public string? Color { get; set; }


        // ========== 给EF Core用的Color属性通知核心 ==========
        // 1. 私有字段（存储实际值，EF Core最终映射数据库的是这个值）
        private string? _color;

        // 2. 公共属性（封装赋值逻辑，触发EF Core能识别的通知）
        public string? Color
        {
            get => _color;
            set
            {
                // 仅值变化时触发通知（避免EF Core无效标记）
                if (_color != value)
                {
                    _color = value;
                    // 触发PropertyChanged事件，EF Core会实时捕获
                    OnPropertyChanged();
                }
            }
        }
        // ==================================================

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

        // ========== EF Core识别的属性通知核心接口 ==========
        // EF Core会自动订阅这个事件
        public event PropertyChangedEventHandler? PropertyChanged;

        // 触发事件的通用方法（EF Core能识别的标准写法）
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        // ==================================================
    }
}
