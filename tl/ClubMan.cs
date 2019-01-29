using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace tianlang
{
    public class ClubMan
    {
        private void SetStep(string qq, Step status) => Db.Exec($"UPDATE user_info SET step={(int)status} WHERE QQ='{qq}'");
        private void SetStep(int uid, Step status) => Db.Exec($"UPDATE user_info SET step={(int)status} WHERE uid={uid}");
        private void SetStep(GroupMember m, Step s) => SetStep(m.uin.ToString(), s);

        private Step GetStep(string qq)
        {
            SqlDataReader r = Db.QueryReader($"SELECT step FROM user_info WHERE QQ='{qq}'");
            int s = 0;
            while (r.Read())
                s = Convert.ToInt32(r[0]);
            r.Close();
            return (Step)s;
        }


        // 关于 SubStep 的设置项

        private void SetSubStep(string qq, SubStep status) => Db.Exec($"UPDATE user_info SET substep={(int)status} WHERE QQ='{qq}'");
        private void SetSubStep(int uid, SubStep status) => Db.Exec($"UPDATE user_info SET substep={(int)status} WHERE uid={uid}");
        private void SetSubStep(GroupMember m, SubStep s) => SetSubStep(m.uin.ToString(), s);

        private SubStep GetSubStep(string qq)
        {
            SqlDataReader r = Db.QueryReader($"SELECT substep FROM user_info WHERE QQ='{qq}'");
            int s = 0;
            while (r.Read())
                s = Convert.ToInt32(r[0]);
            r.Close();
            return (SubStep)s;
        }
        
        private enum Step
        {
            no,
            enroll
        }
        private enum SubStep
        {
            no,
            name,
            subdomain
        }

        public ClubMan(string QQ, string msg)
        {
            
        }

        /// <summary>
        /// 开始 enroll 过程
        /// </summary>
        /// <param name="group"></param>
        public ClubMan(string group)
        {
            GroupMember master = C.GetMaster(group);
            C.GetUid(master.uin.ToString());
            bool isFoM = IRQQApi.Api_IfFriend(C.w, master.uin.ToString()) || C.IsMember(master.uin.ToString());
            if (!isFoM)
                C.SetSession(master, group);
            C.SetStatus(master, Status.clubMan);
            SetStep(master, Step.enroll);
            SetSubStep(master, SubStep.name);

            master.S($"[甜狼 Ver.{C.version}][Next]" +
                      "请回复社团的名字");
        }

        private void Import(int cid, List<GroupMember> l)
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
