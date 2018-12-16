using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tianlang
{
    public static class Si
    {
        private static void Reply(string msg)
        {
            if (C.isTest)
                S.Test(msg);
            else
                S.Si(msg);
        }
        public static void Enter(string command)
        {
            if (command.StartsWith("sql"))
            {
                command = command.Replace("sql", "").Trim();
                if (command == "")
                {
                    Reply("用法:\nsql <exec|query> <SQL 语句>");
                }
                Sql(command);
            }
        }
        private static void Sql(string command)
        {
            if (command.StartsWith("exec"))
            {
                command = command.Replace("exec", "").Trim();
                if (command == "")
                {
                    Reply("用法:\nsql exec <SQL 语句>\n回复受影响的行数或出现的错误");
                    return;
                }
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = Db.conn;
                cmd.CommandText = command;
                try
                {
                    int result = cmd.ExecuteNonQuery();
                    Reply($"执行成功\n{result.ToString()}行受到影响");
                }
                catch (Exception e)
                {
                    IRQQApi.Api_OutPutLog(e.Message);
                    Reply($"执行失败\n{e.Message}");
                }
            }
            else if (command.StartsWith("query"))
            {
                command = command.Replace("query", "").Trim();
                if (command == "")
                {
                    Reply("用法:\nsql query <SQL 语句>\n回复查询到的结果或出现的错误");
                    return;
                }
                try
                {
                    SqlDataReader r = Db.QueryReader(command);
                    string reply = "";
                    int i = 0;
                    while (r.Read())
                    {
                        reply = reply + "\n" + r[0].ToString();
                        i++;
                    }
                    r.Close();
                    reply = $"共找到 {i.ToString()} 条记录" + reply;
                    Reply(reply);
                }
                catch (Exception e)
                {
                    Reply($"查询失败\n{e.Message}");
                }
            }
        }
    }
}
