using System;
using System.Linq;
using System.Text.RegularExpressions;
using Mirai_CSharp.Extensions;
using Mirai_CSharp.Models;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace Clansty.tianlang
{
    internal static class Tg
    {
        internal const string Token = "712657902:AAG4eachKPVtfZeLpeR-8lAbDVy_6AMwASg";

        internal static async void OnMsg(object sender, MessageEventArgs e)
        {
            //响应大群新成员
            if (e.Message.NewChatMembers != null)
            {
                if (e.Message.Chat.Id == G.TG.major)
                {
                    foreach (var usr in e.Message.NewChatMembers)
                    {
                        var rows = Db.users.Select($"tg={usr.Id}");
                        if (rows.Length != 1)
                        {
                            //大群必须验证后加入
                            C.TG.KickChatMemberAsync(G.TG.major, usr.Id);
                        }
                    }
                }
            }

            //响应命令
            if (e.Message.Text != null)
            {
                try
                {
                    var split = e.Message.Text.Split(' ');
                    var action = split[0];
                    switch (action)
                    {
                        case "/id":
                            C.TG.SendTextMessageAsync(e.Message.Chat,
                                $"ChatID: {e.Message.Chat.Id}\n" +
                                $"UserID: {e.Message.From.Id}");
                            return;
                        case "/pin":
                            C.TG.PinChatMessageAsync(e.Message.Chat, e.Message.MessageId);
                            return;
                        case "/start":
                            C.TG.SendTextMessageAsync(e.Message.Chat,
                                "请发送绑定码");
                            return;
                        case "bind":
                            TgBinding.Bind(e);
                            return;
                    }
                }
                catch (Exception exception)
                {
                    C.TG.SendTextMessageAsync(e.Message.Chat, exception.ToString());
                }
            }

            var fwdinfo = Q2TgMap.Tg2Q(e.Message.Chat.Id);
            //规则转发
            if (fwdinfo != null)
            {
                long qtime;
                var sdr = e.Message.From.FirstName;
                if (e.Message.Chat.Id == G.TG.major)
                {
                    //特殊处理发信人
                    var usrs = Db.users.Select($"tg={e.Message.From.Id}");
                    if (usrs.Length != 1)
                    {
                        C.TG.KickChatMemberAsync(G.TG.major, e.Message.From.Id);
                        return;
                    }

                    var u = new User(usrs[0]);
                    sdr = u.ProperNamecard;
                }

                if (!fwdinfo.includeSender)
                    sdr = "";

                if (e.Message.ReplyToMessage != null)
                {
                    var rmsg = e.Message.ReplyToMessage;
                    if (rmsg.From.Id == C.tguid)
                    {
                        if (rmsg.Text != null)
                            sdr = $"「{rmsg.Text}」\n------\n{sdr}";
                        else if (rmsg.Caption != null)
                            sdr = $"「{rmsg.Caption}\n[图片]」\n------\n{sdr}";
                    }
                    else
                    {
                        if (rmsg.Text != null)
                            sdr = $"「{rmsg.From.FirstName}:\n{rmsg.Text}」\n------\n{sdr}";
                        else if (rmsg.Caption != null)
                            sdr = $"「{rmsg.From.FirstName}:\n{rmsg.Caption}\n[图片]」\n------\n{sdr}";
                        else if (rmsg.Sticker != null)
                            sdr = $"「{rmsg.From.FirstName}:\n[表情]」\n------\n{sdr}";
                        else if (rmsg.Photo != null)
                            sdr = $"「{rmsg.From.FirstName}:\n[图片]」\n------\n{sdr}";
                    }
                }

                if (fwdinfo.includeSender)
                    sdr += ":\n";

                if (e.Message.Text != null)
                {
                    //文本转发 tg2q
                    qtime = await fwdinfo.host.SendGroupMessageAsync(fwdinfo.gin,
                        new PlainMessage(sdr + e.Message.Text));
                    Db.ldb.Put(qtime.ToString(), e.Message.MessageId.ToString());
                    //命令
                    if (e.Message.Chat.Id == G.TG.si)
                    {
                        var rows = Db.users.Select($"tg={e.Message.From.Id}");
                        var u = new User(rows[0]);
                        Cmds.Enter(e.Message.Text.TrimStart('/'), u, false);
                    }

                    if (e.Message.Chat.Id == G.TG.major)
                    {
                        if (e.Message.Text.StartsWith("sudo "))
                        {
                            var rows = Db.users.Select($"tg={e.Message.From.Id}");
                            var u = new User(rows[0]);
                            Cmds.Enter(e.Message.Text.GetRight("sudo "), u, true);
                        }
                    }
                }

                //图片转发
                string ps = null;
                if (e.Message.Photo != null)
                {
                    ps = e.Message.Photo.Last().FileId;
                }

                if (e.Message.Sticker != null)
                {
                    ps = e.Message.Sticker.FileId;
                }

                if (ps is null) return;
                var targ = fwdinfo.gin;
                var pf = await C.TG.GetFileAsync(ps);
                var tos = new MessageBuilder();
                tos.AddPlainMessage(sdr);
                if (!string.IsNullOrWhiteSpace(e.Message.Caption))
                {
                    tos .AddPlainMessage(e.Message.Caption + "\n");
                }

                tos.AddImageMessage(url: $"https://api.telegram.org/file/bot{Token}/{pf.FilePath}");
                qtime = await fwdinfo.host.SendGroupMessageAsync(targ, tos);
                Db.ldb.Put(qtime.ToString(), e.Message.MessageId.ToString());
            }
        }
    }
}