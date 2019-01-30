using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
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

        /// <summary>
        /// Continue Enrollment Session
        /// </summary>
        /// <param name="QQ"></param>
        /// <param name="msg"></param>
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
                            try
                            {
                                Commit("subdomain", $"'{sd}'");
                            }
                            catch (Exception e)
                            {
                                R("出现错误: \n" +
                                 $"{e.Message}" +
                                  "可能是你指定的子域名已被占用");
                                return;
                            }
                            C.SetStatus(u, Status.no);
                            R("保存成功\n" +
                             $"你的访问域名将是 https://{sd}.nths.moe[Next]");
                            SqlDataReader r = Db.QueryReader($"SELECT groupid FROM club_info WHERE master={u.Uid}");
                            string group = "";
                            while (r.Read())
                                group = r[0].ToString();
                            r.Close();
                            r = Db.QueryReader($"SELECT name FROM club_info WHERE master={u.Uid}");
                            string n = "";
                            while (r.Read())
                                n = r[0].ToString();
                            r.Close();
                            R($"正在通过群 {IRQQApi.Api_GetGroupName(C.w, group)} 导入成员，可能需要一些时间，请稍后");
                            ImportResult import = Import(group, n);
                            R("导入完成\n" +
                             $"导入了 {import.all} 个成员\n" +
                             $"其中 {import.notReged} 个信息不完整，已发起信息补全会话");
                            break;
                    }
                    break;
            }

            void R(string rmsg) => S.P(QQ, rmsg);
            void Commit(string key, string value) => Db.Exec($"UPDATE club_info SET {key}={value} WHERE master={u.Uid}");
            void CommitSubStep() => SetSubStep(u.Uid, subStep);
        }

        /// <summary>
        /// 开始 enroll 会话
        /// </summary>
        /// <param name="group"></param>
        public ClubMan(string group)
        {
            S.Group(group, "注册会话已启动，请查看私聊界面");
            GroupMember master = C.GetMaster(group);
            int uid = C.GetUid(master.uin.ToString());
            bool isFoM = IRQQApi.Api_IfFriend(C.w, master.uin.ToString()) || C.IsMember(master.uin.ToString());
            if (!isFoM)
                C.SetSession(master, group);
            C.SetStatus(master, Status.clubMan);
            SetStep(master, Step.enroll);
            SetSubStep(master, SubStep.name);

            Db.Exec($"INSERT INTO club_info (master, groupid) VALUES ({uid}, '{group}')");

            master.S($"[甜狼 Ver.{C.version}]\n" +
                      "社团信息注册会话[Next]" +
                      "请回复社团的名字");
        }


        private ImportResult Import(string g, string brief)
        {
            int cid = 0;
            SqlDataReader r = Db.QueryReader($"SELECT cid FROM club_info WHERE groupid='{g}'");
            while (r.Read())
                cid = (int)r[0];
            r.Close();
            List<GroupMember> l = C.GetMembers(g);
            ImportResult result;
            result.all = 0;
            result.notReged = 0;
            result.users = new List<User>();
            foreach (GroupMember m in l)
            {
                if (m.uin.ToString() == C.wp || m.uin.ToString() == C.wt)
                    continue; //跳过狼
                string qq = m.uin.ToString();
                User u = new User(qq);
                Db.Exec($"INSERT INTO club_member (cid, uid) VALUES ({cid}, {u.Uid})");
                result.all++;
                if (u.Enrollment == 0 || u.Name == "" || u.Nick == "")
                {
                    result.notReged++;
                    InfoSetup.StartNonMember(qq, g, brief);
                    Thread.Sleep(1000);
                }
                result.users.Add(u);
            }
            return result;
        }

        private struct ImportResult
        {
            public int all;
            public int notReged;
            public List<User> users;
        }
    }
}
