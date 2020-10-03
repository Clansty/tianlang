using System.Text.RegularExpressions;
using CornSDK;

namespace Clansty.tianlang
{
    static class Q2Tg
    {
        static readonly string[] botCodes =
        {
            "[Audio",
            "[file",
            "[redpack",
            "{\"app\":",
            "[bigFace",
            "[Graffiti",
            "[picShow",
            "[litleVideo,"
        };

        public static void NewGroupMsg(GroupMsgArgs e)
        {
            if (!G.Map.ContainsKey(e.FromGroup)) return;
            var msg = e.Msg.Trim();
            var from = Utf.Decode(e.FromCard);

            if (msg.StartsWith("[Reply"))
                msg = msg.GetRight("]").Trim();
            if (msg.StartsWith("<?xml") || msg.StartsWith("链接<?xml"))
            {
                if (msg.Contains("url=\""))
                {
                    msg = msg.Between("url=\"", "\"");
                }
                else return;
            }

            if (msg.Contains("<?xml"))
                msg = msg.GetLeft("<?xml");

            foreach (var i in botCodes)
            {
                if (msg.StartsWith(i))
                    return;
            }

            var pic = new Regex(@"\[pic,hash=\w+\]");
            if (pic.IsMatch(msg))
            {
                var hash = pic.Match(msg).Groups[0].Value;
                var purl = C.QQ.GetPicUrl(hash, e.FromGroup);
                msg = pic.Replace(msg, "");
                msg = msg.Trim(' ', '\r', '\n', '\t');
                if (!string.IsNullOrEmpty(msg))
                    msg = "\n" + Utf.Decode(msg);
                C.TG.SendPhotoAsync(G.Map[e.FromGroup],
                    purl.Result,
                    from + ":" + msg);
            }
            else
            {
                C.TG.SendTextMessageAsync(G.Map[e.FromGroup],
                    from + ":\n" + Utf.Decode(msg));
            }
        }
    }
}