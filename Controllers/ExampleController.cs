using Asp.Versioning;
using LY_WebApi.Common;
using LY_WebApi.Filter.ActionValidations;
using LY_WebApi.Filter.ExceptionFilters;
using LY_WebApi.Models;
using LY_WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using LY_WebApi.Filters.ResourceFilter;

namespace LY_WebApi.Controllers
{
    /// <summary>
    /// 测试控制器，所有业务数据均通过 ShirtDto 进行输入输出
    /// </summary>
    [ApiController]
    [Route("LYwebapi/v{version:apiVersion}/[controller]")]
    [Shirts_ExceptionFilter]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [ApiVersion("2.0")]
    public class ExampleController : ControllerBase
    {
        private readonly ExampleService service;
        private readonly IMapper mapper;

        public ExampleController(ExampleService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        #region 简单无数据查询（保留原样）

        [HttpGet("dataFromRoute/{RouteData}")]
        public IActionResult AddDataFromRoute([FromRoute] string RouteData)
        {
            return Ok($"获取“路由”数据：{RouteData}");
        }

        [HttpGet("dataFromRoute2/{RouteData}")]
        public IActionResult AddDataFromRoute2([FromRoute] string RouteData)
        {
            return Ok($"不同GET方法获取“路由”数据：{RouteData}");
        }

        [HttpGet("dataFromQuery")]
        public IActionResult AddDataFromQuery([FromQuery] string QueryData)
        {
            return Ok($"获取“查询字符串数据”数据：{QueryData}");
        }

        [HttpGet("dataFromHeader")]
        public IActionResult AddDataFromHeader([FromHeader(Name = "test")] string HeaderData)
        {
            return Ok($"获取“头信息数据”数据：{HeaderData}");
        }

        #endregion

        /// <summary>
        /// 新增衬衫数据（使用DTO）
        /// </summary>
        [HttpPost("addShirtData")]
        [TypeFilter(typeof(StrictFieldValidationFilter<ShirtDto>))]
        public async Task<IActionResult> AddShirtData([FromBody] ShirtDto dto)
        {
            var resultDto = await service.AddAsync(dto);
            return Ok(ApiResponse.Success(resultDto, msg: "添加数据成功"));
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        [HttpDelete("deleteShirtDataById/{id}")]
        [TypeFilter(typeof(Shirts_ValidateShirtIdFilterAttribute))]
        public async Task<IActionResult> DeleteShirtData(int id)
        {
            var resDto = await service.DeleteAsync(id);
            return Ok(ApiResponse.Success(resDto, msg: $"id：{id}的数据删除成功"));
        }

        /// <summary>
        /// 更新数据（使用DTO）
        /// </summary>
        [HttpPut("updateShirtData")]
        public async Task<IActionResult> UpdateShirtData([FromBody] ShirtDto dto)
        {
            var resultDto = await service.UpdateAsync(dto);
            return Ok(ApiResponse.Success(resultDto, msg: $"数据更新成功"));
        }

        /// <summary>
        /// 获取所有衬衫数据（返回DTO集合）
        /// </summary>
        [HttpGet("GetShirtsData")]
        public async Task<IActionResult> GetShirts()
        {
            var resDtoList = await service.GetAllAsync();
            return Ok(ApiResponse.Success(resDtoList, msg: $"数据获取成功"));
        }

        /// <summary>
        /// 根据id获取衬衫数据（返回DTO）
        /// </summary>
        [HttpGet("getShirtDataById/{id}")]
        [TypeFilter(typeof(Shirts_ValidateShirtIdFilterAttribute))]
        public async Task<IActionResult> GetShirtDataById([FromRoute] int id)
        {
            var resultDto = await service.GetByIdAsync(id);
            return Ok(ApiResponse.Success(resultDto));
        }
        //todo 通过前端表单数据 获取数据 
    }
}
