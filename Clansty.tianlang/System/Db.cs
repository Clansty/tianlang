﻿using System;
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
        internal static void Init()
        {
            #if DEBUG
            var jsonUsers = File.ReadAllText(@"C:\Users\clans\Desktop\users", Encoding.UTF8);
            var jsonPersons = File.ReadAllText(@"C:\Users\clans\Desktop\persons", Encoding.UTF8);
            #else
            var jsonUsers = File.ReadAllText("/root/data/users", Encoding.UTF8);
            var jsonPersons = File.ReadAllText("/root/data/persons", Encoding.UTF8);
            #endif
            users = JsonConvert.DeserializeObject<DataTable>(jsonUsers);
            persons = JsonConvert.DeserializeObject<DataTable>(jsonPersons);
            users.PrimaryKey = new[] { users.Columns[0] };
            persons.PrimaryKey = new[] { persons.Columns[0] };
#if !DEBUG
            var options = new Options { CreateIfMissing = true };
            ldb = new DB(options, "/root/ldb/qtime2tgmsgid");
#endif
        }
        internal static void Commit()
        {
            var jsonUsers = JsonConvert.SerializeObject(users);
            var jsonPersons = JsonConvert.SerializeObject(persons);
            File.WriteAllText("/root/data/users", jsonUsers);
            File.WriteAllText("/root/data/persons", jsonPersons);
            var dt = DateTime.Now.ToString("MM.dd.yyyy.HH.mm.ss");
            File.WriteAllText($"/root/data/bak/{dt}.users", jsonUsers);
            File.WriteAllText($"/root/data/bak/{dt}.persons", jsonPersons);
        }
    }
}
