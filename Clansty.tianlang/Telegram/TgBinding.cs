using System;
using System.Globalization;
using Telegram.Bot.Args;

namespace Clansty.tianlang
{
    public class TgBinding
    {
        internal static void Init(User u, bool rebind = false)
        {
            if (!rebind)
                if (u.TgUid != 0)
                {
                    S.Private(u, "你已经绑定了一个 Telegram 账号\n如需重新绑定，请完整复制本条消息并发回给我");
                    return;
                }

            var bindCode = (long) int.MinValue - new Random().Next();
            while (Db.users.Select($"tg={bindCode}").Length > 0)
                bindCode = (long) int.MinValue - new Random().Next(); //不重复
            u.TgUid = bindCode;
            C.WriteLn($"/bind {bindCode:x}:\n请完整复制此条消息\n在 Telegram 中搜索\"@tianlangBot\"并发送此文本");
            S.Private(u, $"\\/bind {bindCode:x}:\n请完整复制此条消息\n在 Telegram 中搜索\"@tianlangBot\"并发送此文本");
        }

        internal static void Bind(MessageEventArgs e)
        {
            if (e.Message.From.Id != e.Message.Chat.Id) return;
            var action = e.Message.Text.Split(' ')[1];
            if (action.Contains(":"))
                action = action.GetLeft(":");
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
        }
    }
}