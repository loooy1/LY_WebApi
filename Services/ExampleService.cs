using AutoMapper;
using LY_WebApi.Models;

namespace LY_WebApi.Services
{
    /// <summary>
    /// 示例服务，负责 Shirts 实体与 ShirtDto 的转换及业务操作
    /// </summary>
    public class ExampleService
    {
        private readonly LocalService<Shirts> _localService;
        private readonly IMapper _mapper;

        public ExampleService(LocalService<Shirts> localService, IMapper mapper)
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

            var result = await _localService.AddAsync(entity);
            return _mapper.Map<ShirtDto>(result);
        }

        /// <summary>
        /// 更新一条 ShirtDto 数据（DTO转实体后更新）
        /// </summary>
        public async Task<ShirtDto> UpdateAsync(ShirtDto dto)
        {
            var entity = _mapper.Map<Shirts>(dto);
            // 查询数据库原始数据，保留未在DTO中的字段
            var dbEntity = await _localService.GetByIdAsync(entity.Id);

            entity.GuidId ??= new Guid("00000000-1111-1111-1111-000000000000");
            // 其他需要保护的字段也可在此赋值

            var result = await _localService.UpdateAsync(entity);
            return _mapper.Map<ShirtDto>(result);
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
