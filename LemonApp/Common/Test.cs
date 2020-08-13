using Newtonsoft.Json.Linq;
using System;
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
                if (!key.StartsWith("u"))
                    continue;
                var v = i["value"];
                var name = v.Value<string>("name");
                var nick = v.Value<string>("nick");
                var junior = v.Value<string>("junior");
                var enrollment = v.Value<string>("enrollment");
                var role = v.Value<string>("role");
                if (name == "" &&
                    nick == "" &&
                    junior == "0" &&
                    enrollment == "-1" &&
                    role == "0")
                    continue;
                var q = key.GetRight("u");
                if (q.Length < 5)
                    continue;
                if (!long.TryParse(q, out var lq))
                    continue;
                var u = new User(lq);
                u.Name = name;
                u.Nick = nick;
                u.Junior = junior == "1";
                u.Enrollment = int.Parse(enrollment);
                u.Role = (UserType)int.Parse(role);
                C.WriteLn(lq);
            }
            Db.Commit();
            while (true)
                Console.ReadLine();
        }

    }
}
