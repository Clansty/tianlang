using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Native.Sdk.Cqp;

namespace Clansty.tianlang
{
    public static class C
    {
        /*
         * TODO:
         * 號碼全用 long
         * 新生群名片标准化
         * 检测是否成员来决定是否推荐大群/新生群
         * 新生群后台实名实现
         */
        
        public const string Version = "3.1.17.6";//20200731
        public static CQApi CQApi = null;

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
