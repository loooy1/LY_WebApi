using LY_WebApi.Repository;

namespace LY_WebApi.Services
{
    /// <summary>
    /// 泛型本地服务，封装通用的增删改查操作
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public class LocalService<T> where T : class
    {
        private readonly SqlRepository<T> _repository;

        /// <summary>
        /// 构造函数，注入泛型仓储
        /// </summary>
        /// <param name="repository">泛型仓储</param>
        public LocalService(SqlRepository<T> repository)
        {
            _repository = repository;
        }   

        /// <summary>
        /// 新增一条数据
        /// </summary>
        public async Task<T> AddAsync(T entity)
        {
            return await _repository.Add(entity);
        }

        /// <summary>
        /// 根据主键删除一条数据
        /// </summary>
        public async Task<T> DeleteAsync(int id)
        {
            return await _repository.Delete(id);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public async Task<T> UpdateAsync(T entity)
        {
            await _repository.Update(entity);
            return entity;
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        /// <summary>
        /// 根据主键获取一条数据
        /// </summary>
        public async Task<T> GetByIdAsync(int id)
        {
            return await _repository.GetById(id);
        }
    }
}
