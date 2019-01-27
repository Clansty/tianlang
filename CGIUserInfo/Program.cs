using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace CGIUserInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Write("Content-Type: text/html;charset=utf-8;\n\n");
            try
            {

                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = "server=.;database=tianlang;integrated security=SSPI";
                conn.Open();
                string queryStr = Environment.GetEnvironmentVariable("QUERY_STRING").Trim();
                int uid = Convert.ToInt32(queryStr);
                Console.WriteLine($"<meta charset=\"utf-8\" /><title>uid {uid} 的信息</title>");
                if (queryStr != null && queryStr != "")
                {
                    string command = $"SELECT * FROM user_info WHERE uid={uid}";
                    SqlDataAdapter sda = new SqlDataAdapter(command, conn);
                    DataSet data = new DataSet();
                    sda.Fill(data);

                    //复制自 User.cs
                    string QQ = "";
                    string Name = "";
                    int Class = 0;
                    bool Branch = false;
                    string Nick = "";
                    bool Junior = false;
                    string Grade = "";
                    int Enrollment = 0;
                    

                    QQ = data.Tables[0].Rows[0]["QQ"].ToString();
                    Name = data.Tables[0].Rows[0]["name"] == DBNull.Value ? "" : data.Tables[0].Rows[0]["name"].ToString();
                    Class = data.Tables[0].Rows[0]["class"] == DBNull.Value ? 0 : (int)data.Tables[0].Rows[0]["class"];
                    Branch = data.Tables[0].Rows[0]["branch"] == DBNull.Value ? false : (bool)data.Tables[0].Rows[0]["branch"];
                    Nick = data.Tables[0].Rows[0]["nick"] == DBNull.Value ? "" : data.Tables[0].Rows[0]["nick"].ToString();
                    Junior = data.Tables[0].Rows[0]["junior"] == DBNull.Value ? false : (bool)data.Tables[0].Rows[0]["junior"];
                    Enrollment = data.Tables[0].Rows[0]["enrollment"] == DBNull.Value ? 0 : (int)data.Tables[0].Rows[0]["enrollment"];


                    if (Junior)
                        switch (Enrollment)
                        {
                            case 2018:
                                Grade = "初一";
                                break;
                            case 2017:
                                Grade = "初二";
                                break;
                            case 2016:
                                Grade = "初三";
                                break;
                        }
                    else
                        switch (Enrollment)
                        {
                            case 2018:
                                Grade = "高一";
                                break;
                            case 2017:
                                Grade = "高二";
                                break;
                            case 2016:
                                Grade = "高三";
                                break;
                        }
                    if (Enrollment < 2016)
                        Grade = (Enrollment + 3).ToString() + "届" + (Junior ? "初中" : "");

                    string result = $"uid: {uid}<br>" +
                                    $"qq: {QQ}<br>" +
                                    $"name: {Name}<br>" +
                                    $"class: {Class}<br>" +
                                    $"branch: {(Branch ? "金阊" : "本部")}<br>" +
                                    $"nick: {Nick}<br>" +
                                    $"junior: {Junior}<br>" +
                                    $"enrollment: {Enrollment}<br>" +
                                    $"grade: {Grade}<br>";

                    Console.WriteLine(result);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"<strong>错误信息: {e.Message}</strong>");
            }
        }
    }
}
