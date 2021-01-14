using System.Threading.Tasks;
using Mirai_CSharp;
using Mirai_CSharp.Models;

namespace Clansty.tianlang
{
    public class PrivateMsgArgs
    {
        public long FromQQ { get; set; }
        public string FromNick { get; set; }
        public IMessageBase[] Chain { get; set; }
        public MiraiHttpSession Robot { get; set; }

        public Task Reply(string msg, bool includeSource = false) =>
            Reply(new IMessageBase[] {new PlainMessage(msg)}, includeSource);

        public Task Reply(IMessageBase[] chain, bool includeSource = false) =>
            Robot.SendTempMessageAsync(
                FromQQ,
                G.major,
                chain,
                includeSource ? ((SourceMessage) Chain[0]).Id : (int?) null);
    }
}