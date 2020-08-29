using System.Collections.Generic;

namespace Clansty.tianlang
{
    static class MemberList
    {
        internal static HashSet<long> major = new HashSet<long>();
        internal static void UpdateMajor(GroupMemberRaw l)
        {
            major.Clear();
            foreach (var i in l.member)
            {
                major.Add(i.uin);
            }
        }
        internal static void UpdateMajor()
        {
            UpdateMajor(Robot.GetGroupMembers(G.major));
        }
    }
}
