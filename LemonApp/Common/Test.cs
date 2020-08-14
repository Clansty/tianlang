using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Clansty.tianlang
{
    public static class Test
    {
        public static void Do()
        {
            Db.Init();
            var json = File.ReadAllText(@"C:\Users\clans\Desktop\dump.json", Encoding.UTF8);
            var jo = JToken.Parse(json);
            foreach (var i in jo)
            {
                var key = i.Value<string>("key");
                if (key == "rn2017" ||
                    key == "rn2018" ||
                    key == "rn2018jc" ||
                    key == "rn2019" ||
                    key == "rn2019jc")
                {
                    C.WriteLn(key);
                    var v = i["value"].ToObject<Dictionary<string, string>>();
                    foreach (var kvp in v)
                    {
                        if (kvp.Value == "0")
                            continue;
                        var u = new User(long.Parse(kvp.Value));
                        if (kvp.Key == "杨雨欣" ||
                            kvp.Key == "林逐水" ||
                            kvp.Key == "阮南烛" ||
                            kvp.Key == "张欣怡" ||
                            kvp.Key == "杨帆" ||
                            kvp.Key == "陈昊" ||
                            kvp.Key == "王宇轩" ||
                            kvp.Key == "徐越" ||
                            kvp.Key == "王睿葱" ||
                            kvp.Key == "王奕")
                            continue;
                        var p = Person.Get(kvp.Key);
                        u.Row["bind"] = p.Id;
                    }
                }
            }
            Db.Commit();
            while (true)
                Console.ReadLine();
        }

    }
}
