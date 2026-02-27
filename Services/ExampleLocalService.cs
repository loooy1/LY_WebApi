using AutoMapper;
using LY_WebApi.Models;

namespace LY_WebApi.Services
{
    /// <summary>
    /// 示例服务，负责 Shirts 实体与 ShirtDto 的转换及业务操作
    /// </summary>
    public class ExampleLocalService
    {
        private readonly LocalService<Shirts> _localService;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数，注入泛型本地服务和 AutoMapper 映射器
        /// </summary>
        /// <param name="localService"></param>
        /// <param name="mapper"></param>
        public ExampleLocalService(LocalService<Shirts> localService, IMapper mapper)
        {
            _localService = localService;
            _mapper = mapper;
        }

        /// <summary>
        /// 新增一条 ShirtDto 数据（DTO转实体后插入）
        /// </summary>
        public async Task<ShirtDto> AddAsync(ShirtDto dto)
        {
            var entity = _mapper.Map<Shirts>(dto);
            // 可补全实体类的非DTO字段
            entity.GuidId ??= new Guid("00000000-0000-0000-0000-000000000000");

            var result = await _localService.AddAsync(entity, x => x.Id == entity.Id);

            if(!result.IsSuccess)

                // 插入失败后抛出业务逻辑异常 让异常过滤器处理
                throw new InvalidOperationException(result.Message);

            return _mapper.Map<ShirtDto>(result.Data);
        }

        /// <summary>
        /// 更新一条 ShirtDto 数据（DTO转实体后更新）
        /// </summary>
        public async Task<ShirtDto> UpdateAsync(ShirtDto dto)
        {
            // 1. 先查数据库，获取【被跟踪的原始实体】（核心！）
            var dbEntity = await _localService.GetByIdAsync(dto.Id);
            if (dbEntity == null)
                throw new KeyNotFoundException($"未找到 Id={dto.Id} 的衬衫");

            // 2. 把 DTO 映射到【被跟踪的实体】（改这个实体才会触发属性通知）
            _mapper.Map(dto, dbEntity);
            dbEntity.GuidId ??= new Guid("00000000-1111-1111-1111-000000000000");

            // 3. 调用 UpdateAsync（此时去掉 _db.Update 也能更新，因为 dbEntity 被跟踪+属性通知标记）
            await _localService.UpdateAsync(dbEntity);

            return _mapper.Map<ShirtDto>(dbEntity);
        }



        /// <summary>
        /// 根据主键获取一条 ShirtDto 数据
        /// </summary>
        public async Task<ShirtDto?> GetByIdAsync(int id)
        {
            var entity = await _localService.GetByIdAsync(id);
            return _mapper.Map<ShirtDto>(entity);
        }

        /// <summary>
        /// 获取所有 ShirtDto 数据
        /// </summary>
        public async Task<IEnumerable<ShirtDto>> GetAllAsync()
        {
            var entities = await _localService.GetAllAsync();
            return _mapper.Map<IEnumerable<ShirtDto>>(entities);
        }

        /// <summary>
        /// 根据主键删除一条 ShirtDto 数据
        /// </summary>
        public async Task<ShirtDto> DeleteAsync(int id)
        {
            var result = await _localService.DeleteAsync(id);
            return _mapper.Map<ShirtDto>(result);
        }
    }
}
