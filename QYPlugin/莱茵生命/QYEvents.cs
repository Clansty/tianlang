﻿using System;

namespace Clansty.tianlang
{
    public static class PluginInfo //插件的相关信息
    {
        public const string name = "甜狼 v3"; //名称
        public const string appid = "RhodesIsland"; //标识
        public const string version = "3.0"; //版本
        public const int versionId = 1; //版本ID
        public const string author = "凌莞"; //作者
        public const string description = "Clansty.tianlang.v3"; // 简介
    }

    public static class QYEvents
    {
        public static void Start()
        {
            C.AllocConsole();
            Console.Title = $"甜狼 {C.Version}";
            new Menu().Show();
            UserInfo.InitQmpCheckTask();
            //TG.InitAsync();
        }
        public static void FriendMsg(FriendMsgArgs e)
        {
            User u = new User(e.FromQQ);
            if (u.Status == Status.setup)
                Setup.Enter(e);
            else if (e.Msg == "whoami")
            {
                UserInfo.CheckQmpAsync(u);
                e.Reply(u.ToXml("你的信息"));
            }
            else if (e.Msg.StartsWith("我叫"))
            {
                var name = e.Msg.GetRight("我叫").Trim();
                if (u.Verified)
                {
                    e.Reply(Strs.Get("rnVerified"));//你已经实名认证了，无需再次补填姓名
                    return;
                }
                if (u.VerifyMsg == RealNameVerifingResult.unsupported)
                {
                    e.Reply(Strs.Get("rnUnsupported"));
                    return;
                }

                var chk = RealName.Check(name);

                if (chk.Status == RealNameStatus.notFound)
                {
                    e.Reply(Strs.Get("rnNotFound"));
                    return;
                }

                if (chk.OccupiedQQ != null && chk.OccupiedQQ != e.FromQQ)
                {
                    e.Reply(Strs.Get("rnOccupied"));
                    return;
                }

                if (chk.Status == RealNameStatus.e2017 && u.Enrollment != 2017)
                {
                    e.Reply(Strs.Get("rnUnmatch", u.Grade));
                    return;
                }
                if (chk.Status == RealNameStatus.e2018 && u.Enrollment != 2018)
                {
                    e.Reply(Strs.Get("rnUnmatch", u.Grade));
                    return;
                }
                if (chk.Status == RealNameStatus.e2019 && u.Enrollment != 2019)
                {
                    e.Reply(Strs.Get("rnUnmatch", u.Grade));
                    return;
                }


                if (chk.OccupiedQQ == null)
                {
                    //尝试进行验证
                    u.Name = name;
                    if (u.Verified)
                    {
                        e.Reply(Strs.Get("rnOK"));
                        return;
                    }
                    e.Reply(Strs.Get("unexceptedErr", u.VerifyMsg));
                    return;
                }
                UserInfo.CheckQmpAsync(u);
            }
        }
        public static void GroupMsg(GroupMsgArgs e)
        {
            // 网易云点歌
            if (e.Msg.StartsWith("点歌"))
                Netease.Start(e);

            if (e.FromGroup == G.si)
                Cmds.SiEnter(e);

            if (e.FromGroup == G.major)
            {
                UserInfo.CheckQmpAsync(new User(e.FromQQ), e.Card);
                if (e.Msg.StartsWith("sudo "))
                    Cmds.SudoEnter(e);
                Repeater.Enter(e.Msg);
                Stats.New(e);
            }

            //if (e.FromGroup == G.iDE)
            //{
            //    Q2TG.IDE(e);
            //}

            if (e.FromGroup == G.test)
            {
                if (e.Msg.StartsWith("{") && e.Msg.IndexOf('}') > 0)
                    e.Reply($"[LQ:lightappelem,type=1,data={e.Msg.Replace(",", "&#44;")},msg_resid=]");
                else if (e.Msg.StartsWith("[LQ:lightappelem,type=1,data=") && e.Msg.IndexOf(",msg_resid=]") > 0)
                    e.Reply(e.Msg.Between("[LQ:lightappelem,type=1,data=", ",msg_resid=]").Replace("&#44;", ","));
                else if (e.Msg.StartsWith("[LQ:richmsg") && e.Msg.IndexOf("]") > 0)
                    e.Reply(e.Msg.Trim('[', ']'));
                else if (e.Msg.StartsWith("LQ:richmsg"))
                    e.Reply($"[{e.Msg.UnEscape()}]");
                //else
                //Q2TG.Test(e);
            }
        }
        public static void GroupAdminAdded(GroupAdminChangedArgs e)
        {

        }

        public static void GroupAdminRemoved(GroupAdminChangedArgs e)
        {

        }

        public static void RequestAddFriend(RequestAddFriendArgs e)
        {
            e.Accept();
        }

        public static void RequestAddGroup(RequestGroupArgs e)
        {
            if (e.Group == G.major)
            {
                var msg = e.Msg.GetRight("答案：").Trim();
                User u = new User(e.FromQQ);
                C.Write("有人 ", ConsoleColor.DarkCyan);
                C.Write(e.FromQQ, ConsoleColor.Cyan);
                C.Write(" 加群 ", ConsoleColor.DarkCyan);
                C.Write(msg, ConsoleColor.Cyan);
                C.WriteLn("");
                if (u.Role == UserType.blackListed)
                {
                    e.Reject("blacklisted");
                    S.Si(u.ToXml(Strs.Get("addRejected", Strs.Get("blacklisted"))) + $"\n申请信息: {msg}");
                    return;
                }
                if (Rds.SContains("knownUsers", e.FromQQ))
                {
                    e.Accept();
                    S.Si(u.ToXml("加群申请已同意: 白名单用户") + $"\n申请信息: {msg}");
                    return;
                }

                if (msg.IndexOf(" ") < 0)
                {
                    //TODO 只填了姓名，但是姓名找得到可以同意
                    if (u.Verified && u.Name == msg)
                    {
                        e.Accept();
                        S.Si(u.ToXml("加群申请已同意: 已实名用户") + $"\n申请信息: {msg}");
                        return;
                    }
                    var chk2 = RealName.Check(msg);
                    if (chk2.OccupiedQQ != null && chk2.OccupiedQQ != e.FromQQ && chk2.Status != RealNameStatus.notFound)
                    {
                        e.Reject("此身份已有一个账户加入，如有疑问请联系管理员");
                        S.Si(u.ToXml("加群申请已拒绝: 此人已存在") + $"\n申请信息: {msg}");
                        return;
                    }
                    if (chk2.OccupiedQQ == null && chk2.Status != RealNameStatus.notFound)
                    {
                        //尝试进行验证
                        u.Name = msg;
                        if (u.Verified)
                        {
                            e.Accept();
                            S.Si(u.ToXml("加群申请已同意: 实名认证成功") + $"\n申请信息: {msg}");
                            return;
                        }
                        var err = u.VerifyMsg;
                        e.Reject("玄学错误，请联系管理员");
                        S.Si(u.ToXml("加群申请已拒绝: 玄学错误，此错误不应该由本段代码处理") + $"\n申请信息: {msg}\n未预期的错误: {err}");
                        return;
                    }

                    e.Reject(Strs.Get("formatIncorrect"));
                    S.Si(u.ToXml(Strs.Get("addRejected", Strs.Get("formatErr"))) + $"\n申请信息: {msg}");
                    return;
                }

                var enr = UserInfo.ParseEnrollment(msg.GetLeft(" "));
                var name = msg.GetRight(" ").Trim();

                if (enr < 1970)
                {
                    e.Reject(Strs.Get("EnrFormatErr"));
                    S.Si(u.ToXml(Strs.Get("addRejected", Strs.Get("EnrFormatErr"))) + $"\n申请信息: {msg}");
                    return;
                }

                u.Enrollment = enr;

                if (enr != 2017 && enr != 2018 && enr != 2019)
                {
                    u.Name = name;
                    u.Junior = UserInfo.ParseJunior(msg.GetLeft(" "));
                    S.Si(u.ToXml("此年级不支持自动审核") + $"\n申请信息: {msg}");
                    return;
                }

                if (u.Verified)
                {
                    if (u.Name != name)
                    {
                        e.Reject("已实名账户请用登记姓名加入，如有问题请联系管理员");
                        S.Si(u.ToXml("加群申请已拒绝: 已实名账户尝试 override") + $"\n申请信息: {msg}");
                        return;
                    }
                    e.Accept();
                    S.Si(u.ToXml("加群申请已同意: 已实名用户") + $"\n申请信息: {msg}");
                    return;
                }

                var chk = RealName.Check(name);

                if (chk.Status == RealNameStatus.notFound)
                {
                    e.Reject("查无此人");
                    S.Si(u.ToXml("加群申请已拒绝: 查无此人") + $"\n申请信息: {msg}");
                    return;
                }

                if (chk.OccupiedQQ != null && chk.OccupiedQQ != e.FromQQ)
                {
                    e.Reject("此身份已有一个账户加入，如有疑问请联系管理员");
                    S.Si(u.ToXml("加群申请已拒绝: 此人已存在") + $"\n申请信息: {msg}");
                    return;
                }

                if (chk.Status == RealNameStatus.e2017 && enr != 2017)
                {
                    e.Reject("姓名与年级不匹配，请检查");
                    S.Si(u.ToXml("加群申请已拒绝: 姓名与年级不匹配") + $"\n申请信息: {msg}");
                    return;
                }
                if (chk.Status == RealNameStatus.e2018 && enr != 2018)
                {
                    e.Reject("姓名与年级不匹配，请检查");
                    S.Si(u.ToXml("加群申请已拒绝: 姓名与年级不匹配") + $"\n申请信息: {msg}");
                    return;
                }
                if (chk.Status == RealNameStatus.e2018jc && enr != 2018)
                {
                    e.Reject("姓名与年级不匹配，请检查");
                    S.Si(u.ToXml("加群申请已拒绝: 姓名与年级不匹配") + $"\n申请信息: {msg}");
                    return;
                }
                if (chk.Status == RealNameStatus.e2019 && enr != 2019)
                {
                    e.Reject("姓名与年级不匹配，请检查");
                    S.Si(u.ToXml("加群申请已拒绝: 姓名与年级不匹配") + $"\n申请信息: {msg}");
                    return;
                }
                if (chk.Status == RealNameStatus.e2019jc && enr != 2019)
                {
                    e.Reject("姓名与年级不匹配，请检查");
                    S.Si(u.ToXml("加群申请已拒绝: 姓名与年级不匹配") + $"\n申请信息: {msg}");
                    return;
                }


                if (chk.OccupiedQQ == null)
                {
                    //尝试进行验证
                    u.Name = name;
                    u.Junior = UserInfo.ParseJunior(msg.GetLeft(" "));
                    if (u.Verified)
                    {
                        e.Accept();
                        S.Si(u.ToXml("加群申请已同意: 实名认证成功") + $"\n申请信息: {msg}");
                        return;
                    }
                    var err = u.VerifyMsg;
                    e.Reject("玄学错误，请联系管理员");
                    S.Si(u.ToXml("加群申请已拒绝: 玄学错误，此错误不应该由本段代码处理") + $"\n申请信息: {msg}\n未预期的错误: {err}");
                    return;
                }

                S.Si(u.ToXml("申请用户信息"));
            }
        }

        public static void RequestInviteGroup(RequestGroupArgs e)
        {
            e.Accept();
        }

        internal static void GroupAddMember(string group, string member)
        {
            if (group == G.major)
            {
                User u = new User(member);
                if (u.IsFresh)
                {
                    Setup.New(member, true);
                    return;
                }
                else
                {
                    u.Namecard = u.ProperNamecard;
                    u.Status = Status.no;
                    string tip = Strs.Get($"setupTip{u.Enrollment}");//根据年级来分的提示
                    if (tip != "")
                        S.Private(member, tip); //你是高一的，建议同时加入 2018 级新高一年级群 //??2019级新生即将来临?敬请期待??? 
                    tip = Strs.Get($"setupTipAll");//给所有人的广告位
                    if (tip != "")
                        S.Private(member, tip); //目前是这个：【西花园事务所】是江苏省苏州第十中学校学生自建生活服务平台，添加【西花园事务所】为特别关心，可以第一时间收到最新消息 
                    //S.Private(member, u.ToXml("欢迎回来"));
                    //S.Private(member, Strs.Get("welcomeBack"));//"如果以上信息有误，你可以回复 <setup> 来重新设置"
                    S.Si(u.ToXml("数据库存在此人信息，新人向导跳过"));
                    return;
                }
            }
        }

        internal static void GroupCardChanged(string group, string member, string card)
        {
            if (group == G.major)
            {
                UserInfo.CheckQmpAsync(new User(member));
            }
        }
    }
}
