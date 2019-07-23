using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;

namespace Clansty.tianlang
{
    static class Netease
    {
        public static void Start(GroupMsgArgs e)
        {
            string name = e.Msg.GetRight("点歌").Trim();
            if (name == "")
                e.Reply("网易云音乐点歌\n" +
                        "用法:\n" +
                        "点歌 <歌名>", true);
            else
            {
                Song s = GetSong(name);
                C.WriteLn(s.ToJson());
                e.Reply(s.ToJson(), false);
            }
        }


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

            public string ToJson()
            {
                if (success)
                {
                    object todo = new
                    {
                        app = "com.tencent.structmsg",
                        meta = new
                        {
                            music = new
                            {
                                desc = singer,
                                jumpUrl = Page,
                                musicUrl = url,
                                preview = pic,
                                tag = $"甜狼 {C.Version}",
                                title = name
                            }
                        },
                        prompt = $"[甜狼点歌]{name}",
                        ver = "0.0.0.1",
                        view = "music"
                    };
                    return $"[LQ:lightappelem,type=1,data={JsonConvert.SerializeObject(todo).Replace(",", "&#44;")},msg_resid=]";
                }
                else
                    return "出现了一点错误";
            }


            public string ToXml()
            {
                if (success)
                    return "[LQ:richmsg,type=1,template_1=" +
                           "<msg serviceID=\"2\" templateID=\"1\" action=\"web\" " +
                               $"brief=\"[甜狼点歌] {name}\" sourceMsgId=\"0\" " +
                               $"url=\"{Page}\" flag=\"0\" adverSign=\"0\" multiMsgFlag=\"0\">" +
                             "<item layout=\"2\">" +
                              $"<audio cover=\"{pic}\" " +
                                     $"src=\"{url}\" />" +
                              $"<title>{name}</title>" +
                              $"<summary>{singer}</summary>" +
                               "</item>" +
                            $"<source name=\"甜狼 Ver.{C.Version}\" icon=\"\" url=\"\" action=\"app\" a_actionData=\"com.netease.cloudmusic\" i_actionData=\"tencent100495085://\" appid=\"100495085\" />" +
                           "</msg>" +
                           ",service_id=2,msg_resid=,rand=0,seq=0,flags=0]";
                else
                    return "出现了一点错误";
            }
        }

        //public static string GetTheRedirectUrl(string originalAddress)
        //{
        //    string redirectUrl;
        //    WebRequest myRequest = WebRequest.Create(originalAddress);

        //    WebResponse myResponse = myRequest.GetResponse();
        //    redirectUrl = myResponse.ResponseUri.ToString();

        //    myResponse.Close();
        //    return redirectUrl;
        //}
        public static string GetWebText(string url)
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream datastream = response.GetResponseStream();
            StreamReader reader = new StreamReader(datastream, Encoding.UTF8);
            string result = reader.ReadToEnd();
            reader.Close();
            datastream.Close();
            response.Close();
            return result;
        }
        private static Song GetSong(string name)
        {
            Song s = new Song();
            try
            {
                string r = GetWebText($"https://v1.itooi.cn/netease/search?keyword={name}&type=song&pageSize=1");
                s.id = r.Between("\"id\":", ",");
                s.name = r.LastBetween("\"name\":\"", "\"");
                s.pic = $"https://v1.itooi.cn/netease/pic?id={s.id}";
                s.singer = r.Between("\"ar\":[{\"name\":\"", "\"");
                s.url = $"https://v1.itooi.cn/netease/url?id={s.id}&quality=flac";
                s.success = true;
                return s;
            }
            catch
            {
                return s;
            }
        }
        private static string GetXml(string name) => GetSong(name).ToXml();
    }
}
