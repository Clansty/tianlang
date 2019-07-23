namespace Clansty.tianlang
{
    static class Strs
    {
        public static string Get(string key) => Rds.HGet("strs", key);
        public static string Get(string key, params string[] para) => string.Format(Get(key), para);
        public static void Set(string key, string value) => Rds.HSet("strs", key, value);
    }
}
