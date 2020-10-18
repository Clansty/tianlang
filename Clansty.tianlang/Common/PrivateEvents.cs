using CornSDK;

namespace Clansty.tianlang
{
    public class PrivateEvents : ICornEventHandler
    {
        public void OnFriendMsg(FriendMsgArgs e)
        {
            
        }

        public void OnTempMsg(TempMsgArgs e)
        {
            
        }

        public void OnGroupMsg(GroupMsgArgs e)
        {
            Q2Tg.NewGroupMsg(e);
        }

        public void OnFriendRequest(FriendRequestArgs e)
        {
            
        }

        public void OnGroupInviteRequest(GroupRequestArgs e)
        {
            
        }

        public void OnGroupJoinRequest(GroupRequestArgs e)
        {
            
        }

        public void OnGroupAddMember(GroupMemberChangedArgs e)
        {
            
        }

        public void OnGroupLeftMember(GroupMemberChangedArgs e)
        {
            
        }
    }
}