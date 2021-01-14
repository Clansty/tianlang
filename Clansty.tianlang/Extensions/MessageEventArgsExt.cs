using System.Threading.Tasks;
using Mirai_CSharp.Models;

namespace Clansty.tianlang
{
    public static class MessageEventArgsExt
    {
        public static Task Reply(this IGroupMessageEventArgs e, IMessageBase[] chain, bool includeSource = false)
        {
            return C.QQ.NthsBot.SendGroupMessageAsync(e.Sender.Group.Id, chain,
                includeSource ? ((SourceMessage) e.Chain[0]).Id : (int?) null);
        }

        public static Task Reply(this IGroupMessageEventArgs e, string msg, bool includeSource = false)
        {
            return Reply(e, new IMessageBase[] {new PlainMessage(msg)}, includeSource);
        }
    }
}