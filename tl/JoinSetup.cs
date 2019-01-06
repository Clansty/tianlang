using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace tianlang
{
    public class JoinSetup
    {


        /// <summary>
        /// 设置某个 QQ 的 Step
        /// </summary>
        /// <param name="qq"></param>
        /// <param name="status"></param>
        private static void SetStep(string qq, Step status) => Db.Exec($"UPDATE user_info SET step={(int)status} WHERE QQ='{qq}'");
        /// <summary>
        /// 根据 uid 设置 Step
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="status"></param>
        private static void SetStep(int uid, Step status) => Db.Exec($"UPDATE user_info SET step={(int)status} WHERE uid={uid}");
        private static Step GetStep(string qq)
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
        private static void SetSubStep(string qq, SubStep status) => Db.Exec($"UPDATE user_info SET substep={(int)status} WHERE QQ='{qq}'");
        /// <summary>
        /// 根据 uid 设置 SubStep
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="status"></param>
        private static void SetSubStep(int uid, SubStep status) => Db.Exec($"UPDATE user_info SET substep={(int)status} WHERE uid={uid}");
        private static SubStep GetSubStep(string qq)
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
            /// <summary>
            /// 提交年级昵称姓名，进入大群时使用
            /// </summary>
            gradeNickName,
            /// <summary>
            /// 提交年级班级姓名，为今后保留
            /// </summary>
            gradeClassName,
            
        }
        private enum SubStep
        {
            no,
            grade,
            nick,
            name,
        }

        // 文本常量
        private const string welcome = "你好，我是甜狼，本群的人工智能管理[Next]" +
                                       "欢迎加入本群，请跟随引导回答以下问题，我会自动帮你修改群名片。[Next]" +
                                       "如果输入错误，你可以说<上一步>，或者<重新开始>.(不包括尖括号哦）";
        private const string step1 = "请回复你的 校区 年级 [班级]\n" +
                                     "年级是必须要的，校区不写默认本部，班级可以不写\n" +
                                     "例如: 高二八班，金阊高一1班，本部高三2班，2018届";
        private const string step2 = "请回复你想在群内使用的昵称，昵称中不需要包含年级";
        private const string step3 = "请告诉我你的姓名，这并不会在群里公开";

        private const string grade1Tip = "你是高一的，建议同时加入 2018 级新高一年级群，如果你还没有加入的话\n" +
                                         "长按识别二维码，即可加入\n" +
                                         "[IR:pic=C:\\Users\\Administrator\\Pictures\\njq.jpg]";
        private const string wgoTip = "【西花园事务所】是江苏省苏州第十中学校学生自建生活服务平台，添加【西花园事务所】为特别关心，可以第一时间收到最新消息\n" +
                                      "QQ 号: 728400657\n" +
                                      "[IR:pic=C:\\Users\\Administrator\\Pictures\\wgo.jpg]";

        private const string cantGoBack = "这是第一步，无法后退哦";

        public static void Start(string qq)
        {
            C.GetUid(qq);
            C.SetStatus(qq, Status.joinSetup);
            SetStep(qq, Step.gradeNickName);
            if (!C.isTest)
                S.Major($"欢迎新人 [IR:at={qq}]，请注意我给你发送的私聊消息哦~");
            S.P(qq, $"[Version 2.{C.version}][Next]" + welcome);
            SetSubStep(qq, SubStep.grade);
            S.P(qq, step1);
        }

        public static void Enter(string QQ, string msg)
        {
            Step step = GetStep(QQ);
            SubStep subStep = GetSubStep(QQ);
            User u = new User(QQ);

            if (msg == "上一步")
            {
                if ((int)subStep > 1)
                    subStep--;
                else
                    R(cantGoBack);
                switch (step)
                {
                    case Step.gradeNickName:
                        switch (subStep)
                        {
                            case SubStep.grade:
                                R(step1);
                                break;
                            case SubStep.nick:
                                R(step2);
                                break;
                        }
                        break;
                }
                CommitSubStep();
                return;
            }

            if (msg == "重新开始")
            {
                subStep = SubStep.grade;
                R(step1);
                CommitSubStep();
            }

            switch (step)
            {
                case Step.gradeNickName:
                    switch (subStep)
                    {
                        // 年级步骤
                        case SubStep.grade:
                            u.Fill(msg);
                            if (u.Grade == "")
                            {
                                S.Si("群名片向导异常\n" +
                                    $"QQ: {QQ}\n" +
                                    $"在 step 1 发送: {msg}");
                                R("回复格式不正确，必须包含年级哦");
                                R(step1);
                            }
                            else
                            {
                                string r = $"年级: {u.Grade}";
                                if (u.Class != 0)
                                    r = r + $"\n班级: {u.Class}";
                                r = r + "\n校区: " + (u.Branch ? "金阊" : "本部");
                                r = r + "\n如果判断错误，可以说上一步";
                                R(r);
                                Commit("class", u.Class.ToString());
                                Commit("enrollment", u.Enrollment.ToString());
                                Commit("branch", u.Branch ? "1" : "0");
                                subStep = SubStep.nick;
                                CommitSubStep();
                                R(step2);
                            }
                            break;
                        // 昵称步骤
                        case SubStep.nick:
                            string qmp = $"{u.Grade} | {msg}";
                            if (u.Branch)
                                qmp = "金阊" + qmp;
                            bool isOK = IRQQApi.Api_SetGroupCard(C.w, C.isTest ? G.test : G.major, u.QQ, qmp);
                            if (!isOK)
                            {
                                R(C.err);
                                S.Test($"修改群名片出错\n" +
                                       $"QQ: {u.QQ}\n" +
                                       $"Card: {qmp}");
                            }
                            Commit("nick", $"'{msg}'");
                            subStep = SubStep.name;
                            CommitSubStep();
                            R(step3);
                            break;
                        // 姓名步骤
                        case SubStep.name:
                            R($"你的群名片已修改为 {u.NameCard}[Next]" +
                               "目前我们需要的信息就这么多，祝你在群里玩的开心");
                            if (u.Enrollment == 2018)
                                R(grade1Tip);
                            R(wgoTip);
                            Commit("name", $"'{msg}'");
                            subStep = SubStep.no;
                            CommitSubStep();
                            C.SetStatus(QQ, Status.no);
                            u = new User(QQ);
                            Si.R("新人信息\n" + u.ToString());
                            break;
                    }
                    break;
            }
            void R(string rmsg) => S.P(QQ, rmsg);
            void Commit(string key, string value) => Db.Exec($"UPDATE user_info SET {key}={value} WHERE uid={u.Uid}");
            void CommitSubStep() => SetSubStep(u.Uid, subStep);
        }
    }
}
