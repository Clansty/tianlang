using System.Collections.Generic;

namespace Clansty.tianlang
{
    static class MemberList
    {
        public static HashSet<string> major = new HashSet<string>();
        public static void UpdateMajor(ICollection<string> l)
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
