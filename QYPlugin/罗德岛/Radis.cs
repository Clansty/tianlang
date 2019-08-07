using ServiceStack.Redis;
using System;

namespace Clansty.tianlang
{
    public static class Rds
    {
        public static IRedisClient GetClient() => pool.GetClient();
        public static PooledRedisClientManager pool = new PooledRedisClientManager(233, 10, "127.0.0.1:6379");
        public static void HSet(string key, string subkey, string value)
        {
            IRedisClient client = GetClient();
            client.SetEntryInHash(key, subkey, value);
            client.Dispose();
        }

        public static void SAdd(string key, string value)
        {
            IRedisClient client = GetClient();
            client.AddItemToSet(key, value);
            client.Dispose();
        }

        public static void LAdd(string key, string value)
        {
            IRedisClient client = GetClient();
            client.AddItemToList(key, value);
            client.Dispose();
        }

        public static bool SContains(string key, string value)
        {
            IRedisClient client = GetClient();
            bool r = client.SetContainsItem(key, value);
            client.Dispose();
            return r;
        }

        public static string HGet(string key, string subkey)
        {
            string r;
            IRedisClient client = GetClient();
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
