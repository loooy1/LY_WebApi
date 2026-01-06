namespace LY_WebApi.Models.Repository
{
    /// <summary>
    /// shirts 内存仓库
    /// </summary>
    public static class Shirt_Repository
    {
        //存储几个实例
        private static List<Shirts> shirts = new List<Shirts>()
        {
            new Shirts(){ShirtsId =1,Brand="品牌1",Color="黑",Size=5,Gender="男",MyProperty=50 },
            new Shirts(){ShirtsId =2,Brand="品牌2",Color="黑",Size=5,Gender="男",MyProperty=51 },
            new Shirts(){ShirtsId =3,Brand="品牌3",Color="黑",Size=5,Gender="男",MyProperty=52 },
            new Shirts(){ShirtsId =4,Brand="品牌4",Color="黑",Size=5,Gender="男",MyProperty=53 },
            new Shirts(){ShirtsId =5,Brand="品牌5",Color="黑",Size=5,Gender="男",MyProperty=54 },
            new Shirts(){ShirtsId =6,Brand="品牌6",Color="黑",Size=5,Gender="男",MyProperty=55 },
        };

        /// <summary>
        /// 判断是否存在对应id的shirt
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool ShirtExists(int id)
        {
            return shirts.Any(x => x.ShirtsId == id);
        }

        /// <summary>
        /// 返回指定id的shirt
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Shirts? GetShirtsById(int id)
        {
            return shirts.FirstOrDefault(x => x.ShirtsId == id);
        }
    }
}
