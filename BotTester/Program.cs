using Clansty.tianlang;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Sql.Init();
            var a = Sql.users;
            var b = a.Rows.Find(54321);
            var c = a.Rows.Add(12345, "aa", "bb", 0, 0, 0, 0, 0);
            Sql.Commit();
        }
    }
}
