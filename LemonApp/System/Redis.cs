﻿using System;
using ServiceStack.Redis;

namespace Clansty.tianlang
{
    public static class Rds
    {
        public static IRedisClient GetClient() => pool.GetClient();
#if DEBUG
        public static PooledRedisClientManager pool = new PooledRedisClientManager(233, 10, "qVAo9C1tCbD2PEiR@101.132.178.136:6379");
#else
        public static PooledRedisClientManager pool = new PooledRedisClientManager(233, 10, "qVAo9C1tCbD2PEiR@127.0.0.1:6379");
#endif
        public static void HSet(string key, string subkey, string value)
        {
            var client = GetClient();
            client.SetEntryInHash(key, subkey, value);
            client.Dispose();
        }

        public static void SAdd(string key, string value)
        {
            var client = GetClient();
            client.AddItemToSet(key, value);
            client.Dispose();
        }

        public static void LAdd(string key, string value)
        {
            var client = GetClient();
            client.AddItemToList(key, value);
            client.Dispose();
        }

        public static bool SContains(string key, string value)
        {
            var client = GetClient();
            var r = client.SetContainsItem(key, value);
            client.Dispose();
            return r;
        }

        public static string HGet(string key, string subkey)
        {
            string r;
            var client = GetClient();
            try
            {
                r = client.GetValueFromHash(key, subkey) ?? "";
            }
            catch (Exception e)
            {
                C.WriteLn(e.Message, ConsoleColor.Red);
                r = "";
            }
            client.Dispose();
            return r;
        }
    }
}