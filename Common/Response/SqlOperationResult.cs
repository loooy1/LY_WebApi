using System;

namespace LY_WebApi.Common.Response
{
    /// <summary>
    /// 全局统一SQL操作结果类
    /// 只负责对外提供结果方法
    /// </summary>
    public class SqlOperationResult
    {
        /// <summary>
        /// 成功结果 (带返回数据，推荐！强类型无装箱)
        /// </summary>
        public static SqlOperationResult<T> Success<T>(T data = default, string msg = "操作成功")
        {
            return new SqlOperationResult<T> { IsSuccess = true, Message = msg, Data = data, DateTime = DateTime.Now };
        }

        /// <summary>
        /// 失败结果 (带返回数据)
        /// </summary>
        public static SqlOperationResult<T> Fail<T>(T data = default, string msg = "操作失败")
        {
            return new SqlOperationResult<T> { IsSuccess = false, Message = msg, Data = data, DateTime = DateTime.Now };
        }

        /// <summary>
        /// 成功结果 - 无返回数据，只返回提示语（无泛型，专门给删除/修改等场景用）
        /// </summary>
        public static SqlOperationResult<object> Success(string msg = "操作成功")
        {
            return new SqlOperationResult<object> { IsSuccess = true, Message = msg, Data = null, DateTime = DateTime.Now };
        }

        /// <summary>
        /// 失败结果 - 无返回数据，只返回提示语（无泛型）
        /// </summary>
        public static SqlOperationResult<object> Fail(string msg = "操作失败")
        {
            return new SqlOperationResult<object> { IsSuccess = false, Message = msg, Data = null, DateTime = DateTime.Now };
        }
    }

    /// <summary>
    /// 带泛型数据的SQL操作结果类
    /// 只负责数据结构
    /// </summary>
    public class SqlOperationResult<T>
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 操作信息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 操作数据
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime DateTime { get; set; }
    }
}