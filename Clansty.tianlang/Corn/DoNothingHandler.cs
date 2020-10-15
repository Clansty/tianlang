namespace CornSDK
{
    public class DoNothingHandler : IFriendMsgHandler, ITempMsgHandler, IGroupMsgHandler, IFriendRequestHandler, IGroupInviteRequestHandler, IGroupJoinRequestHandler, IGroupAddMemberHandler, IGroupLeftMemberHandler
    {
        public void OnFriendMsg(FriendMsgArgs e) { }
        public void OnTempMsg(TempMsgArgs e) { }
        public void OnGroupMsg(GroupMsgArgs e) { }
        public void OnFriendRequest(FriendRequestArgs e) { }
        public void OnGroupJoinRequest(GroupRequestArgs e) { }
        public void OnGroupInviteRequest(GroupRequestArgs e) { }
        public void OnGroupAddMember(GroupMemberChangedArgs e)
        {
        }
        public void OnGroupLeftMember(GroupMemberChangedArgs e) { }
    }
}
