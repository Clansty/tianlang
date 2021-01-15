using System.Collections.Generic;
using System.Threading.Tasks;
using Mirai_CSharp.Models;

namespace Clansty.tianlang
{
    static class MemberList
    {
        internal static HashSet<long> major = new HashSet<long>();
        internal static void UpdateMajor(IGroupMemberInfo[] l)
        {
            major.Clear();
            foreach (var i in l)
            {
                major.Add(i.Id);
            }
        }
        internal static async Task UpdateMajor()
        {
            UpdateMajor(await C.QQ.NthsBot.GetGroupMemberListAsync(G.major));
        }
    }
}
