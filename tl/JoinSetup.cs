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
        private static void SetStep(string qq, Step status) => Db.Exec($"UPDATE user_info SET step={status} WHERE QQ='{qq}'");
        /// <summary>
        /// 根据 uid 设置 Step
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="status"></param>
        private static void SetStep(int uid, Step status) => Db.Exec($"UPDATE user_info SET step={status} WHERE uid={uid}");
        private static Step GetStep(string qq)
        {
            SqlDataReader r = Db.QueryReader($"SELECT step FROM user_info WHERE QQ='{qq}'");
            int s = 0;
            while (r.Read())
                s = Convert.ToInt32(r[0]);
            r.Close();
            return (Step)s;
        }

        /// <summary>
        /// 设置某个 QQ 的 SubStep
        /// </summary>
        /// <param name="qq"></param>
        /// <param name="status"></param>
        private static void SetSubStep(string qq, SubStep status) => Db.Exec($"UPDATE user_info SET substep={status} WHERE QQ='{qq}'");
        /// <summary>
        /// 根据 uid 设置 Step
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="status"></param>
        private static void SetSubStep(int uid, SubStep status) => Db.Exec($"UPDATE user_info SET substep={status} WHERE uid={uid}");
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
            gradeNickName,
            gradeName,
            
        }
        private enum SubStep
        {
            no,
            grade,
            nick,
            name,
        }

        public void Start(string qq)
        {
            C.GetUid(qq);
            C.SetStatus(qq, Status.joinSetup);
            SetStep(qq, Step.gradeNickName);
            if (!C.isTest)
                S.Major($"欢迎新人 [IR:at={qq}]，请注意我给你发送的私聊消息哦~");
            S.P(qq, $"[Version 2.{C.version}][Next]" +
                "你好，我是甜狼，本群的人工智能管理[Next]" +
                "欢迎加入本群，请跟随引导回答以下问题，我会自动帮你修改群名片。[Next]" +
                "如果输入错误，你可以说<上一步>，或者<重新开始>.(不包括尖括号哦）");
            SetSubStep(qq, SubStep.grade);
            S.P(qq, "请回复你的 校区 年级 [班级]\n" +
                "年级是必须要的，校区不写默认本部，班级可以不写\n" +
                "例如: 高二八班，金阊高一1班，本部高三2班，2018届");
        }

        public void Enter(string QQ)
        {

        }
    }
}
