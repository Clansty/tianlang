using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Native.Sdk.Cqp.Model;

namespace Clansty.tianlang
{
    static class MemberList
    {
        public static HashSet<string> major = new HashSet<string>();
        public static HashSet<string> g2020 = new HashSet<string>();
        public static void UpdateMajor(GroupMemberInfoCollection l)
        {
            major.Clear();
            foreach (var i in l)
            {
                major.Add(i.QQ);
            }
        }
        public static void UpdateG2020(GroupMemberInfoCollection l)
        {
            g2020.Clear();
            foreach (var i in l)
            {
                g2020.Add(i.QQ);
            }
        }
        public static void UpdateMajor()
        {
            UpdateMajor(C.CQApi.GetGroupMemberList(G.major));
        }
        public static void UpdateG2020()
        {
            UpdateMajor(C.CQApi.GetGroupMemberList(G.major));
        }
    }
}
