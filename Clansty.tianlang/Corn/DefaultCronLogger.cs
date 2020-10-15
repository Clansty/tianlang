using System;
using System.Collections.Generic;
using System.Text;

namespace CornSDK
{
    /// <summary>
    /// 默认的日志记录器，默认输出时间和日志内容并有不同的颜色显示
    /// </summary>
    class DefaultCronLogger : ICronLogger
    {
        public void Log(string text, CornLogLevel level = CornLogLevel.Info)
        {
            switch (level)
            {
                case CornLogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case CornLogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case CornLogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case CornLogLevel.Succeed:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case CornLogLevel.IncommingMsg:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                default:
                    break;
            }
            Console.WriteLine($"[{DateTime.Now}]{text}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
