using AutoMapper;
using LY_WebApi.Models;

namespace LY_WebApi.Common
{
    /// <summary>
    /// AutoMapper 映射配置，关联 Shirts 实体类与 ShirtDto 数据传输对象
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AutoMapperProfile()
        {
            // 实体类 <-> DTO 映射
            CreateMap<ShirtDto, Shirts>()
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));

            CreateMap<Shirts, ShirtDto>()
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));
        }
    }
}