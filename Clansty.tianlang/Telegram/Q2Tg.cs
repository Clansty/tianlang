﻿using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CornSDK;
using Telegram.Bot.Types;

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

        public static async Task NewGroupMsg(GroupMsgArgs e)
        {
            if (!G.Map.ContainsKey(e.FromGroup)) return;
            var msg = e.Msg.Trim();
            var from = Utf.Decode(e.FromCard);

            var reply = new Regex(@"\[Reply,.+,SendTime=(\d+).*\]");

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
            Message message;
            if (pic.IsMatch(msg))
            {
                var hash = pic.Match(msg).Groups[0].Value;
                var purl = C.QQ.GetPicUrl(hash, e.FromGroup);
                msg = pic.Replace(msg, "");
                msg = msg.Trim(' ', '\r', '\n', '\t');
                if (!string.IsNullOrEmpty(msg))
                    msg = "\n" + Utf.Decode(msg);
                message = await C.TG.SendPhotoAsync(G.Map[e.FromGroup],
                    purl.Result,
                    from + ":" + msg);
            }
            else
            {
                message = await C.TG.SendTextMessageAsync(G.Map[e.FromGroup],
                    from + ":\n" + Utf.Decode(msg));
            }

            var msgid = message.MessageId;
            C.WriteLn($"{e.Time}->{msgid}");
            Db.qtime2tgmsgid.Put(e.Time.ToString(), msgid.ToString());
        }
    }
}