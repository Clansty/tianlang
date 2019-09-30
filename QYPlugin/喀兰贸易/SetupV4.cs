﻿using System;

namespace Clansty.tianlang
{
    class Setup
    {
        public static void New(string qq, bool first = false)
        {
            var u = new User(qq);
            if (u.Verified && u.Grade != "未知") //代表不是新人的情况
            {
                if (first)
                    return;
                throw new Exception("看上去此人已经实名了或无需认证，更换实名信息需要走管理人工验证流程");
            }
            u.Status = Status.setup;
            u.Step = 1;
            S.Private(qq, Strs.Get("setupGreetingIntr")); //你好，我是甜狼，本群的人工智能管理
            S.Private(qq, "为了保证本群的安全与秩序，请告诉我你的真实姓名来验证你的身份，这并不会在群里公开");
            S.Major(Strs.Get("setupGroupWelcome", qq)); //欢迎新人 [@{0}]，请注意我给你发送的私聊消息哦~
            C.WriteLn($"{qq} 加群向导启动 first = {first}", ConsoleColor.Cyan);
        }
        public static void Enter(FriendMsgArgs e)
        {
            User u = new User(e.FromQQ);
            string m = e.Msg;

            switch (u.Step)
            {
                case 2:
                    int g = UserInfo.ParseEnrollment(m);
                    if (g < 1970)
                    {
                        C.WriteLn($"setup step1 异常，{u.Uin} 发 {m}", ConsoleColor.Yellow);
                        S.Si("群名片向导异常\n" +
                            $"QQ: {u.Uin}\n" +
                            $"在 step 2 发送: {m}");
                        e.Reply(Strs.Get("setupStep1Err")); //"回复格式不正确，必须包含年级哦"
                        e.Reply("请回复你开学后的年级或毕业的年份\n例如 高一, 初三, 2019届, 2019届初中, 2019届初中/高一"); //
                        return;
                    }
                    u.Enrollment = g;
                    u.Junior = UserInfo.ParseJunior(m);
                    if (u.VerifyMsg == RealNameVerifingResult.notFound)
                    {
                        e.Reply("看上去此实名身份无法找到，请务必使用真实姓名并检查姓名拼写。你可以通过回复“我叫xxx”（其中 xxx 为你的真实姓名）来重新填写姓名");
                        FinishWizard("新人实名失败");
                        return;
                    }
                    FinishWizard();
                    return;
                case 1:
                    u.Name = m;
                    if (u.VerifyMsg == RealNameVerifingResult.succeed)
                    {
                        FinishWizard();
                        return;
                    }

                    if (u.VerifyMsg == RealNameVerifingResult.occupied)
                    {
                        e.Reply("看上去此实名身份已经有另一个账号加入，请联系管理员处理");
                        FinishWizard("新人账号冲突");
                        return;
                    }
                    u.Step = 2;
                    C.WriteLn($"{u.Uin} 加群向导进入第 2 步", ConsoleColor.Cyan);
                    e.Reply("请回复你开学后的年级或毕业的年份\n例如 高一, 初三, 2019届, 2019届初中, 2019届初中/高一");
                    return;
                default:
                    HandleStepErr();
                    return;
            }

            void FinishWizard(string msg = "新人信息")
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
                S.Si(u.ToXml(msg));
            }
            void HandleStepErr()
            {
                C.WriteLn($"{e.FromQQ} setup 出现 step 有毛病的问题，step = {u.Step}，已重置状态", ConsoleColor.Magenta);
                u.Status = Status.no;
                u.Step = -1;
            }
        }

    }
}
