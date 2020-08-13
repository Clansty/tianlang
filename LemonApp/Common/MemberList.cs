using System.Collections.Generic;

namespace Clansty.tianlang
{
    static class MemberList
    {
        internal static HashSet<long> major = new HashSet<long>();
        internal static void UpdateMajor(ICollection<long> l)
        {
            major.Clear();
            foreach (var i in l)
            {
                major.Add(i);
            }
        }
        internal static void UpdateMajor()
        {
            UpdateMajor(Robot.GetGroupMembers(G.major));
        }
    }
}
