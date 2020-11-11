namespace CornSDK
{
    public class GroupMemberChangedArgs
    {
        internal GroupMemberChangedArgs()
        {
        }

        public Corn Robot { get; internal set; }
        public long FromQQ { get; internal set; }
        public string FromNick { get; internal set; }
        public long FromGroup { get; internal set; }
        public string FromGroupName { get; internal set; }
    }
}