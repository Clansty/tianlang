﻿using System;namespace Clansty.tianlang{    class Setup    {        public static void New(string qq, bool first = false)        {            var u = new User(qq);            if (u.Verified && u.Grade != "未知") //代表不是新人的情况            {                if (first)                    return;                throw new Exception("看上去此人已经实名了或无需认证，更换实名信息需要走管理人工验证流程");            }            u.Status = Status.setup;            u.Step = 1;            S.Private(qq, $"[甜狼 版本 {C.Version}]");            S.Private(qq, Strs.SetupGreetingIntr); //你好，我是甜狼，本群的人工智能管理            S.Private(qq, "为了保证本群的安全与秩序，请告诉我你的真实姓名来验证你的身份，这并不会在群里公开");            if (first)                S.Major($"欢迎新人 [LQ:@{qq}]，请注意我给你发送的私聊消息哦~");            C.WriteLn($"{qq} 加群向导启动 first = {first}", ConsoleColor.Cyan);        }        public static void Enter(FriendMsgArgs e)        {            var u = new User(e.FromQQ);            string m = e.Msg.Trim();            switch (u.Step)            {                case 2: // input grade                    var g = UserInfo.ParseEnrollment(m);                    if (g < 1970)                    {                        C.WriteLn($"setup step1 异常，{u.Uin} 发 {m}", ConsoleColor.Yellow);                        S.Si("群名片向导异常\n" +                             $"QQ: {u.Uin}\n" +                             $"在 step 2 发送: {m}");                        e.Reply(Strs.SetupStep2Err); //"回复格式不正确，必须包含年级哦"                        e.Reply(Strs.SetupStep2); //                        return;                    }                    u.Enrollment = g;                    u.Junior = UserInfo.ParseJunior(m);                    if (u.VerifyMsg == RealNameVerifingResult.notFound)                    {                        e.Reply("看上去此实名身份无法找到，请务必使用真实姓名并检查姓名拼写。你可以通过回复“我叫xxx”（其中 xxx 为你的真实姓名）来重新填写姓名");                        FinishWizard("新人实名失败");                        return;                    }                    FinishWizard();                    return;                case 1: //input name                    if (!UserInfo.NameVerify(m))                    {                        e.Reply(Strs.NameVerifyFailed);                        return;                    }                    u.Name = m;                    if (u.VerifyMsg == RealNameVerifingResult.succeed)                    {                        FinishWizard();                        return;                    }                    if (u.VerifyMsg == RealNameVerifingResult.occupied)                    {                        e.Reply("看上去此实名身份已经有另一个账号加入，请联系管理员处理");                        FinishWizard("新人账号冲突");                        return;                    }                    u.Step = 2;                    C.WriteLn($"{u.Uin} 加群向导进入第 2 步", ConsoleColor.Cyan);                    e.Reply("请回复你开学后的年级或毕业的年份\n例如 高一, 初三, 2019届, 2019届初中, 2019届初中/高一");                    return;                default:                    HandleStepErr();                    return;            }            void FinishWizard(string msg = "新人信息")            {                u.Step = -1;                u.Status = Status.no;                C.WriteLn($"{u.Uin} 加群向导完成", ConsoleColor.Green);                e.Reply(string.Format(Strs.SetupSetNC, u.ProperNamecard)); //你的群名片已修改为 {0}                e.Reply(Strs.SetupOK); //目前我们需要的信息就这么多，祝你在群里玩的开心                //TODO: 推年级群                S.Si(u.ToXml(msg));            }            void HandleStepErr()            {                C.WriteLn($"{e.FromQQ} setup 出现 step 有毛病的问题，step = {u.Step}，已重置状态", ConsoleColor.Magenta);                u.Status = Status.no;                u.Step = -1;            }        }    }}