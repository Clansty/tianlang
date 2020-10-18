using System.Threading.Tasks;

namespace CornSDK
{
    public class GroupMsgArgs
    {
        internal GroupMsgArgs()
        {
        }

        public long FromQQ { get; internal set; }
        public long FromGroup { get; internal set; }
        public long Time { get; internal set; }
        public string FromGroupName { get; internal set; }
        public string FromCard { get; internal set; }
        public string FromTitle { get; internal set; }
        public string Msg { get; internal set; }
        public string ReplyMsg { get; internal set; }
        public Corn Robot { get; internal set; }
        public long RecvQQ { get; set; }

        public Task Reply(string msg, bool anonymous = false) => Robot.SendGroupMsg(FromGroup, msg, anonymous);
    }
}