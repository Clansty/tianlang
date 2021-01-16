using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Mirai_CSharp.Models;

namespace Clansty.tianlang
{
    public static class Watchdog
    {
        private static string checkUuid = "";

        private static long lasterrtime;

        private static HashSet<(long, long)> session = new HashSet<(long, long)>();

        public static void RunCheck()
        {
            checkUuid = Guid.NewGuid().ToString();
            session.Clear();

            C.QQ.Clansty.SendGroupMessageAsync(G.check, new PlainMessage(checkUuid));
            C.QQ.NthsBot.SendGroupMessageAsync(G.check, new PlainMessage(checkUuid));

            Task.Run(Timer);
        }

        public static void Msg(long fromQQ, long recvQQ, string uuid)
        {
            if (uuid == checkUuid)
            {
                session.Add((fromQQ, recvQQ));
            }
        }

        private static async void Timer()
        {
            await Task.Delay(20000);
            if (lasterrtime != 0)
            {
                var now = DateTime.Now;
                var last = DateTime.FromBinary(lasterrtime);
                if (now - last < TimeSpan.FromHours(2))
                {
                    // no notification in 1 hours
                    return;
                }
            }

            if (session.Count != 2)
            {
                lasterrtime = DateTime.Now.ToBinary();
                new WebClient().DownloadString(
                    "https://sc.ftqq.com/SCU126714T7691001cb3cc816392272193222ff72f5fafd50add6a5.send?" +
                    $"text=server check got {session.Count} responses");
            }
        }
    }
}