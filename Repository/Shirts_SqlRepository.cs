using System.Linq.Expressions;
using LY_WebApi.Common.Response;
using LY_WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace LY_WebApi.Repository
{
    /// <summary>
    /// 操作数据库的泛型仓库
    /// </summary>
    public class SqlRepository<T> where T : class
    {
        private readonly AppDbContext _db;

        /// <summary>
        /// 构造函数 依赖注入[数据库操作上下文]
        /// </summary>
        /// <param name="db"></param>
        public SqlRepository(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// 根据id查询(查不到数据抛出异常)  
        /// todo根据任何一个列去查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetById(int id)
        {
            // AsNoTracking() 关闭跟踪，提升查询性能，纯查询场景推荐
            var res = await _db.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => EF.Property<int>(x, "Id") == id);
            if (res == null)
            {
                throw new KeyNotFoundException($"根据ID：{id}，未查询到对应数据！");
            }
            return res;

        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> GetAll()
        {
            // ✅ 优化点3：AsNoTracking() 关闭跟踪，提升查询性能
            return await _db.Set<T>().AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "修改的实体数据不能为空");
            }
            _db.Set<T>().Update(entity);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> Delete(int id)
        {
            var entity = await GetById(id);
            _db.Set<T>().Remove(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// 检查实体是否存在（根据条件）
        /// </summary>
        /// <param name="predicate">检查条件，如 x => x.Id == id 或 x => x.Name == name</param>
        /// <returns>true 如果存在</returns>
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _db.Set<T>().AnyAsync(predicate);
        }

        /// <summary>
        /// 添加（如果不存在，根据条件检查）
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="predicate">检查条件</param>
        /// <returns>添加的实体或现有实体</returns>
        public async Task<SqlOperationResult<T>> Add(T entity, Expression<Func<T, bool>> predicate)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "添加的实体数据不能为空");
            }

            // 检查是否存在
            if (await ExistsAsync(predicate))
            {
                // 已存在，返回现有实体（或抛异常，根据业务需求）
                return SqlOperationResult.Fail(entity, "实体已存在，无法添加重复数据");
            }

            // 不存在，添加新实体
            await _db.Set<T>().AddAsync(entity);
            await _db.SaveChangesAsync();
            return SqlOperationResult.Success(entity, "实体添加成功");
        }
    }
}