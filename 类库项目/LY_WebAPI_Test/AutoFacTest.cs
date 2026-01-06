namespace LY_WebAPI_Test
{
class Program
{
    static void Main()
    {
        Console.Write("总头数：");
        int h = int.Parse(Console.ReadLine());
        Console.Write("总脚数：");
        int f = int.Parse(Console.ReadLine());
        
        int r = (f - 2 * h) / 2;
        Console.WriteLine($"兔子：{r}，鸡：{h - r}");
    }
    }
}
