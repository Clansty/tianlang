using System.Collections.Generic;

namespace Clansty.tianlang
{
    static class MemberList
    {
        public static HashSet<long> major = new HashSet<long>();
        public static void UpdateMajor(ICollection<long> l)
        {
            major.Clear();
            foreach (var i in l)
            {
                major.Add(i);
            }
        }
        public static void UpdateMajor()
        {
            UpdateMajor(Robot.GetGroupMembers(G.major));
        }
    }
}
