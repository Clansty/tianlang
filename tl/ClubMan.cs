using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace tianlang
{
    public static class ClubMan
    {
        public static void Enroll(string QQ, string group)
        {
            string json = IRQQApi.Api_GetGroupMemberList(C.w, group);
            dynamic d = JToken.Parse(json);
            JArray ja = d.mems;
            List<GroupMember> l = ja.ToObject<List<GroupMember>>();

            GroupMember groupMaster = l.Find(C.master);

            if (groupMaster.uin.ToString() != QQ)
            {
                S.Group(group, "只有群主可以使用此功能");
                return;
            }
        }
        private static void Import(int cid, List<GroupMember> l)
        {
            
            string c = "";
            string n = "";
            foreach (GroupMember m in l)
            {
                c = c + m.card + '\n';
                n = n + m.nick + '\n';
            }

            S.Test(c);
            S.Test(n);

        }
    }
}
