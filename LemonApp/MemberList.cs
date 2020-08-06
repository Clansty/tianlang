using System.Collections.Generic;
using Native.Sdk.Cqp.Model;

namespace Clansty.tianlang
{
    static class MemberList
    {
        public static HashSet<string> major = new HashSet<string>();
        public static void UpdateMajor(GroupMemberInfoCollection l)
        {
            major.Clear();
            foreach (var i in l)
            {
                major.Add(i.QQ);
            }
        }
        public static void UpdateMajor()
        {
            UpdateMajor(C.CQApi.GetGroupMemberList(G.major));
        }
    }
}
