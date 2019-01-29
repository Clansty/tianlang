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
        /// <summary>
        /// 设置某个 QQ 的 Step
        /// </summary>
        /// <param name="qq"></param>
        /// <param name="status"></param>
        private void SetStep(string qq, Step status) => Db.Exec($"UPDATE user_info SET step={(int)status} WHERE QQ='{qq}'");
        /// <summary>
        /// 根据 uid 设置 Step
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="status"></param>
        private void SetStep(int uid, Step status) => Db.Exec($"UPDATE user_info SET step={(int)status} WHERE uid={uid}");
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

        /// <summary>
        /// 设置某个 QQ 的 SubStep
        /// </summary>
        /// <param name="qq"></param>
        /// <param name="status"></param>
        private void SetSubStep(string qq, SubStep status) => Db.Exec($"UPDATE user_info SET substep={(int)status} WHERE QQ='{qq}'");
        /// <summary>
        /// 根据 uid 设置 SubStep
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="status"></param>
        private void SetSubStep(int uid, SubStep status) => Db.Exec($"UPDATE user_info SET substep={(int)status} WHERE uid={uid}");
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

        public ClubMan(string group)
        {
            
        }

        private void Enroll(string QQ, string group)
        {

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
