using System.Threading.Tasks;

namespace CornSDK
{
    public class TempMsgArgs
    {
        internal TempMsgArgs() { }
        public long FromQQ { get; internal set; }
        public long FromGroup { get; internal set; }
        public string FromNick { get; internal set; }
        public string Msg { get; internal set; }
        public Corn Robot { get; internal set; }
        public Task Reply(string msg) => Robot.SendTempMsg(FromGroup, FromQQ, msg);
    }
}