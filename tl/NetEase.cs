using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace tianlang
{
    public class NetEase
    {
        private class Song
        {
            public string id = "";
            public string name = "";
            public string singer = "";
            public string pic = "";
            public string url = "";
            public bool success = false;

            public string Page
            {
                get
                {
                    return $"http://music.163.com/song/{id}/";
                }
            }

            public override string ToString()
            {
                if (success)
                    return "<msg serviceID=\"2\" templateID=\"1\" action=\"web\" " +
                               $"brief=\"[甜狼点歌] {name}\" sourceMsgId=\"0\" " +
                               $"url=\"{Page}\" flag=\"0\" adverSign=\"0\" multiMsgFlag=\"0\">" +
                             "<item layout=\"2\">" +
                              $"<audio cover=\"{pic}\" " +
                                     $"src=\"{url}\" />" +
                              $"<title>{name}</title>" +
                              $"<summary>{singer}</summary>" +
                               "</item>" +
                            $"<source name=\"甜狼 Ver.{C.version}\" icon=\"\" url=\"\" action=\"app\" a_actionData=\"com.netease.cloudmusic\" i_actionData=\"tencent100495085://\" appid=\"100495085\" />" +
                           "</msg>";
                else
                    return "err";
            }
        }

        public static string GetTheRedirectUrl(string originalAddress)
        {
            string redirectUrl;
            WebRequest myRequest = WebRequest.Create(originalAddress);

            WebResponse myResponse = myRequest.GetResponse();
            redirectUrl = myResponse.ResponseUri.ToString();

            myResponse.Close();
            return redirectUrl;
        }

        private static Song GetSong(string name)
        {
            Song s = new Song();
            try
            {
                string url = $"https://api.bzqll.com/music/netease/search?key=579621905&s={name}&type=song&limit=1&offset=0";
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                Stream datastream = response.GetResponseStream();
                StreamReader reader = new StreamReader(datastream, Encoding.UTF8);
                string r = reader.ReadToEnd();
                reader.Close();
                datastream.Close();
                response.Close();
                // 复制的
                if (r.IndexOf("\"result\":\"SUCCESS\"") < 0)
                    return s;
                s.id = r.Between("\"id\":\"", "\"");
                s.name = r.Between("\"name\":\"", "\"");
                s.pic = r.Between("\"pic\":\"", "\"");
                s.singer = r.Between("\"singer\":\"", "\"");
                s.url = GetTheRedirectUrl(r.Between("\"url\":\"", "\"")).Replace("https:", "http:");
                s.success = true;
                return s;
            }
            catch
            {
                return s;
            }
        }
        private static string GetXml(string name) => GetSong(name).ToString();

        public static void Enter(string group, string name)
        {
            if (name != "")
            {
                string x = GetXml(name);
                if (x == "err")
                    S.Group(group, C.err);
                else
                    IRQQApi.Api_SendXML(C.W, 1, 2, group, group, x, 2);
            }
            else
                S.Group(group, "网易云音乐点歌\n" +
                               "用法:\n" +
                               "点歌 <歌名>");
        }
    }
}
