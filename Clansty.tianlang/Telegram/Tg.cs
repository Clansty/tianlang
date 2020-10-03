using System.Globalization;
using System.Linq;
using Telegram.Bot.Args;

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
                var action = e.Message.Text.Split(' ')[0];
                switch (action)
                {
                    case "/id":
                        C.TG.SendTextMessageAsync(e.Message.Chat,
                            $"会话 ID: {e.Message.Chat.Id}\n" +
                            $"用户 ID: {e.Message.From.Id}");
                        return;
                    case "/start":
                        return;
                    case "/bind":
                        if (e.Message.From.Id != e.Message.Chat.Id) return;
                        action = e.Message.Text.Split(' ')[1];
                        if (action.Contains("!"))
                            action = action.GetLeft("!");
                        if (!long.TryParse(action, NumberStyles.HexNumber, null, out var bindCode))
                        {
                            C.TG.SendTextMessageAsync(e.Message.Chat,
                                "绑定码不正确");
                            return;
                        }

                        if (bindCode > -1)
                        {
                            C.TG.SendTextMessageAsync(e.Message.Chat,
                                "绑定码不正确");
                            return;
                        }

                        var rows = Db.users.Select($"tg={bindCode}");
                        if (rows.Length != 1)
                        {
                            C.TG.SendTextMessageAsync(e.Message.Chat,
                                "绑定码不正确");
                            return;
                        }

                        var u = new User(rows[0]);
                        u.TgUid = e.Message.From.Id;
                        C.TG.UnbanChatMemberAsync(G.TG.major, e.Message.From.Id);
                        S.Private(u, $"你已经与 Telegram 账号 {e.Message.From.FirstName} 绑定成功");
                        C.TG.SendTextMessageAsync(e.Message.Chat,
                            $"你的 Telegram 与 {u.ProperNamecard}({u.Uin})绑定成功\n" +
                            "点击链接加入大群:\n" +
                            C.link);
                        return;
                }
            }

            //规则转发
            if (!G.Map.ContainsKey(e.Message.Chat.Id)) return;
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

            if (e.Message.Text != null)
            {
                //文本转发 tg2q
                C.QQ.SendGroupMsg(G.Map[e.Message.Chat.Id],
                    sdr + ":\n" +
                    Utf.Encode(e.Message.Text));
                //命令
                if (e.Message.Chat.Id == G.TG.si)
                    Cmds.Enter(e.Message.Text, 839827911, false);

                if (e.Message.Chat.Id == G.TG.major)
                {
                    Cmds.Enter(e.Message.Text.GetRight("sudo "), 839827911, true);
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
            var targ = G.Map[e.Message.Chat.Id];
            var pf = await C.TG.GetFileAsync(ps);
            var hash = await C.QQ.UploadGroupPic(targ, $"https://api.telegram.org/file/bot{Token}/{pf.FilePath}");
            var tos = sdr + ":\n";
            if (!string.IsNullOrWhiteSpace(e.Message.Caption))
            {
                tos += e.Message.Caption + "\n";
            }

            tos = Utf.Encode(tos);
            tos += hash;
            C.QQ.SendGroupMsg(targ, tos);
        }
    }
}