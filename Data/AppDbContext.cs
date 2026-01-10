using LY_WebApi.Models;
using Microsoft.EntityFrameworkCore;

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


        public DbSet<Shirts> shirt { get; set; }

        /// <summary>
        /// 重写  数据库表的「自定义配置 + 初始化数据」入口
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //把数据写入到数据库中
            modelBuilder.Entity<Shirts>().HasData(
                new Shirts() { ShirtsId = 1, Brand = "品牌1", Color = "黑", Size = 5, Gender = "男", MyProperty = 50, GuidId = new Guid("00000000-0000-0000-0000-000000000001") },
                new Shirts() { ShirtsId = 2, Brand = "品牌2", Color = "黑", Size = 5, Gender = "男", MyProperty = 51, GuidId = new Guid("00000000-0000-0000-0000-000000000002") },
                new Shirts() { ShirtsId = 3, Brand = "品牌3", Color = "黑", Size = 5, Gender = "男", MyProperty = 52, GuidId = new Guid("00000000-0000-0000-0000-000000000003") },
                new Shirts() { ShirtsId = 4, Brand = "品牌4", Color = "黑", Size = 5, Gender = "男", MyProperty = 53, GuidId = new Guid("00000000-0000-0000-0000-000000000004") },
                new Shirts() { ShirtsId = 5, Brand = "品牌5", Color = "黑", Size = 5, Gender = "男", MyProperty = 54, GuidId = new Guid("00000000-0000-0000-0000-000000000005") },
                new Shirts() { ShirtsId = 6, Brand = "品牌6", Color = "黑", Size = 5, Gender = "男", MyProperty = 55, GuidId = new Guid("00000000-0000-0000-0000-000000000006") }
                );
        }
    }
}
