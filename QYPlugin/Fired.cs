using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace Clansty.tianlang
{
    public static class Fired
    {
        public static void Cron(bool test = false)
        {
            C.WriteLn("fire cron");
            string last = Rds.HGet("fired", "last");
            string now = DateTime.Now.ToShortDateString();
            if (last == now)
                return;
            if (DateTime.Now.Hour < 5)
                return;
            Rds.HSet("fired", "last", now);
            Like(1980853671, "839827911", 50);
            Like(2125742312, "839827911", 50);
            Like(3524849039, "839827911", 50);
            Like(2568291736, "839827911", 50);
            Like(2568393973, "839827911", 50);
            Like(2563289279, "839827911", 50);
            if (test)
            {
                C.WriteLn(last);
                C.WriteLn(now);
                return;
            }
            //EnFire(839827911);
            //EnLike(839827911);
        }

        public static void EnFire(long w)
        {
            HashSet<string> s = Rds.client.GetAllItemsFromSet($"fire{w}");
            foreach (string i in s)
            {
                Sf(w, i, Strs.Get("fire"));
                C.WriteLn($"{i} <- fire", ConsoleColor.Cyan);
                Thread.Sleep(1000);
                //(int o, int r) = Like(w, i, 20);
                //C.WriteLn($"{i} <- {o}, {r}", ConsoleColor.Cyan);
                //Thread.Sleep(5000);
            }
        }

        public static void EnLike(long w)
        {
            HashSet<string> s = Rds.client.GetAllItemsFromSet($"like{w}");
            foreach (string i in s)
            {
                (int o, int r) = Like(w, i, 20);
                C.WriteLn($"{i} <- {o}, {r}", ConsoleColor.Cyan);
                if (r != 0)
                {
                    C.WriteLn("err", ConsoleColor.Red);
                }
                Thread.Sleep(10000);
            }
            C.WriteLn($"done");
        }

        private static (int, int) Like(long w, string i, int c)
        {
            int o = 0;
            int r;
            while (true)
            {
                r = Like2(w, i);
                if (r != 0)
                    break;
                o++;
                if (o >= c)
                    break;
            }
            return (o, r);
        }

        private static void Sf(long from, string to, string msg) => QY_sendFriendMsg(Robot.AuthCode, from, Convert.ToInt64(to), msg);
        [DllImport("QYOffer.dll")]
        private static extern int QY_sendFriendMsg(int authCode, long qqID, long target, string msg);
        private static int Like2(long from, string to) => QY_sendLikeFavorite(Robot.AuthCode, from, Convert.ToInt64(to));
        [DllImport("QYOffer.dll")]
        private static extern int QY_sendLikeFavorite(int authCode, long qqID, long targ);

    }
}
