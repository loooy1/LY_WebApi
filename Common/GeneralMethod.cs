namespace LY_WebApi.Common
{
    /// <summary>
    /// 通用方法
    /// </summary>
    public class GeneralMethod
    {
        /// <summary>
        /// 重置控制台的字体+背景为系统默认颜色
        /// </summary>
        public static void ResetConsoleColor()
        {
            Console.ForegroundColor = ConsoleColor.Gray;  // 控制台默认字体色
            Console.BackgroundColor = ConsoleColor.Black; // 控制台默认背景色
        }

        /// <summary>
        /// 自定义字体颜色打印内容【常用，推荐】
        /// </summary>
        /// <param name="msg">要打印的内容</param>
        /// <param name="fontColor">字体颜色</param>
        /// <param name="isLine">是否换行打印，true=换行(WriteLine)，false=不换行(Write)</param>
        public static void ConsoleWriteColor(string msg, ConsoleColor fontColor, bool isLine = true)
        {
            try
            {
                // 设置字体颜色
                Console.ForegroundColor = fontColor;
                if (isLine)
                {
                    Console.WriteLine(msg);
                }
                else
                {
                    Console.Write(msg);
                }
            }
            finally
            {
                // 无论是否异常，都重置颜色，核心保证！
                ResetConsoleColor();
            }
        }

        /// <summary>
        /// 打印【成功日志】 绿色字体
        /// </summary>
        /// <param name="msg">成功信息</param>
        public static void PrintSuccess(string msg)
        {
            ConsoleWriteColor($"[Success]：{msg}", ConsoleColor.Green);
        }

        /// <summary>
        /// 打印【错误日志】 红色字体 (适配你的异常/EF报错/业务失败)
        /// </summary>
        /// <param name="msg">错误信息</param>
        public static void PrintError(string msg)
        {
            ConsoleWriteColor($"[Error]：{msg}", ConsoleColor.Red);
        }

        /// <summary>
        /// 打印【警告日志】 黄色字体 (适配你的业务提醒/非致命错误)
        /// </summary>
        /// <param name="msg">警告信息</param>
        public static void PrintWarning(string msg)
        {
            ConsoleWriteColor($"[Warning]：{msg}", ConsoleColor.Yellow);
        }

    }
}
