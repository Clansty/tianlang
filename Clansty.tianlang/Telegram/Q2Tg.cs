using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CornSDK;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace Clansty.tianlang
{
    static class Q2Tg
    {
        static readonly Dictionary<string, string> botCodes = new Dictionary<string, string>()
        {
            ["[file"] = "【文件】",
            ["[redpack"] = "【红包】",
            ["{\"app\":"] = "【卡片消息】",
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

            msg = msg.Trim();
            var atRegex = new Regex(@"\[@(\w+)\]");
            foreach (Match match in atRegex.Matches(msg))
            {
                var strAtqq = match.Groups[1].Value;
                var isLong = long.TryParse(strAtqq, out var atqq);
                if (isLong)
                {
                    var card = await C.QQ.GetGroupCard(e.FromGroup, atqq, e.RecvQQ);
                    if (card == "")
                    {
                        card = await C.QQ.GetNick(atqq, true, e.RecvQQ);
                    }

                    msg = msg.Replace(match.Value, "@" + card);
                }
            }

            var biliRegex =
                new Regex(
                    @"{.*""desc"":""哔哩哔哩"".*""prompt"":""\[QQ小程序\]哔哩哔哩"".*""qqdocurl"":""(https:\\/\\/b23.tv\\/.*\?).*"".*}");
            if (biliRegex.IsMatch(msg))
            {
                msg = biliRegex.Match(msg).Groups[1].Value;
                msg = msg.Replace("\\", "");
            }

            var jsonLinkRegex =
                new Regex(@"{.*""app"":""com.tencent.structmsg"".*""jumpUrl"":""(https?:\\/\\/[^"",]*)"".*}");
            if (jsonLinkRegex.IsMatch(msg))
            {
                msg = jsonLinkRegex.Match(msg).Groups[1].Value;
                msg = msg.Replace("\\/", "/");
            }
            
            foreach (var i in botCodes)
            {
                //Special bot codes that can't be proceeded by above code will be replaced
                if (msg.StartsWith(i.Key))
                {
                    msg = i.Value;
                    break;
                }
            }

            var picRegex = new Regex(@"\[pic,hash=\w+\]");
            var audioRegex = new Regex(@"\[Audio,.+,url=(.+),.*\]");
            var videoRegex=new Regex(@"\[litleVideo,linkParam=(\w*),hash1=(\w*).*]");
            Message message;
            if (picRegex.IsMatch(msg) || msg.StartsWith("[Graffiti") || msg.StartsWith("[picShow") || msg.StartsWith("[bigFace") || msg.StartsWith("[flashPic"))
            {
                //        ✓                                ✗                              ？                             ✗                             ✓
                // photo
                string hash;
                if (picRegex.IsMatch(msg))
                {
                    hash = picRegex.Match(msg).Value;
                    msg = picRegex.Replace(msg, "");
                }
                else
                {
                    hash = msg;
                    msg = "";
                }

                var purl = C.QQ.GetPicUrl(hash, e.FromGroup);
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
                //voice
                var url = audioRegex.Match(msg).Groups[1].Value;
                var path = "/root/silk/" + DateTime.Now.ToBinary();
                new WebClient().DownloadFile(url, path);
                var oggpath = Silk.decode(path);
                message = await C.TG.SendVoiceAsync(fwdinfo.tg, File.OpenRead(oggpath), from + ":",
                    replyToMessageId: replyId);
            }
            else if (videoRegex.IsMatch(msg))
            {
                //video
                var match = videoRegex.Match(msg);
                var param = match.Groups[1].Value;
                var hash1 = match.Groups[2].Value;
                var url = await C.QQ.GetVideoUrl(e.RecvQQ, e.FromGroup, e.FromQQ, param, hash1);
                message = await C.TG.SendTextMessageAsync(fwdinfo.tg,
                    from + ":\n" + url,
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