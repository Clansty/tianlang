using System;

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
            
            S.Private(u, $"/bind {bindCode:x}!\n请完整复制此条消息\n在 Telegram 中搜索\"@tianlangBot\"并发送此文本");
        }
    }
}