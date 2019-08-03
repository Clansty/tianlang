﻿using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Clansty.tianlang
{
    public static class Fired
    {
        public static async Task CronAsync(bool test = false)
        {
            string last = Rds.HGet("fired", "last");
            string now = DateTime.Now.ToShortDateString();
            if (last == now)
                return;
            if (DateTime.Now.Hour < 5)
                return;
            Rds.HSet("fired", "last", now);
            await Task.Run(EnLikeNew);
        }

        private static void EnLikeNew()
        {
            IRedisClient client = Rds.GetClient();
            HashSet<string> likee = client.GetAllItemsFromSet("likee");
            HashSet<string> liker = client.GetAllItemsFromSet("liker");
            client.Dispose();
            foreach (string ee in likee)
            {
                int tot = 0;
                foreach (string er in liker)
                {
                    (int o, int r) = Like(er, ee, 50);
                    tot += o;
                    if (r != 0)
                        C.WriteLn($"like err {r}: {er} => {ee}", ConsoleColor.Red);
                }
                C.WriteLn($"like: {ee} <= {tot}");
            }
            C.WriteLn("like done");
        }


        public static void EnFire(long w)
        {
            IRedisClient client = Rds.GetClient();
            HashSet<string> s = client.GetAllItemsFromSet($"fire{w}");
            client.Dispose();
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
            IRedisClient client = Rds.GetClient();
            HashSet<string> s = client.GetAllItemsFromSet($"like{w}");
            client.Dispose();
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

        private static (int, int) Like(string w, string i, int c) => Like(long.Parse(w), i, c);

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
