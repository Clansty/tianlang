using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LemonApp
{
    static class Events
    {
        //想更新功能，自己加，发 pr
        internal static void Exit()
        {

        }

        internal static void Enable()
        {

        }

        internal static void Disable()
        {

        }
        internal static void Msg(FriendMsgArgs e)
        {

        }
        internal static void Msg(GroupMsgArgs e)
        {
            if (e.Msg == "members")
            {
                var m = Robot.GetGroupMembers(e.FromGroup);
                var r = "";
                foreach (var i in m)
                {
                    r += i;
                    r += "\n";
                }
                r = r.Trim('\n');
                e.Reply(r);
            }
        }
        internal static void Msg(TempMsgArgs e)
        {

        }

        internal static void AddFriend(RequestAddFriendArgs requestAddFriendArgs)
        {

        }

        internal static void GroupAddMember(GroupAddMemberArgs groupAddMemberArgs)
        {

        }

        internal static void JoinGroupRequest(RequestAddGroupArgs requestAddGroupArgs)
        {

        }

        internal static void InviteGroupRequest(RequestAddGroupArgs requestAddGroupArgs)
        {

        }
    }
}
