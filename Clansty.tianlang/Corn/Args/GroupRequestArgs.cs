using System.Threading.Tasks;

namespace CornSDK
{
    public class GroupRequestArgs
    {
        internal GroupRequestArgs()
        {
        }

        public Corn Robot { get; internal set; }
        public long FromQQ { get; internal set; }
        public long OperateQQ { get; internal set; }
        public string OperateNick { get; internal set; }
        public string FromNick { get; internal set; }
        public long FromGroup { get; internal set; }
        public string FromGroupName { get; internal set; }
        public string Msg { get; internal set; }
        internal int Type { get; set; }
        internal long Seq { get; set; }
        internal long fromqq { private get; set; }
        public Task Accept() => Robot.Post("setgroupaddrequest", new
        {
            fromqq,
            group = FromGroup,
            qq = FromQQ,
            seq = Seq,
            op = 11,
            type = Type
        });
        public Task Reject(string reason = "") => Robot.Post("setgroupaddrequest", new
        {
            fromqq,
            group = FromGroup,
            qq = FromQQ,
            seq = Seq,
            op = 12,
            type = Type,
            reason
        });

    }
}