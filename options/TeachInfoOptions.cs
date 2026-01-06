using System.ComponentModel.DataAnnotations;

namespace LY_WebApi.options
{
    /// <summary>
    /// 教学信息配置（对应配置文件 TeachInfo 节点）
    /// </summary>
    public class TeachInfoOptions
    {
        /// <summary>
        /// 配置节点名
        /// </summary>
        public const string SectionName = "TeachInfo";

        /// <summary>
        /// 教学ID
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "TeachInfo.Id 必须大于0")]
        public int Id { get; set; }

        /// <summary>
        /// 教学名称
        /// </summary>
        [Required(ErrorMessage = "TeachInfo.Name 不能为空")]
        public string Name { get; set; } = string.Empty;
    }
}
