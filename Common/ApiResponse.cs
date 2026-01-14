using System;

namespace LY_WebApi.Common
{
    /// <summary>
    /// 全局统一API响应结果类
    /// 只负责对外提供响应方法
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// 成功响应 (带返回数据，推荐！强类型无装箱)
        /// </summary>
        public static ApiResponseResult<T> Success<T>(T data = default, string msg = "操作成功", int code = 200)
        {
            return new ApiResponseResult<T> { Code = code, Message = msg, Data = data, DateTime = DateTime.Now };
        }

        /// <summary>
        /// 失败响应 (带返回数据)
        /// </summary>
        public static ApiResponseResult<T> Fail<T>(T data = default, string msg = "操作失败", int code = 400)
        {
            return new ApiResponseResult<T> { Code = code, Message = msg, Data = data, DateTime = DateTime.Now};
        }

        /// <summary>
        /// 成功响应 - 无返回数据，只返回提示语（无泛型，专门给删除/修改等场景用）
        /// </summary>
        public static ApiResponseResult<object> Success(string msg = "操作成功", int code = 200)
        {
            return new ApiResponseResult<object> { Code = code, Message = msg, Data = null, DateTime = DateTime.Now };
        }

        /// <summary>
        /// 失败响应 - 无返回数据，只返回提示语（无泛型）
        /// </summary>
        public static ApiResponseResult<object> Fail(string msg = "操作失败", int code = 400)
        {
            return new ApiResponseResult<object> { Code = code, Message = msg, Data = null, DateTime = DateTime.Now };
        }
    }

    /// <summary>
    /// 带泛型数据的API响应结果类
    /// 只负责数据结构
    /// </summary>
    public class ApiResponseResult<T>
    {
        /// <summary>
        /// http返回码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 响应信息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 响应数据
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// 响应时间
        /// </summary>
        public DateTime DateTime { get; set; }
}
}

