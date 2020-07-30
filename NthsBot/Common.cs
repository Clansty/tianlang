using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clansty.tianlang
{
    public static class C
    {
        /*
         * TODO:
         * 新生群名片标准化
         * 检测是否成员来决定是否推荐大群/新生群
         * 新生群后台实名实现
         */
        
        public const string Version = "3.0.17.5";//20200730

        public static void Write(string text, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void WriteLn(string text, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
    }
}
