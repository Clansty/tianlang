using Clansty.tianlang;
using System;
using System.Runtime.InteropServices;

namespace BotTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Sql.Init();
            var u = new User(999888777);
            C.WriteLn(u.ToXml());
            Sql.Commit();
            Console.ReadLine();
        }
    }
}
