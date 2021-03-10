using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using LevelDB;
using Newtonsoft.Json;

namespace Clansty.tianlang
{
    static class Db
    {
#if DEBUG
        const string connStr = "server = cdb-pi7fvpvu.cd.tencentcdb.com; user = root; database = tianlang; port = 10058; password = gvm63Vbq9rT9uH29";
#else
        const string connStr = "server = cdb-pi7fvpvu.cd.tencentcdb.com; user = root; database = tianlang; port = 10058; password = gvm63Vbq9rT9uH29";
#endif
        internal static DataTable users;
        internal static DataTable persons;
        internal static DB ldb;
        internal static Dictionary<long, SeMember> SeMembers;
        internal static void Init(bool reload=false)
        {
            #if DEBUG
            var jsonUsers = File.ReadAllText(@"C:\Users\clans\Desktop\users", Encoding.UTF8);
            var jsonPersons = File.ReadAllText(@"C:\Users\clans\Desktop\persons", Encoding.UTF8);
            #else
            var jsonUsers = File.ReadAllText("/home/clansty/data/users", Encoding.UTF8);
            var jsonPersons = File.ReadAllText("/home/clansty/data/persons", Encoding.UTF8);
            #endif
            users = JsonConvert.DeserializeObject<DataTable>(jsonUsers);
            persons = JsonConvert.DeserializeObject<DataTable>(jsonPersons);
            users.PrimaryKey = new[] { users.Columns[0] };
            persons.PrimaryKey = new[] { persons.Columns[0] };
#if !DEBUG
            if (!reload)
            {
                var options = new Options { CreateIfMissing = true };
                ldb = new DB(options, "/home/clansty/ldb/qtime2tgmsgid");
            }
            SeMembers =
                JsonConvert.DeserializeObject<Dictionary<long, SeMember>>(File.ReadAllText("/home/clansty/data/rh.json"));
#endif
        }
        internal static void Commit()
        {
            var jsonUsers = JsonConvert.SerializeObject(users);
            File.WriteAllText("/home/clansty/data/users", jsonUsers);
            var dt = DateTime.Now.ToString("MM.dd.yyyy.HH.mm.ss");
            File.WriteAllText($"/home/clansty/data/bak/{dt}.users", jsonUsers);
        }
    }
}
