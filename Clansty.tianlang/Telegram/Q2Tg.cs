using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CornSDK;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Clansty.tianlang
{
    static class Q2Tg
    {
        static readonly Dictionary<string, string> botCodes = new Dictionary<string, string>()
        {
            // ["[Audio"] = "【语音】",
            ["[file"] = "【文件】",
            ["[redpack"] = "【红包】",
            ["{\"app\":"] = "【卡片消息】",
            ["[bigFace"] = "【大表情】",
            ["[Graffiti"] = "【涂鸦】",
            ["[picShow"] = "【秀图】",
            ["[litleVideo,"] = "【小视频】"
        };

        public static async Task NewGroupMsg(GroupMsgArgs e)
        {
            var fwdinfo = Q2TgMap.Q2Tg(e.RecvQQ, e.FromGroup);
            if (fwdinfo is null) return;

            var msg = e.Msg.Trim();
            var from = Utf.Decode(e.FromCard);

            var replyRegex = new Regex(@"\[Reply,.+,SendTime=(\d+).*\]");
            string replyIdStr = null;
            if (replyRegex.IsMatch(msg))
            {
                var match = replyRegex.Match(msg);
                var qtime = match.Groups[1].Value;
                replyIdStr = Db.ldb.Get(qtime);
                msg = replyRegex.Replace(msg, "");
            }

            var replyId = 0;
            if (!string.IsNullOrWhiteSpace(replyIdStr))
                replyId = int.Parse(replyIdStr);

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
                if (msg.StartsWith(i.Key))
                {
                    msg = i.Value;
                    break;
                }
            }

            msg = msg.Trim();
            var atRegex = new Regex(@"\[@(\w+)\]");
            if (atRegex.IsMatch(msg))
            {
                var match = atRegex.Match(msg);
                var strAtqq = match.Groups[1].Value;
                var isLong = long.TryParse(strAtqq, out var atqq);
                if (isLong)
                {
                    var card = await C.QQ.GetGroupCard(e.FromGroup, atqq, e.RecvQQ);
                    if (card == "")
                    {
                        card = await C.QQ.GetNick(atqq, true, e.RecvQQ);
                    }

                    msg = atRegex.Replace(msg, "@" + card);
                }
            }

            var picRegex = new Regex(@"\[pic,hash=\w+\]");
            var audioRegex = new Regex(@"\[Audio,.+,url=(.+),.*\]");
            Message message;
            if (picRegex.IsMatch(msg))
            {
                // photo
                var hash = picRegex.Match(msg).Groups[0].Value;
                var purl = C.QQ.GetPicUrl(hash, e.FromGroup);
                msg = picRegex.Replace(msg, "");
                msg = msg.Trim(' ', '\r', '\n', '\t');
                if (!string.IsNullOrEmpty(msg))
                    msg = "\n" + Utf.Decode(msg);
                message = await C.TG.SendPhotoAsync(fwdinfo.tg,
                    purl.Result,
                    from + ":" + msg,
                    replyToMessageId: replyId);
            }
            else if (audioRegex.IsMatch(msg))
            {
                var url = picRegex.Match(msg).Groups[0].Value;
                var path = "/root/silk" + DateTime.Now.ToBinary();
                new WebClient().DownloadFile(url, path);
                var oggpath = Silk.decode(path);
                message = await C.TG.SendVoiceAsync(fwdinfo.tg, oggpath, from + ":",
                    replyToMessageId: replyId);
            }
            else
            {
                // text
                message = await C.TG.SendTextMessageAsync(fwdinfo.tg,
                    from + ":\n" + Utf.Decode(msg),
                    replyToMessageId: replyId);
            }

            var msgid = message.MessageId;
            Db.ldb.Put(e.Time.ToString(), msgid.ToString());
        }
    }
}