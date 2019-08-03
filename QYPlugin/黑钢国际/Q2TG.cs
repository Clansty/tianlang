using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Clansty.tianlang
{
    static class Q2TG
    {
        public static async Task IDE(GroupMsgArgs e)
        {
            Message r;
            string emsg = e.Msg;
            int reply = 0;
            if (emsg.StartsWith("[LQ:srcmsg,seq=")) // 是回复
            {
                string seq = emsg.Between("[LQ:srcmsg,seq=", ",");
                emsg = emsg.GetRight("]");
                reply = TG.GetReplyInfo("ide", seq);
            }
            if (emsg.StartsWith("[LQ:image,") && emsg.EndsWith("]")) // 是图片
            {
                string picUrl = emsg.LastBetween(",urls=", "]").Replace("&amp;", "&");
                WebClient web = new WebClient();
                byte[] pic = web.DownloadData(picUrl);
                Stream st = new MemoryStream(pic);
                r = await TG.bot.SendPhotoAsync(TG.iDE,
                    new Telegram.Bot.Types.InputFiles.InputOnlineFile(st),
                    $"<b>{e.Card}</b> On QQ: ", 
                    Telegram.Bot.Types.Enums.ParseMode.Html,
                    replyToMessageId: reply);
                st.Dispose();
            }
            else if (emsg.IndexOf("[LQ:image,") > -1) // 是图文
            {
                string imgstr = "[LQ:image," + emsg.Between("[LQ:image,", "]") + "]";
                string msgstr = emsg.Replace(imgstr, "");
                string picUrl = imgstr.LastBetween(",urls=", "]").Replace("&amp;", "&");
                WebClient web = new WebClient();
                byte[] pic = web.DownloadData(picUrl);
                Stream st = new MemoryStream(pic);
                r=await TG.bot.SendPhotoAsync(TG.iDE, 
                    new Telegram.Bot.Types.InputFiles.InputOnlineFile(st),
                    $"<b>{e.Card}</b> On QQ: \n{msgstr}", 
                    Telegram.Bot.Types.Enums.ParseMode.Html,
                    replyToMessageId: reply);
                st.Dispose();
                web.Dispose();
            }
            else // 是文字
            {
                r = await TG.bot.SendTextMessageAsync(TG.iDE, 
                    $"<b>{e.Card}</b> On QQ:\n" + emsg, 
                    Telegram.Bot.Types.Enums.ParseMode.Html,
                    replyToMessageId: reply);
            }
            TG.RecordReplyInfo("ide", r, e.SrcMsg.Between("[LQ:srcmsg,seq=", ","), e.SrcMsg);
        }

        // 记录 QQ 消息与转发到 tg 消息的回复数据
        // Q->Q, qseq->tid
        // T->T, tid->☆qsrc
        // Q->T, ☆qseq->tid
        // T->Q, tid->qsrc
        // 有个问题，咱自己发消息的时候不知道自己发出去消息的 seq 和 src，这块骨头有点硬

        public static async Task Test(GroupMsgArgs e)
        {
            Message r;
            string emsg = e.Msg;
            int reply = 0;
            if (emsg.StartsWith("[LQ:srcmsg,seq=")) // 是回复
            {
                string seq = emsg.Between("[LQ:srcmsg,seq=", ",");
                emsg = emsg.GetRight("]");
                reply = TG.GetReplyInfo("test", seq);
            }
            if (emsg.StartsWith("[LQ:image,") && emsg.EndsWith("]")) // 是图片
            {
                string picUrl = emsg.LastBetween(",urls=", "]").Replace("&amp;", "&");
                WebClient web = new WebClient();
                byte[] pic = web.DownloadData(picUrl);
                Stream st = new MemoryStream(pic);
                r = await TG.bot.SendPhotoAsync(TG.test,
                    new Telegram.Bot.Types.InputFiles.InputOnlineFile(st),
                    $"<b>{e.Card}</b> On QQ: ",
                    Telegram.Bot.Types.Enums.ParseMode.Html,
                    replyToMessageId: reply);
                st.Dispose();
            }
            else if (emsg.IndexOf("[LQ:image,") > -1) // 是图文
            {
                string imgstr = "[LQ:image," + emsg.Between("[LQ:image,", "]") + "]";
                string msgstr = emsg.Replace(imgstr, "");
                string picUrl = imgstr.LastBetween(",urls=", "]").Replace("&amp;", "&");
                WebClient web = new WebClient();
                byte[] pic = web.DownloadData(picUrl);
                Stream st = new MemoryStream(pic);
                r = await TG.bot.SendPhotoAsync(TG.test,
                    new Telegram.Bot.Types.InputFiles.InputOnlineFile(st),
                    $"<b>{e.Card}</b> On QQ: \n{msgstr}",
                    Telegram.Bot.Types.Enums.ParseMode.Html,
                    replyToMessageId: reply);
                st.Dispose();
                web.Dispose();
            }
            else // 是文字
            {
                r = await TG.bot.SendTextMessageAsync(TG.test,
                    $"<b>{e.Card}</b> On QQ:\n" + emsg,
                    Telegram.Bot.Types.Enums.ParseMode.Html,
                    replyToMessageId: reply);
            }
            TG.RecordReplyInfo("test", r, e.SrcMsg.Between("[LQ:srcmsg,seq=", ","), e.SrcMsg);
        }
    }
}
