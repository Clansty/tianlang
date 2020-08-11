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
        }
    }
}
