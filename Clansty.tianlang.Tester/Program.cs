using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clansty.tianlang.Tester
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new NamesMan());
            Rds.pool = new PooledRedisClientManager(233, 10, "101.132.178.136:6379");
            var client = Rds.GetClient();
            var todo = "".Split(',');
            foreach (var i in todo)
            {

            }
            MessageBox.Show("Test");
        }
    }
}
