namespace CornSDK
{
    public interface ICornEventHandler: IFriendMsgHandler, ITempMsgHandler, IGroupMsgHandler, IFriendRequestHandler, IGroupInviteRequestHandler, IGroupJoinRequestHandler, IGroupAddMemberHandler, IGroupLeftMemberHandler
    {
    }
}