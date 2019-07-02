using ServiceStack.Redis;
using System;

namespace Clansty.tianlang
{
    static class Rds
    {
        public static RedisClient client = new RedisClient("127.0.0.1", 6379);
        public static bool Set<T>(string key, T value) => client.Set(key, value);
        public static bool HSet(string key, string subkey, string value) => client.SetEntryInHash(key, subkey, value);
        public static void SAdd(string key, string value) => client.AddItemToSet(key, value);
        public static void LAdd(string key, string value) => client.AddItemToList(key, value);
        public static bool SContains(string key, string value) => client.SetContainsItem(key, value);

        public static string HGet(string key, string subkey)
        {
            try
            {
                return client.GetValueFromHash(key, subkey) ?? "";
            }
            catch (Exception e)
            {
                C.WriteLn(e.Message, ConsoleColor.Red);
                return "";
            }
        }
    }
}
