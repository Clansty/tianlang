using System;
using System.Threading.Tasks;

namespace Clansty.tianlang
{
    static class Events
    {
        //想更新功能，自己加，发 pr
        internal static void Exit()
        {

        }
        internal static void Enable()
        {
            C.AllocConsole();
            Console.Title = $@"甜狼 {C.Version}";
            new Menu().Show();
            async void UpdateList()
            {
                await Task.Delay(2333);
                MemberList.UpdateMajor();
                C.WriteLn("Memberlist updated");
            }
            UserInfo.InitQmpCheckTask();
            UpdateList();
        }
        internal static void Disable()
        {

        }
        internal static void Msg(FriendMsgArgs e)
        {
            var u = new User(e.FromQQ);
            if (u.Status == Status.setup)
                Setup.Enter(e);
            else if (e.Msg == "whoami")
            {
                UserInfo.CheckQmpAsync(u);
                e.Reply(u.ToXml("你的信息"));
            }
            else if (e.Msg.StartsWith("我叫"))
            {
                //here, to line 93 handles manual name-filling event
                var name = e.Msg.GetRight("我叫").Trim();
                if (u.Verified)
                {
                    e.Reply(Strs.Get("rnVerified")); //你已经实名认证了，无需再次补填姓名
                    return;
                }

                if (u.VerifyMsg == RealNameVerifingResult.unsupported)
                {
                    //尝试进行验证
                    u.Name = name;
                    e.Reply(Strs.Get("rnOK")); // here set succeeded
                    //update in 3.0.14.2 notice the administration
                    S.Si(u.ToXml("旧人补填姓名成功"));
                    UserInfo.CheckQmpAsync(u);
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
                        e.Reply(Strs.Get("rnOK")); // here set succeeded
                        //update in 3.0.14.2 notice the administration
                        S.Si(u.ToXml("旧人补填姓名成功"));
                        return;
                    }

                    e.Reply(Strs.Get("unexceptedErr", u.VerifyMsg));
                    UserInfo.CheckQmpAsync(u);
                }
            } // end manual name-filling handling
        }
        internal static void Msg(GroupMsgArgs e)
        {
            if (e.FromGroup == G.si)
                Cmds.SiEnter(e);

            if (e.FromGroup == G.major)
            {
                //UserInfo.CheckQmpAsync(new User(e.FromQQ), e.Card);
                if (e.Msg.StartsWith("sudo "))
                    Cmds.SudoEnter(e);
                Repeater.Enter(e.Msg);
                if (C.recording)
                {
                    var u = new User(e.FromQQ);
                    if (u.Enrollment == 2020 || u.Enrollment == 2017 && u.Junior)
                    {
                        Rds.SAdd("elec2020", u.Uin);
                    }
                }
            }
        }
        internal static void AddFriend(RequestAddFriendArgs e)
        {
            e.Accept();
        }
        internal static void GroupAddMember(GroupAddMemberArgs e)
        {
            if (e.Group == G.major)
            {
                MemberList.major.Add(e.BeingOperateQQ);
                var u = new User(e.BeingOperateQQ);
                if (u.IsFresh)
                {
                    Setup.New(e.BeingOperateQQ, true);
                }
                else
                {
                    u.Namecard = u.ProperNamecard;
                    u.Status = Status.no;
                    var tip = Strs.Get($"setupTip{u.Enrollment}"); //根据年级来分的提示
                    if (tip != "")
                        S.Private(e.BeingOperateQQ, tip); //你是高一的，建议同时加入 2018 级新高一年级群 //??2019级新生即将来临?敬请期待??? 
                    tip = Strs.Get("setupTipAll"); //给所有人的广告位
                    if (tip != "")
                        S.Private(e.BeingOperateQQ, tip); //目前是这个：【西花园事务所】是江苏省苏州第十中学校学生自建生活服务平台，添加【西花园事务所】为特别关心，可以第一时间收到最新消息 
                    //S.Private(member, u.ToXml("欢迎回来"));
                    //S.Private(member, Strs.Get("welcomeBack"));//"如果以上信息有误，你可以回复 <setup> 来重新设置"
                    S.Si(u.ToXml("数据库存在此人信息，新人向导跳过"));
                    UserInfo.CheckQmpAsync(u);
                }
            }

            if (e.Group == G.g2020)
            {
                if (!MemberList.major.Contains(e.BeingOperateQQ))
                {
                    //这里由于不是大群成员所以需要以手动群临时方式发送
                    Robot.Send.Temp(G.g2020, e.BeingOperateQQ, $"看起来你还没有加入十中大群的说\n加入苏州十中跨年级大群 {G.major}，解锁更多好玩的");
                }
            }
        }
        internal static void JoinGroupRequest(RequestAddGroupArgs e)
        {
            if (e.Group == G.major)
            {
                var msg = e.Msg.Contains("答案：")
                    ? e.Msg.GetRight("答案：").Trim()
                    : e.Msg;
                var u = new User(e.QQ);
                C.Write("有人 ", ConsoleColor.DarkCyan);
                C.Write(e.QQ, ConsoleColor.Cyan);
                C.Write(" 加群 ", ConsoleColor.DarkCyan);
                C.Write(msg, ConsoleColor.Cyan);
                C.WriteLn("");
                if (u.Role == UserType.blackListed)
                {
                    e.Reject("blacklisted");
                    S.Si(u.ToXml(Strs.Get("addRejected", Strs.Get("blacklisted"))) + $"\n申请信息: {msg}");
                    return;
                }

                if (msg.IndexOf(" ", StringComparison.Ordinal) < 0)
                {
                    //TODO 只填了姓名，但是姓名找得到可以同意
                    if (u.VerifyMsg == RealNameVerifingResult.succeed && u.Name == msg)
                    {
                        e.Accept();
                        S.Si(u.ToXml("加群申请已同意: 已实名用户") + $"\n申请信息: {msg}");
                        return;
                    }

                    var chk2 = RealName.Check(msg);
                    if (chk2.OccupiedQQ != null && chk2.OccupiedQQ != e.QQ &&
                        chk2.Status != RealNameStatus.notFound)
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
                    e.Reject(
                        Strs.Get("EnrFormatErr"));
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
                        e.Reject(
                            "已实名账户请用登记姓名加入，如有问题请联系管理员");
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

                if (chk.OccupiedQQ != null && chk.OccupiedQQ != e.QQ)
                {
                    e.Reject(
                        "此身份已有一个账户加入，如有疑问请联系管理员");
                    S.Si(u.ToXml("加群申请已拒绝: 此人已存在") + $"\n申请信息: {msg}");
                    return;
                }

                #region
                //if (chk.Status == RealNameStatus.e2017 && enr != 2017)
                //{
                //    e.Reject("姓名与年级不匹配，请检查");
                //    S.Si(u.ToXml("加群申请已拒绝: 姓名与年级不匹配") + $"\n申请信息: {msg}");
                //    return;
                //}
                //if (chk.Status == RealNameStatus.e2018 && enr != 2018)
                //{
                //    e.Reject("姓名与年级不匹配，请检查");
                //    S.Si(u.ToXml("加群申请已拒绝: 姓名与年级不匹配") + $"\n申请信息: {msg}");
                //    return;
                //}
                //if (chk.Status == RealNameStatus.e2018jc && enr != 2018)
                //{
                //    e.Reject("姓名与年级不匹配，请检查");
                //    S.Si(u.ToXml("加群申请已拒绝: 姓名与年级不匹配") + $"\n申请信息: {msg}");
                //    return;
                //}
                //if (chk.Status == RealNameStatus.e2019 && enr != 2019)
                //{
                //    e.Reject("姓名与年级不匹配，请检查");
                //    S.Si(u.ToXml("加群申请已拒绝: 姓名与年级不匹配") + $"\n申请信息: {msg}");
                //    return;
                //}
                //if (chk.Status == RealNameStatus.e2019jc && enr != 2019)
                //{
                //    e.Reject("姓名与年级不匹配，请检查");
                //    S.Si(u.ToXml("加群申请已拒绝: 姓名与年级不匹配") + $"\n申请信息: {msg}");
                //    return;
                //}
                #endregion

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
                    e.Reject(
                        "玄学错误，请联系管理员");
                    S.Si(u.ToXml("加群申请已拒绝: 玄学错误，此错误不应该由本段代码处理\n" +
                                 $"申请信息: {msg}\n未预期的错误: {err}"));
                    return;
                }

                S.Si(u.ToXml("申请用户信息"));
            }


        }
        internal static void InviteGroupRequest(RequestAddGroupArgs e)
        {
            e.Accept();
        }
    }
}
