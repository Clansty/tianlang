using System.Collections.Generic;
using System.Threading.Tasks;

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
        internal static Task UpdateMajor()
        {
            return Task.Run(() =>
            {
                UpdateMajor(Robot.GetGroupMembers(G.major));
            });
        }
    }
}
