using System.Linq;
using Telegram.Bot.Args;

namespace Clansty.tianlang
{
    internal static class Tg
    {
        internal const string Token = "712657902:AAG4eachKPVtfZeLpeR-8lAbDVy_6AMwASg";
        internal static async void OnMsg(object sender, MessageEventArgs e)
        {
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
                }

                //文本转发 tg2q
                if (G.Map.ContainsKey(e.Message.Chat.Id))
                {
                    C.QQ.SendGroupMsg(G.Map[e.Message.Chat.Id],
                        e.Message.From.FirstName + ":\n" +
                        Utf.Encode(e.Message.Text));
                }
                //命令
                if (e.Message.Chat.Id == G.TG.si)
                    Cmds.Enter(e.Message.Text, 839827911, false);

                if (e.Message.Chat.Id == G.TG.major)
                {
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
            if (!G.Map.ContainsKey(e.Message.Chat.Id)) return;
            var targ = G.Map[e.Message.Chat.Id];
            var pf = await C.TG.GetFileAsync(ps);
            var hash = await C.QQ.UploadGroupPic(targ, $"https://api.telegram.org/file/bot{Token}/{pf.FilePath}");
            var tos = e.Message.From.FirstName + ":\n";
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