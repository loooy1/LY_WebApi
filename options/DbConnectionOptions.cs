using System.ComponentModel.DataAnnotations;

namespace LY_WebApi.options
{
    /// <summary>
    /// 数据库连接配置（对应配置文件 ConnectionStrings 节点）
    /// </summary>
    public class DbConnectionOptions
    {
        /// <summary>
        /// 配置节点名
        /// </summary>
        public const string SectionName = "ConnectionStrings";

        /// <summary>
        /// 写库连接字符串
        /// </summary>
        [Required(ErrorMessage = "写库连接字符串（WriteConnection）不能为空")]
        public string WriteConnection { get; set; } = string.Empty;

        /// <summary>
        /// 读库连接字符串列表（初始化空列表，避免空指针）
        /// </summary>
        public List<string> ReadConnectionList { get; set; } = new List<string>();
    }
}
