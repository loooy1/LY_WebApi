using LY_WebApi.Models;
using LY_WebApi.Models.Repository;

namespace LY_WebApi.Services
{
    /// <summary>
    /// Shirts 实体的业务逻辑服务，提供增删改查异步操作
    /// </summary>
    public class ExampleService
    {
        private readonly SqlRepository<Shirts> _repository;

        /// <summary>
        /// 构造函数，注入 Shirts 仓储
        /// </summary>
        /// <param name="repository">Shirts 仓储</param>
        public ExampleService(SqlRepository<Shirts> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 新增一条 Shirts 数据
        /// </summary>
        /// <param name="entity">Shirts 实体</param>
        /// <returns>新增后的实体</returns>
        public async Task<Shirts> AddAsync(Shirts entity)
        {
            await _repository.Add(entity);
            return entity;
        }

        /// <summary>
        /// 根据主键删除一条 Shirts 数据
        /// </summary>
        /// <param name="id">Shirts 主键</param>
        /// <returns>删除是否成功</returns>
        public async Task<Shirts> DeleteAsync(int id)
        {
            return await _repository.Delete(id);
        }

        /// <summary>
        /// 更新一条 Shirts 数据
        /// </summary>
        /// <param name="entity">Shirts 实体</param>
        /// <returns>更新后的实体</returns>
        public async Task<Shirts> UpdateAsync(Shirts entity)
        {
            await _repository.Update(entity);
            return entity;
        }

        /// <summary>
        /// 获取所有 Shirts 数据
        /// </summary>
        /// <returns>Shirts 实体集合</returns>
        public async Task<IEnumerable<Shirts>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        /// <summary>
        /// 根据主键获取一条 Shirts 数据
        /// </summary>
        /// <param name="id">Shirts 主键</param>
        /// <returns>Shirts 实体或 null</returns>
        public async Task<Shirts?> GetByIdAsync(int id)
        {
            // 假设仓储有 GetById 方法，否则需自行实现
            return await _repository.GetById(id);
        }
    }
}
