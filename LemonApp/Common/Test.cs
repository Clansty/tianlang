using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Clansty.tianlang
{
    public static class Test
    {
        public static void Do()
        {
            const string connStr = "server = cdb-pi7fvpvu.cd.tencentcdb.com; user = root; database = tianlang; port = 10058; password = t00rrooT";
            const string sqlPersons = "SELECT * FROM persons";
            var daPersons = new MySqlDataAdapter(sqlPersons, connStr);
            new MySqlCommandBuilder(daPersons);
            var persons = new DataTable();
            daPersons.Fill(persons);
            var juns = new string[] { "68861", "68862", "68863" };
            var brs = new string[] { "72293", "72294", "72295", "72296", "72838", "72839", "72535", "72536", "72537", "72538" };
            var aj = File.ReadAllText(@"C:\Users\clans\Desktop\all.json", Encoding.UTF8);
            var ajo = JObject.Parse(aj);
            foreach (var d in ajo["data"])
            {
                foreach (var c in d["classes"])
                {
                    var cid = c.Value<string>("class_id");
                    var cj = File.ReadAllText(@"C:\Users\clans\Desktop\classes\" + cid + ".json", Encoding.UTF8);
                    var cjo = JObject.Parse(cj);
                    foreach (var s in cjo["student_info"])
                    {
                        var i = s.Value<int>("student_id");
                        var sj = File.ReadAllText(@"C:\Users\clans\Desktop\ss\" + i + ".json", Encoding.UTF8);
                        var sjo = JObject.Parse(sj);
                        var name = sjo["data"].Value<string>("truename");
                        var junior = juns.Contains(cid);
                        var branch = brs.Contains(cid);
                        var board = sjo["data"].Value<string>("board") == "1";
                        var gender = ulong.Parse(sjo["data"].Value<string>("sex"));
                        var enrollment = d.Value<int>("enr");
                        var _class = int.Parse(c.Value<string>("class_title"));
                        persons.Rows.Add(null, name, junior, branch, board, gender, _class, enrollment);
                        C.WriteLn(persons.Rows.Count);
                    }
                }                
            }
            daPersons.Update(persons);
            while (true)
                Console.ReadLine();
        }

    }
}
