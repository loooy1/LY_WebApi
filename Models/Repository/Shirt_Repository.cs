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
            new Shirts(){ShirtsId =1,Brand="品牌1",Color="黑",Size=5,Gender="男",MyProperty=50,GuidId=new Guid("00000000-0000-0000-0000-000000000001")},
            new Shirts(){ShirtsId =2,Brand="品牌2",Color="黑",Size=5,Gender="男",MyProperty=51,GuidId=new Guid("00000000-0000-0000-0000-000000000002")},
            new Shirts(){ShirtsId =3,Brand="品牌3",Color="黑",Size=5,Gender="男",MyProperty=52,GuidId=new Guid("00000000-0000-0000-0000-000000000003")},
            new Shirts(){ShirtsId =4,Brand="品牌4",Color="黑",Size=5,Gender="男",MyProperty=53,GuidId=new Guid("00000000-0000-0000-0000-000000000004")},
            new Shirts(){ShirtsId =5,Brand="品牌5",Color="黑",Size=5,Gender="男",MyProperty=54,GuidId=new Guid("00000000-0000-0000-0000-000000000005")},
            new Shirts(){ShirtsId =6,Brand="品牌6",Color="黑",Size=5,Gender="男",MyProperty=55,GuidId=new Guid("00000000-0000-0000-0000-000000000006")},

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shirt"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool AddShirt(Shirts shirt, out string result)
        {
            var maxId = shirts.Max(x => x.ShirtsId);
            shirt.ShirtsId = maxId + 1;

            //判断shirt的Guid是否存在，不存在则增加 存在则拒绝增加
            if (shirts.Any(x => x.GuidId == shirt.GuidId))
            {
                result = $"已存在Guid为{shirt.GuidId}的衬衫，无法添加重复Guid的衬衫";
                return false;
            }


            shirts.Add(shirt);
            result = $"成功添加{shirt.ToString()}";
            return true;
        }
    }
}
