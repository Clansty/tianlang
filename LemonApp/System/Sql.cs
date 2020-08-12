﻿using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Clansty.tianlang
{
    public static class Sql
    {
        #region 我有点想在启动的时候把库里面东西全都取到内存，内存里操作，每隔一段时间再放回数据库
        //
        //        public static MySqlConnection conn = null;
        //        public static void Connect()
        //        {
        //#if DEBUG
        //            string connStr = "server = cdb-pi7fvpvu.cd.tencentcdb.com; user = root; database = tianlang_dev; port = 10058; password = t00rrooT";
        //#else
        //            string connStr = "server = cdb-pi7fvpvu.cd.tencentcdb.com; user = root; database = tianlang; port = 10058; password = t00rrooT";
        //#endif
        //            conn = new MySqlConnection(connStr);
        //            try 
        //            { 
        //                conn.Open();
        //            }
        //            catch (Exception ex) 
        //            { 
        //                Console.WriteLine(ex.ToString());
        //            }
        //        }
        //        public static MySqlCommand Cmd(string sql)
        //        {
        //            return new MySqlCommand(sql, conn);
        //        }
        //        public static MySqlDataReader Query(string sql)
        //        {
        //            var cmd = Cmd(sql);
        //            return cmd.ExecuteReader();
        //        }
        //        public static void Exec(string sql)
        //        {
        //            var cmd = Cmd(sql);
        //            cmd.ExecuteNonQuery();
        //        }
        #endregion
        //直接 v2 吧
#if DEBUG
        const string connStr = "server = cdb-pi7fvpvu.cd.tencentcdb.com; user = root; database = tianlang_dev; port = 10058; password = t00rrooT";
#else
        const string connStr = "server = cdb-pi7fvpvu.cd.tencentcdb.com; user = root; database = tianlang; port = 10058; password = t00rrooT";
#endif
        public static DataTable users = new DataTable();
        public static DataTable persons = new DataTable();
        private static MySqlDataAdapter daUsers = null;
        private static MySqlDataAdapter daPersons = null;
        public static void Init()
        {
            var sqlUsers = "SELECT * FROM users";
            var sqlPersons = "SELECT * FROM persons";
            daUsers = new MySqlDataAdapter(sqlUsers, connStr);
            daPersons = new MySqlDataAdapter(sqlPersons, connStr);
            new MySqlCommandBuilder(daUsers);
            new MySqlCommandBuilder(daPersons);
            daUsers.FillAsync(users);
            daPersons.FillAsync(persons);
            users.PrimaryKey = new DataColumn[] { users.Columns[0] };
            persons.PrimaryKey = new DataColumn[] { persons.Columns[0] };
        }
        public static void Commit()
        {
            try
            {
                daUsers.Update(users);
                daPersons.Update(persons);
            }
            catch (Exception ex)
            {
                C.WriteLn(ex.ToString(), ConsoleColor.Red);                
            }
        }
    }
}