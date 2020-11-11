using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace Clansty.tianlang
{
    static class Timers
    {
        private static Timer t;

        internal static void Init()
        {
            t = new Timer(1000 * 60 * 60);
            t.Elapsed += async (_, __) =>
            {
                await UserInfo.CheckAllQmpAsync();
                Db.Commit();
            };
            t.AutoReset = true;
            t.Enabled = true;
        }
    }
}
