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
            subDomain
        }

        public ClubMan(string QQ, string msg)
        {
            User u = new User(QQ);
            Step step = GetStep(QQ);
            SubStep subStep = GetSubStep(QQ);

            switch (step)
            {
                case Step.enroll:
                    switch (subStep)
                    {
                        case SubStep.name:
                            string name = msg;
                            Commit("name", $"'{name}'");
                            subStep = SubStep.subDomain;
                            CommitSubStep();
                            R($"名称: {name}\n" +
                               "请输入你要使用的子域名(https: //[?].nths .moe)\n" +
                               "[?]处的内容");
                            break;
                        case SubStep.subDomain:
                            string sd = msg;
                            bool isOK = true;
                            for (int i = 0; i < sd.Length; i++)
                            {
                                if (sd[i] > 127)
                                    isOK = false;
                            }
                            if (!isOK)
                            {
                                R("子域名不能包含中文，请重新选择");
                                return;
                            }
                            Commit("subdomain", $"'{sd}'");
                            C.SetStatus(u, Status.no);
                            R("保存成功\n" +
                             $"你的访问域名将是 https://{sd}.nths.moe[Next]");
                            SqlDataReader r = Db.QueryReader($"SELECT groupid FROM club_info WHERE master={u.Uid}");
                            string s = "";
                            while (r.Read())
                                s = r[0].ToString();
                            r.Close();
                            R($"正在通过群 {IRQQApi.Api_GetGroupName(C.w, s)} 导入成员，可能需要一些时间，请稍后");
                            break;
                    }
                    break;
            }

            void R(string rmsg) => S.P(QQ, rmsg);
            void Commit(string key, string value) => Db.Exec($"UPDATE club_info SET {key}={value} WHERE master={u.Uid}");
            void CommitSubStep() => SetSubStep(u.Uid, subStep);
        }

        /// <summary>
        /// 开始 enroll 过程
        /// </summary>
        /// <param name="group"></param>
        public ClubMan(string group)
        {
            S.Group(group, "注册过程已启动，请查看私聊界面");
            GroupMember master = C.GetMaster(group);
            int uid = C.GetUid(master.uin.ToString());
            bool isFoM = IRQQApi.Api_IfFriend(C.w, master.uin.ToString()) || C.IsMember(master.uin.ToString());
            if (!isFoM)
                C.SetSession(master, group);
            C.SetStatus(master, Status.clubMan);
            SetStep(master, Step.enroll);
            SetSubStep(master, SubStep.name);

            Db.Exec($"INSERT INTO club_info (master) VALUES ({uid})");

            master.S($"[甜狼 Ver.{C.version}]\n" +
                      "社团信息注册过程[Next]" +
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
