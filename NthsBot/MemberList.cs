using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clansty.tianlang
{
    static class MemberList
    {
        public static HashSet<string> major = new HashSet<string>();
        public static HashSet<string> g2020 = new HashSet<string>();
        public static void UpdateMajor(List<GroupMember> l)
        {
            major.Clear();
            foreach (var i in l)
            {
                major.Add(i.QQ);
            }
        }
        public static void UpdateG2020(List<GroupMember> l)
        {
            g2020.Clear();
            foreach (var i in l)
            {
                g2020.Add(i.QQ);
            }
        }
        public static void UpdateMajor()
        {
            UpdateMajor(Robot.Group.GetMembers(G.major));
        }
        public static void UpdateG2020()
        {
            UpdateMajor(Robot.Group.GetMembers(G.g2020));
        }
    }
}
