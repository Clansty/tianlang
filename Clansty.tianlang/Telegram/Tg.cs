using System;
using System.Linq;
using System.Text.RegularExpressions;
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
                                $"会话 ID: {e.Message.Chat.Id}\n" +
                                $"用户 ID: {e.Message.From.Id}");
                            return;
                        case "/start":
                            if (e.Message.From.Id != e.Message.Chat.Id) return;
                            if (split.Length == 2)
                            {
                                var param = split[1];
                                var uuidRegex =
                                    new Regex(@"[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}");
                                var file = Db.ldb.Get("file" + param).Split(':');
                                if (uuidRegex.IsMatch(param))
                                {
                                    string link;
                                    if (file.Length == 4)
                                        link = await C.QQ.GetFileUrl(file[0], file[1], "/" + param, file[3]);
                                    else
                                        link = await C.QQ.GetFileUrl(file[0], file[1], file[3], file[4]);
                                    C.TG.SendTextMessageAsync(e.Message.Chat,
                                        $"文件: {file[3]}\n" +
                                        $"大小: {file[2]}",
                                        replyMarkup: new InlineKeyboardMarkup(
                                            new InlineKeyboardButton
                                            {
                                                Text = "下载",
                                                Url = link
                                            }));
                                    return;
                                }
                            }

                            C.TG.SendTextMessageAsync(e.Message.Chat,
                                "请发送绑定码");
                            return;
                        case "bind":
                            TgBinding.Bind(e);
                            return;

                        #region fire
                        // case "/fire":
                        //     if (e.Message.Chat.Id != 351768429) return;
                        //     if (split.Length == 1)
                        //         C.TG.SendTextMessageAsync(e.Message.Chat, await FireList.getList());
                        //     else if (split.Length == 2)
                        //     {
                        //         var text = split[1];
                        //         FireList.Resume(text);
                        //         C.TG.SendTextMessageAsync(e.Message.Chat, "操作成功");
                        //     }
                        //     else if (split.Length == 3)
                        //     {
                        //         switch (split[1])
                        //         {
                        //             case "add":
                        //                 FireList.Add(long.Parse(split[2]));
                        //                 break;
                        //             case "rm":
                        //                 FireList.Remove(long.Parse(split[2]));
                        //                 break;
                        //             default:
                        //                 C.TG.SendTextMessageAsync(e.Message.Chat, "操作无效");
                        //                 return;
                        //         }
                        //
                        //         C.TG.SendTextMessageAsync(e.Message.Chat, "操作成功");
                        //     }
                        //     else
                        //     {
                        //         C.TG.SendTextMessageAsync(e.Message.Chat, "操作无效");
                        //     }
                        //
                        //     return;
                        #endregion
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
                    qtime = await C.QQ.SendGroupMsg(fwdinfo.gin,
                        sdr +
                        Utf.Encode(e.Message.Text),
                        fromqq: fwdinfo.uin);
                    Db.ldb.Put(qtime.ToString(), e.Message.MessageId.ToString());
                    //命令
                    if (e.Message.Chat.Id == G.TG.si)
                        Cmds.Enter(e.Message.Text.TrimStart('/'), 839827911, false);

                    if (e.Message.Chat.Id == G.TG.major)
                    {
                        if (e.Message.Text.StartsWith("sudo "))
                        {
                            var rows = Db.users.Select($"tg={e.Message.From.Id}");
                            var u = new User(rows[0]);
                            Cmds.Enter(e.Message.Text.GetRight("sudo "), u.Uin, true);
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
                var hash = await C.QQ.UploadGroupPic(targ, $"https://api.telegram.org/file/bot{Token}/{pf.FilePath}");
                var tos = sdr;
                if (!string.IsNullOrWhiteSpace(e.Message.Caption))
                {
                    tos += e.Message.Caption + "\n";
                }

                tos = Utf.Encode(tos);
                tos += hash;
                qtime = await C.QQ.SendGroupMsg(targ, tos, fromqq: fwdinfo.uin);
                Db.ldb.Put(qtime.ToString(), e.Message.MessageId.ToString());
            }
        }
    }
}