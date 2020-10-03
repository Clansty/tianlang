using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Clansty.tianlang
{
    static class Db
    {
#if DEBUG
        const string connStr = "server = cdb-pi7fvpvu.cd.tencentcdb.com; user = root; database = tianlang_dev; port = 10058; password = gvm63Vbq9rT9uH29";
#else
        const string connStr = "server = cdb-pi7fvpvu.cd.tencentcdb.com; user = root; database = tianlang; port = 10058; password = gvm63Vbq9rT9uH29";
#endif
        internal static DataTable users = new DataTable();
        internal static DataTable persons = new DataTable();
        private static MySqlDataAdapter daUsers = null;
        internal static void Init()
        {
            const string sqlUsers = "SELECT * FROM users";
            const string sqlPersons = "SELECT * FROM persons";
            daUsers = new MySqlDataAdapter(sqlUsers, connStr);
            var daPersons = new MySqlDataAdapter(sqlPersons, connStr);
            new MySqlCommandBuilder(daUsers);
            daUsers.FillAsync(users);
            daPersons.FillAsync(persons);
            users.PrimaryKey = new DataColumn[] { users.Columns[0] };
            persons.PrimaryKey = new DataColumn[] { persons.Columns[0] };
        }
        internal static void Commit()
        {
            //try
            //{
                daUsers.Update(users);
                C.WriteLn("数据库同步成功");
            //}
            //catch (Exception ex)
            //{
            //    C.WriteLn(ex.ToString(), ConsoleColor.Red);                
            //}
        }
    }
}
