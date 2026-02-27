using LY_WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LY_WebApi.Data
{
    /// <summary>
    /// 【负责数据库连接】→ 连接 MySQL 的核心
    /// 【自动创建数据库表】→ 迁移的核心
    /// 【封装所有数据库操作】→ 你写接口的核心
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// 构造函数 用于从 program.cs文件读取到连接字符串
        /// </summary>
        /// <param name="options"></param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        /// <summary>
        /// 映射数据库shirt表的属性 
        /// 通过这个属性 就可以对shirt表进行增删改查操作
        /// </summary>
        public DbSet<Shirts> shirt { get; set; }

        /// <summary>
        /// 重写  数据库表的「自定义配置 + 初始化数据」入口
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //种子数据 把数据写入到数据库中
            modelBuilder.Entity<Shirts>().HasData(
                new Shirts() { Id = 1, Brand = "品牌1", Color = "黑", Size = 5, Gender = "男", MyProperty = 50, GuidId = new Guid("00000000-0000-0000-0000-000000000001") },
                new Shirts() { Id = 2, Brand = "品牌2", Color = "黑", Size = 5, Gender = "男", MyProperty = 51, GuidId = new Guid("00000000-0000-0000-0000-000000000002") },
                new Shirts() { Id = 3, Brand = "品牌3", Color = "黑", Size = 5, Gender = "男", MyProperty = 52, GuidId = new Guid("00000000-0000-0000-0000-000000000003") },
                new Shirts() { Id = 4, Brand = "品牌4", Color = "黑", Size = 5, Gender = "男", MyProperty = 53, GuidId = new Guid("00000000-0000-0000-0000-000000000004") },
                new Shirts() { Id = 5, Brand = "品牌5", Color = "黑", Size = 5, Gender = "男", MyProperty = 54, GuidId = new Guid("00000000-0000-0000-0000-000000000005") },
                new Shirts() { Id = 6, Brand = "品牌6", Color = "黑", Size = 5, Gender = "男", MyProperty = 55, GuidId = new Guid("00000000-0000-0000-0000-000000000006") }
                );
        }

        /// <summary>
        /// 核心：配置数据库连接 + 开启 SQL 日志  记得在appsetting中更改EF日志级别
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 防止重复配置
            if (optionsBuilder.IsConfigured) return;

            string connStr = "Server=localhost;Port=3306;Database=WebApi_DB;User=root;Password=123456;CharSet=utf8mb4;SslMode=None;AllowPublicKeyRetrieval=True;";

            // 配置 MySQL 驱动 + 开启 SQL 日志
            optionsBuilder
                .UseMySql(
                    connectionString: connStr,
                    // 指定 MySQL 服务器版本（根据你的MySQL版本改，比如 8.0/5.7）
                    serverVersion: new MySqlServerVersion(new Version(8, 0, 30))
                )

            // 开启 EF Core 日志，输出执行的 SQL 语句（核心！）
                .LogTo(
                Console.WriteLine, // 把日志输出到控制台
                new[] { RelationalEventId.CommandExecuting }// 只输出“执行SQL”相关的日志（过滤无关日志）
            );
        }
    }
}
