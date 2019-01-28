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
        public static void Enroll(string QQ,string group)
        {
            string json = IRQQApi.Api_GetGroupMemberList(C.w, G.test);

            dynamic d = JToken.Parse(json);
            JArray ja = d.mems;

            List<GroupMemberList> l = ja.ToObject<List<GroupMemberList>>();

            string c = "";
            string n = "";
            foreach (GroupMemberList m in l)
            {
                c = c + m.card + '\n';
                n = n + m.nick + '\n';
            }

            S.Test(c);
            S.Test(n);


        }
    }
}
