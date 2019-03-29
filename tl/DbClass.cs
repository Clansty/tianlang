using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace tianlang
{
    public static class Db
    {
        public static SqlConnection conn = new SqlConnection();
        public static void Connect()
        {

            //if (C.isTest)
            //    conn.ConnectionString = "server=119.3.78.168,1433;database=tianlang;uid=sa;pwd=Ti@nlang2018";
            //else
                conn.ConnectionString = "server=.;database=tianlang;integrated security=SSPI";

            try
            {
                conn.Open();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                S.Test(e.Message);
            }
            if (IsConnected())
                Console.WriteLine("数据库连接成功");
            else
                Console.WriteLine("数据库连接失败");
        }
        public static void DisConnect()
        {
            conn.Close();
        }
        public static bool IsConnected()
        {
            if (conn.State == ConnectionState.Open)
                return true;
            else
                return false;
        }
        public static int Exec (string command)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;
            cmd.CommandText = command;
            try
            {
                int result = cmd.ExecuteNonQuery();
                return result;
            }
            catch(Exception e)
            {
                IRQQApi.Api_OutPutLog(e.Message);
                S.Test(e.Message);
                return -1;
            }
        }
        public static DataSet Query(string command)
        {
            SqlDataAdapter sda = new SqlDataAdapter(command, conn);
            DataSet dt = new DataSet();
            sda.Fill(dt);
            return dt;
        }
        public static SqlDataReader QueryReader(string command)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;
            cmd.CommandText = command;
            return cmd.ExecuteReader();
        }
    }
}
