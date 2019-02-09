using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace tianlang
{
    public static class CMS
    {
        public static void FileEnter(string qq, string jsonFile)
        {
            int uid = C.GetUid(qq);
            SqlDataReader r = Db.QueryReader($"SELECT cid FROM club_member WHERE uid={uid}");
            if (!r.Read())
            {
                r.Close();
                return;
            }
            int cid = r.GetInt32(0);
            r.Close();
            if (cid != 1024)
                return;
            FileRecv f = JToken.Parse(jsonFile).ToObject<FileRecv>();
            WebRequest request = WebRequest.Create(f.filelink);
            WebResponse response = request.GetResponse();
            Stream datastream = response.GetResponseStream();
            StreamReader reader = new StreamReader(datastream, Encoding.UTF8);
            string md = reader.ReadToEnd();
            reader.Close();
            datastream.Close();
            response.Close();
            DateTime date = DateTime.Now;
            string head = $"title: {f.filename.GetLeft(".md")}\n";
            head += $"date: {date.Year}-{date.Month}-{date.Day} {date.Hour}:{date.Minute}:{date.Second}\n";
            head += $"author: {uid}\n";
            head += $"uid: {uid}\n";
            head += "---\n";
            head += md;
            string fn = $"{f.filename.GetLeft(".md")}{date.ToBinary()}.md";
            File.WriteAllText("C:\\Users\\Administrator\\Documents\\web\\1024\\source\\_posts\\" + fn, head, Encoding.UTF8);
            S.P(qq, "上传成功，部署中");
            Process pd = new Process();
            pd.StartInfo.FileName = "cmd.exe";
            pd.StartInfo.Arguments = "/c hexo g";
            pd.StartInfo.UseShellExecute = false;
            pd.StartInfo.WorkingDirectory = "C:\\Users\\Administrator\\Documents\\web\\1024";
            pd.Start();
            pd.WaitForExit();
            S.P(qq, "部署完成，成不成功不知道");
        }
    }
}
