using System;
using System.Threading;

namespace LY_WebApi.Common
{
    /// <summary>
    /// 通用方法
    /// </summary>
    public static class GeneralMethod
    {
        // 控制台输出锁，保证不同线程输出不交错
        private static readonly object _consoleLock = new();

        /// <summary>
        /// 重置控制台的字体+背景为系统默认颜色（线程安全）
        /// </summary>
        public static void ResetConsoleColor()
        {
            lock (_consoleLock)
            {
                ResetConsoleColorInternal();
            }
        }

        // 内部不加锁的重置函数，供在已持有锁的上下文中调用，避免死锁
        private static void ResetConsoleColorInternal()
        {
            Console.ForegroundColor = ConsoleColor.Gray;  // 控制台默认字体色
            Console.BackgroundColor = ConsoleColor.Black; // 控制台默认背景色
        }

        /// <summary>
        /// 自定义字体颜色打印内容（线程安全）
        /// </summary>
        /// <param name="msg">要打印的内容</param>
        /// <param name="fontColor">字体颜色</param>
        /// <param name="isLine">是否换行打印，true=换行(WriteLine)，false=不换行(Write)</param>
        private static void ConsoleWriteColor(string msg, ConsoleColor fontColor, bool isLine = true)
        {
            // 加锁保证同一时间只有一个线程操作控制台颜色和写入，避免颜色/输出混淆
            lock (_consoleLock)
            {
                try
                {
                    // 输出时附加线程ID和时间，便于排查并发问题
                    var prefix = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [线程:{Thread.CurrentThread.ManagedThreadId}] ";
                    Console.ForegroundColor = fontColor;
                    if (isLine)
                    {
                        Console.WriteLine(prefix + msg);
                    }
                    else
                    {
                        Console.Write(prefix + msg);
                    }
                }
                finally
                {
                    // 在持有锁的情况下恢复颜色（使用内部方法避免再次竞争锁）
                    ResetConsoleColorInternal();
                }
            }
        }

        /// <summary>
        /// 打印【成功日志】 绿色字体
        /// </summary>
        /// <param name="msg">成功信息</param>
        public static void PrintInfo(string msg)
        {
            ConsoleWriteColor($"[Info]：{msg}", ConsoleColor.Green);
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
