using System;

namespace Clansty.tianlang
{
    static class SetupV3
    {
        public static void Enter(FriendMsgArgs e)
        {
            User u = new User(e.FromQQ);
            string m = e.Msg;

            if (m == "上一步")
            {
                if (u.Step > 1)
                    u.Step--;
                else
                    e.Reply(Strs.Get("setupCantBack")); //这是第一步，无法后退哦
                switch (u.Step)
                {
                    case 1:
                        e.Reply(Strs.Get("setupStep1")); //请回复你开学后的年级或毕业的年份，金阊分部的同学请在前面加上金阊\n例如 高一, 初三, 2019届, 2019届初中, 金阊高一
                        return;
                    case 2:
                        e.Reply(Strs.Get("setupStep2")); //请回复你想在群内使用的昵称，昵称中不需要包含年级
                        return;
                    default:
                        HandleStepErr();
                        return;
                }
            }//上一步 end
            if (m == "重新开始")
            {
                u.Step = 1;
                e.Reply(Strs.Get("setupStep1")); //请回复你开学后的年级或毕业的年份，金阊分部的同学请在前面加上金阊\n例如 高一, 初三, 2019届, 2019届初中, 金阊高一
                return;
            } //重新开始 end


            switch (u.Step)
            {
                case 1:
                    int g = UserInfo.ParseEnrollment(m);
                    if (g < 1970)
                    {
                        C.WriteLn($"setup step1 异常，{u.Uin} 发 {m}", ConsoleColor.Yellow);
                        S.Si("群名片向导异常\n" +
                            $"QQ: {u.Uin}\n" +
                            $"在 step 1 发送: {m}");
                        e.Reply(Strs.Get("setupStep1Err")); //"回复格式不正确，必须包含年级哦"
                        e.Reply(Strs.Get("setupStep1")); //请回复你开学后的年级或毕业的年份，金阊分部的同学请在前面加上金阊\n例如 高一, 初三, 2019届, 2019届初中, 金阊高一
                        return;
                    }
                    u.Enrollment = g;
                    u.Junior = UserInfo.ParseJunior(m);
                    u.Step = 2;
                    C.WriteLn($"{u.Uin} 加群向导进入第 2 步", ConsoleColor.Cyan);
                    e.Reply($"年级: {u.Grade}\n" +
                             "如有判断错误，可以说<上一步>");
                    e.Reply(Strs.Get("setupStep2"));//请回复你想在群内使用的昵称，昵称中不需要包含年级
                    return;
                case 2:
                    u.Nick = m;
                    u.Namecard = u.ProperNamecard;
                    u.Step = 3;
                    if (u.Name != "")
                    {
                        FinishWizard();
                        return;
                    }
                    C.WriteLn($"{u.Uin} 加群向导进入第 3 步", ConsoleColor.Cyan);
                    e.Reply(Strs.Get("setupStep3"));//请告诉我你的姓名，这并不会在群里公开               
                    return;
                case 3:
                    u.Name = m;
                    FinishWizard();
                    return;
                default:
                    HandleStepErr();
                    return;
            }

            void FinishWizard()
            {
                u.Step = -1;
                u.Status = Status.no;
                C.WriteLn($"{u.Uin} 加群向导完成", ConsoleColor.Green);
                e.Reply(Strs.Get("setupSetNC", u.ProperNamecard));//你的群名片已修改为 {0}
                e.Reply(Strs.Get("setupOK"));//目前我们需要的信息就这么多，祝你在群里玩的开心
                string tip = Strs.Get($"setupTip{u.Enrollment}");//根据年级来分的提示
                if (tip != "")
                    e.Reply(tip); //你是高一的，建议同时加入 2018 级新高一年级群 //??2019级新生即将来临?敬请期待??? 
                tip = Strs.Get($"setupTipAll");//给所有人的广告位
                if (tip != "")
                    e.Reply(tip); //目前是这个：【西花园事务所】是江苏省苏州第十中学校学生自建生活服务平台，添加【西花园事务所】为特别关心，可以第一时间收到最新消息 
                S.Si(u.ToXml("新人信息"));
            }
            void HandleStepErr()
            {
                C.WriteLn($"{e.FromQQ} setup 出现 step 有毛病的问题，step = {u.Step}，已重置状态", ConsoleColor.Magenta);
                u.Status = Status.no;
                u.Step = -1;
            }
        }

        public static void New(string qq, bool first = false)
        {
            var u = new User(qq)
            {
                Status = Status.setup,
                Step = 1
            };
            if (first)
            {
                S.Private(qq, Strs.Get("setupGreetingIntr")); //你好，我是甜狼，本群的人工智能管理
                S.Private(qq, Strs.Get("setupGreetingInfo")); //欢迎加入本群，请跟随引导回答以下问题，我会自动帮你修改群名片。
                S.Private(qq, Strs.Get("setupGreetingMist")); //如果输入错误，你可以说<上一步>，或者<重新开始>.(不包括尖括号哦）
                S.Major(Strs.Get("setupGroupWelcome", qq)); //欢迎新人 [@{0}]，请注意我给你发送的私聊消息哦~
            }
            // TODO: 这里根据已有信息判断需要进入哪一步
            if (u.Enrollment < 1970)
                S.Private(qq, Strs.Get("setupStep1")); //请回复你开学后的年级或毕业的年份，金阊分部的同学请在前面加上金阊\n例如 高一, 初三, 2019届, 2019届初中, 金阊高一
            else
            {
                u.Step = 2;
                S.Private(qq, Strs.Get("setupStep2"));
            }
            C.WriteLn($"{qq} 加群向导启动 first = {first}", ConsoleColor.Cyan);
        }
    }
}