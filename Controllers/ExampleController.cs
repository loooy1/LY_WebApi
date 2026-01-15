using Asp.Versioning;
using LY_WebApi.Common;
using LY_WebApi.Filter.ActionValidations;
using LY_WebApi.Filter.ExceptionFilters;
using LY_WebApi.Models;
using LY_WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LY_WebApi.Controllers
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    [ApiController]
    [Route("LYwebapi/v{version:apiVersion}/[controller]")]
    [Shirts_ExceptionFilter]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [ApiVersion("2.0")]
    public class ExampleController : ControllerBase
    {
        /// <summary>
        /// 业务逻辑服务
        /// </summary>
        private readonly ExampleService service;

        /// <summary>
        /// 构造函数 注入业务服务
        /// </summary>
        public ExampleController(ExampleService service)
        {
            this.service = service;
        }

        #region 简单无数据查询

        /// <summary>
        /// get方法 从http的路由中获取数据
        /// </summary>
        /// <param name="RouteData"></param>
        /// <returns></returns>
        [HttpGet("dataFromRoute/{RouteData}")]
        public IActionResult AddDataFromRoute([FromRoute] string RouteData)
        {
            return Ok($"获取“路由”数据：{RouteData}");
        }

        /// <summary>
        /// 不同的get方法 要写不同的路由
        /// </summary>
        /// <param name="RouteData"></param>
        /// <returns></returns>
        [HttpGet("dataFromRoute2/{RouteData}")]
        public IActionResult AddDataFromRoute2([FromRoute] string RouteData)
        {
            return Ok($"不同GET方法获取“路由”数据：{RouteData}");
        }

        /// <summary>
        /// get方法 从http的 查询字符串 中获取数据
        /// </summary>
        /// <param name="QueryData"></param>
        /// <returns></returns>
        [HttpGet("dataFromQuery")]
        public IActionResult AddDataFromQuery([FromQuery] string QueryData)
        {
            return Ok($"获取“查询字符串数据”数据：{QueryData}");
        }

        /// <summary>
        /// get方法 从http的 头信息 中获取数据
        /// 头信息是键值对 必须指定 键 的名称
        /// </summary>
        /// <param name="HeaderData"></param>
        /// <returns></returns>
        [HttpGet("dataFromHeader")]
        public IActionResult AddDataFromHeader([FromHeader(Name = "test")] string HeaderData)
        {
            return Ok($"获取“头信息数据”数据：{HeaderData}");
        }

        #endregion



        /// <summary>
        /// 一般post方法 从http的body请求体中获取数据
        /// </summary>
        /// <param name="shirts"></param>
        /// <returns></returns>
        [HttpPost("addShirtData")]
        public async Task<IActionResult> AddShirtData([FromBody] Shirts shirts)
        {
            var result = await service.AddAsync(shirts);
            return Ok(ApiResponse.Success(result, msg: "添加数据成功"));
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        [HttpDelete("deleteShirtDataById/{id}")]
        [TypeFilter(typeof(Shirts_ValidateShirtIdFilterAttribute))]
        public async Task<IActionResult> DeleteShirtData(int id)
        {
            var res = await service.DeleteAsync(id);
            return Ok(ApiResponse.Success(res, msg: $"id：{id}的数据删除成功"));
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("updateShirtData")]
        public async Task<IActionResult> UpdateShirtData([FromBody] Shirts data)
        {
            var result = await service.UpdateAsync(data);
            return Ok(ApiResponse.Success(result, msg: $"数据更新成功"));
        }

        /// <summary>
        /// get方法 获取所有数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetShirtsData")]
        public async Task<IActionResult> GetShirts()
        {
            var res = await service.GetAllAsync();
            return Ok(ApiResponse.Success(res, msg: $"数据获取成功"));
        }

        /// <summary>
        /// get方法 根据id获取数据
        /// </summary>
        /// <param name="id"> 路由中的请求数据 </param>
        /// <returns> IActionResult 是 ASP.NET Core 中统一的 Action 返回接口，封装了所有 HTTP 响应细节 </returns>
        [HttpGet("getShirtDataById/{id}")]
        [TypeFilter(typeof(Shirts_ValidateShirtIdFilterAttribute))]
        public async Task<IActionResult> GetShirtDataById([FromRoute] int id)
        {
            var result = await service.GetByIdAsync(id);
            return Ok(ApiResponse.Success(result));
        }
        //todo 通过前端表单数据 获取数据 
    }
}
