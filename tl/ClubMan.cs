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
        /// 开始 enroll 会话
        /// </summary>
        /// <param name="group"></param>
        public ClubMan(string group)
        {
            S.Group(group, "注册会话已启动，请查看私聊界面");
            string qq = C.GetMaster(group);
            int uid = C.GetUid(qq);
            //bool isFoM = IRQQApi.Api_IfFriend(C.w, master.uin.ToString()) || C.IsMember(master.uin.ToString());
            //if (!isFoM)
            C.SetSession(uid, group);
            C.SetStatus(uid, Status.clubMan);
            SetStep(uid, Step.enroll);
            SetSubStep(uid, SubStep.name);

            Db.Exec($"INSERT INTO club_info (master, groupid) VALUES ({uid}, '{group}')");

            if (!IRQQApi.IsFriend(qq))
                S.P(qq, "你和甜狼还不是好友。建议你先加为好友以方便后续操作", true);
            S.P(uid, $"[甜狼 Ver.{C.version}]\n" +
                      "社团信息注册会话[Next]" +
                      "请回复社团的名字", true);
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
                            //string sd = msg;
                            //bool isOK = true;
                            //for (int i = 0; i < sd.Length; i++)
                            //{
                            //    if (sd[i] > 127)
                            //        isOK = false;
                            //}
                            //if (!isOK)
                            //{
                            //    R("子域名不能包含中文，请重新选择");
                            //    return;
                            //}
                            Commit("subdomain", $"'{msg}'");                            
                            C.SetStatus(u, Status.no);
                            R("保存成功\n" +
                             $"你的访问域名将是 https://{msg}.nths.moe[Next]");
                            SqlDataReader r = Db.QueryReader($"SELECT groupid FROM club_info WHERE master={u.Uid}");
                            string group = "";
                            while (r.Read())
                                group = r[0].ToString();
                            r.Close();
                            r = null;
                            Thread.Sleep(500);
                            r = Db.QueryReader($"SELECT name FROM club_info WHERE master={u.Uid}");
                            string n = "";
                            while (r.Read())
                                n = r[0].ToString();
                            r.Close();
                            r = null;
                            R($"正在通过群 {IRQQApi.Api_GetGroupName(C.W, group)} 导入成员，请稍后");
                            ImportResult import;
                            import = Import(group, n);
                            //R("导入完成\n" +
                            // $"导入了 {import.all} 个成员\n" +
                            // $"其中 {import.notReged} 个信息不完整，已发起信息补全会话");
                            R("导入完成\n" +
                             $"导入了 {import.all} 个成员");

                            break;
                    }
                    break;
            }

            void R(string rmsg) => S.P(QQ, rmsg, true);
            void Commit(string key, string value) => Db.Exec($"UPDATE club_info SET {key}={value} WHERE master={u.Uid}");
            void CommitSubStep() => SetSubStep(u.Uid, subStep);
        }


        public static ImportResult Import(string g, string brief)
        {
            int cid = 0;
            SqlDataReader r = Db.QueryReader($"SELECT cid FROM club_info WHERE groupid='{g}'");
            while (r.Read())
                cid = (int)r[0];
            r.Close();
            r = null;
            List<GroupMember> l = C.GetMembers(g);
            ImportResult result;
            result.all = 0;
            result.notReged = 0;
            //Stack<string> notReged = new Stack<string>();
            //notReged.Push("#"); //哨兵元素
            foreach (GroupMember m in l)
            {
                if (m.uin == C.wp || m.uin == C.wt)
                    continue; //跳过狼
                //User u = new User(m.uin);
                Db.Exec($"INSERT INTO club_member (cid, uid) VALUES ({cid}, {C.GetUid(m.uin)})");
                result.all++;
                //if (u.Enrollment == 0 || u.Name == "" || u.Nick == "")
                //    notReged.Push(m.uin);
            }
            //result.notReged = notReged.Count - 1;
            //l = null;//省点内存
            //while (notReged.Peek() != "#")
            //{
            //    InfoSetup.StartNonMember(notReged.Pop(), g, brief);
            //    Thread.Sleep(10000);
            //}
            return result;
        }

        public struct ImportResult
        {
            public int all;
            public int notReged;
            //public List<User> users;
        }
    }
}
