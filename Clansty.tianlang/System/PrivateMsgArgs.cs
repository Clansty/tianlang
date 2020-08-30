using CornSDK;
using System.Threading.Tasks;

namespace Clansty.tianlang
{
    public class PrivateMsgArgs
    {
        public long FromQQ { get; set; }
        public string FromNick { get; set; }
        public string Msg { get; set; }
        public Corn Robot { get; set; }

        public Task Reply(string msg) => Robot.SendTempMsg(G.major, FromQQ, msg);
    }
}
