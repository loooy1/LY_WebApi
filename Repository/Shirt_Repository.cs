using LY_WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace LY_WebApi.Repository
{
    /// <summary>
    /// shirts 内存仓库
    /// </summary>
    public static class Shirt_Repository
    {
        //存储几个实例
        private static List<Shirts> shirts = new List<Shirts>()
        {
            new Shirts(){Id =1,Brand="品牌1",Color="黑",Size=5,Gender="男",MyProperty=50,GuidId=new Guid("00000000-0000-0000-0000-000000000001")},
            new Shirts(){Id =2,Brand="品牌2",Color="黑",Size=5,Gender="男",MyProperty=51,GuidId=new Guid("00000000-0000-0000-0000-000000000002")},
            new Shirts(){Id =3,Brand="品牌3",Color="黑",Size=5,Gender="男",MyProperty=52,GuidId=new Guid("00000000-0000-0000-0000-000000000003")},
            new Shirts(){Id =4,Brand="品牌4",Color="黑",Size=5,Gender="男",MyProperty=53,GuidId=new Guid("00000000-0000-0000-0000-000000000004")},
            new Shirts(){Id =5,Brand="品牌5",Color="黑",Size=5,Gender="男",MyProperty=54,GuidId=new Guid("00000000-0000-0000-0000-000000000005")},
            new Shirts(){Id =6,Brand="品牌6",Color="黑",Size=5,Gender="男",MyProperty=55,GuidId=new Guid("00000000-0000-0000-0000-000000000006")},

        };

        /// <summary>
        /// 获取所有shirts
        /// </summary>
        /// <returns> 返回shirts集合 </returns>
        public static List<Shirts> GetAllShirts()
        {
            return shirts;
        }

        /// <summary>
        /// 判断是否存在对应id的shirt
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool ShirtExists(int id)
        {
            return shirts.Any(x => x.Id == id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shirt"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool AddShirt(Shirts shirt, out ValidationProblemDetails result)
        {
            var maxId = shirts.Max(x => x.Id);
            shirt.Id = shirts.Any(x => x.Id == shirt.Id) ? maxId + 1 : shirt.Id;
            var problemDetails = new ValidationProblemDetails();

            //判断shirt的Guid是否存在于仓库，不存在则增加 存在则拒绝增加
            if (shirts.Any(x => x.GuidId == shirt.GuidId))
            {
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Validation Failed";
                problemDetails.Detail = $"已存在Guid为{shirt.GuidId}的衬衫，无法添加重复Guid的衬衫";

                result = problemDetails;
                return false;
            }


            shirts.Add(shirt);

            problemDetails.Status = StatusCodes.Status200OK;
            problemDetails.Title = "Validation Succeed";
            problemDetails.Detail = $"成功添加{shirt.ToString()}";

            result = problemDetails;
            return true;
        }


        public static bool UpdateShirt(Shirts shirt, out ValidationProblemDetails result)
        {
            var problemDetails = new ValidationProblemDetails();
            var shirtToUpdate = shirts.FirstOrDefault(x => x.Id == shirt.Id && x.GuidId == shirt.GuidId);

            //存在要更新的衬衫
            if (shirtToUpdate != null)
            {

                shirtToUpdate.Id = shirt.Id;
                shirtToUpdate.Brand = shirt.Brand;
                shirtToUpdate.Color = shirt.Color;
                shirtToUpdate.Size = shirt.Size;
                shirtToUpdate.Gender = shirt.Gender;

                problemDetails.Status = StatusCodes.Status200OK;
                problemDetails.Title = "Success";
                problemDetails.Detail = $"成功更新{shirt.ToString()}";

                result = problemDetails;
                return true;

            }

            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Title = "Failed";
            problemDetails.Detail = $"更新失败，Id或GuidId未找到对应的衬衫";

            result = problemDetails;
            return false;
        }

        /// <summary>
        /// 删除指定id的shirt
        /// </summary>
        /// <param name="shirtId"></param>
        /// <returns></returns>
        public static bool DeleteShirtsById(int shirtId, out ValidationProblemDetails result)
        {
            var shirtToDelete = shirts.FirstOrDefault(x => x.Id == shirtId);
            var problemDetails = new ValidationProblemDetails();
            if (shirtToDelete != null)
            {
                shirts.Remove(shirtToDelete);

                problemDetails.Status = StatusCodes.Status200OK;
                problemDetails.Title = "Success";
                problemDetails.Detail = $"删除成功:{shirtToDelete.ToString()}";

                result = problemDetails;
                return true;
            }

            problemDetails.Status = StatusCodes.Status404NotFound;
            problemDetails.Title = "Failed";
            problemDetails.Detail = $"未找到{shirtId}对应的衬衫";

            result = problemDetails;
            return false;
        }


        /// <summary>
        /// 返回指定id的shirt
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Shirts? GetShirtsById(int id)
        {
            return shirts.FirstOrDefault(x => x.Id == id);
        }


    }
}
