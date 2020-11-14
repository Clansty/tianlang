using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CornSDK;

namespace Clansty.tianlang
{
    public static class Watchdog
    {
        private static string checkUuid = "";

        private static long lasterrtime = 0;

        private static readonly HashSet<long> accounts = new HashSet<long>()
        {
            839827911,
            C.self,
            2603367939
        };

        private static HashSet<(long, long)> session;

        public static void RunCheck()
        {
            checkUuid = Guid.NewGuid().ToString();
            session = new HashSet<(long, long)>();

            foreach (var i in accounts)
            {
                C.QQ.SendGroupMsg(G.check, checkUuid, false, i);
            }

            Task.Run(Timer);
        }

        public static void Msg(GroupMsgArgs e)
        {
            if (e.Msg == checkUuid)
            {
                session.Add((e.FromQQ, e.RecvQQ));
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
                    // no notification in 2 hours
                    return;
                }
            }
            if (session.Count!=6)
            {
                lasterrtime = DateTime.Now.ToBinary();
                new WebClient().DownloadString(
                    $"https://sc.ftqq.com/SCU126714T7691001cb3cc816392272193222ff72f5fafd50add6a5.send?" +
                    $"text=server check got {session.Count} responses");
            }
        }
    }
}