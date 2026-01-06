using System.ComponentModel.DataAnnotations;

namespace LY_WebApi.options
{
    /// <summary>
    /// 应用基础配置（对应配置文件 BaseSetting 节点）
    /// </summary>
    public class BaseSettingOptions
    {
        /// <summary>
        /// 配置节点名（与配置文件一级节点一致）
        /// </summary>
        public const string SectionName = "BaseSetting";

        /// <summary>
        /// 允许的主机（框架内置配置，归到基础配置统一管理）
        /// </summary>
        public string AllowedHosts { get; set; } = "*";

        /// <summary>
        /// 全局唯一ID
        /// </summary>
        [Required(ErrorMessage = "BaseSetting.Id 不能为空")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "BaseSetting.Name 不能为空")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// IOC自动注册的程序集列表（dll逗号分隔）
        /// </summary>
        [Required(ErrorMessage = "BaseSetting.iocConfig 不能为空")]
        public string IocConfig { get; set; } = string.Empty;

        /// <summary>
        /// 语言类型（仅支持 english/chinese）
        /// </summary>
        [RegularExpression("^english|chinese$", ErrorMessage = "languagetype 仅支持 english 或 chinese")]
        public string LanguageType { get; set; } = "chinese";
    }

}
