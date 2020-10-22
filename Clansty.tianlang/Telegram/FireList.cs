using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Clansty.tianlang
{
    static class FireList
    {
        private static HashSet<long> list = null;

        public static void Init()
        {
            try
            {
                var strList = Db.ldb.Get("fireList");
                list = JsonConvert.DeserializeObject<HashSet<long>>(strList);
            }
            catch
            {
                list=new HashSet<long>();
                Db.ldb.Put("fireList", JsonConvert.SerializeObject(list));
            }
        }

        public static void Add(long uin)
        {
            list.Add(uin);
            Db.ldb.Put("fireList", JsonConvert.SerializeObject(list));
        }

        public static void Remove(long uin)
        {
            list.Remove(uin);
            Db.ldb.Put("fireList", JsonConvert.SerializeObject(list));
        }

        public static void Resume(string text = "早啊")
        {
            foreach (var i in list)
            {
                C.QQ.SendPrivateMsg(i, text, 839827911);
            }
        }

        public static async Task<string> getList()
        {
            var res = "";
            foreach (var i in list)
            {
                res += $"{await C.QQ.GetNick(i)}({i})\n";
            }

            res.TrimEnd('\n');
            return res;
        }
    }
}