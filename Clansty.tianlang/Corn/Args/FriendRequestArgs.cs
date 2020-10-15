using System.Threading.Tasks;

namespace CornSDK
{
    public class FriendRequestArgs
    {
        internal FriendRequestArgs()
        {
        }

        public Corn Robot { get; internal set; }
        public long FromQQ { get; internal set; }
        public string FromNick { get; internal set; }
        public string Msg { get; internal set; }
        internal long Seq { private get; set; }
        public Task Accept() => Robot.Post("setfriendaddrequest", new
        {
            fromqq = Robot.config.selfQQ,
            qq = FromQQ,
            seq = Seq,
            op = 1
        });
        public Task Reject() => Robot.Post("setfriendaddrequest", new
        {
            fromqq = Robot.config.selfQQ,
            qq = FromQQ,
            seq = Seq,
            op = 2
        });
    }
}