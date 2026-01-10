using LY_WebApi.Filter.ActionValidations;
using LY_WebApi.Filter.ExceptionFilters;
using LY_WebApi.Models;
using LY_WebApi.Models.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LY_WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExampleController : ControllerBase
    {
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
        /// get方法 从http的路由中获取数据 并专注于返回类型
        /// </summary>
        /// <param name="id"> 路由中的请求数据 </param>
        /// <returns> IActionResult 是 ASP.NET Core 中统一的 Action 返回接口，封装了所有 HTTP 响应细节 </returns>
        [HttpGet("dataFromRouteAndTestResult/{id}")]
        [Shirts_ValidateShirtIdFilter] //控制器方法过滤器
        public IActionResult AddDataFromRouteAndTestResult([FromRoute] int id)
        {
            //返回成功200
            return Ok(Shirt_Repository.GetShirtsById(id));
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

        /// <summary>
        /// 一般post方法 从http的body请求体中获取数据
        /// </summary>
        /// <param name="shirts"></param>
        /// <returns></returns>
        [HttpPost("dataFromBody")]
        public IActionResult AddShirts([FromBody] Shirts shirts)
        {

            if (Shirt_Repository.AddShirt(shirts, out var result))
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [Shirts_ValidateShirtIdFilter]
        public IActionResult DeleteData(int id)
        {
            if (Shirt_Repository.DeleteShirtsById(id, out var result))

                return Ok(result);

            return NotFound(result);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [Shirts_UpdateExceptionFilter]
        public IActionResult UpdateData([FromBody] Shirts data)
        {

            if (Shirt_Repository.UpdateShirt(data, out var result))
                return Ok(result);


            return NotFound(result);
        }
        /// <summary>
        /// get方法 从http的body请求体中获取数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("dataFromMemory")]
        public IActionResult GetShirts()
        {

            return Ok(Shirt_Repository.GetAllShirts());
        }


        //todo 通过前端表单数据 获取数据 


    }
}
