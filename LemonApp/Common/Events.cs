using System;

namespace Clansty.tianlang
{
    static class Events
    {
        internal static void Exit()
        {
            Sql.Commit();
        }
        internal static void Enable()
        {
            C.AllocConsole();
            Console.Title = $@"甜狼 {C.Version}";
            new Menu().Show();
        }
        internal static void Disable()
        {
            Sql.Commit();
        }
        internal static void Msg(FriendMsgArgs e)
        {
#if DEBUG
#else
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
                    e.Reply(Strs.RnVerified); //你已经实名认证了，无需再次补填姓名
                    return;
                }

                if (u.VerifyMsg == RealNameVerifingResult.unsupported)
                {
                    //尝试进行验证
                    u.Name = name;
                    e.Reply(Strs.RnOK); // here set succeeded
                    //update in 3.0.14.2 notice the administration
                    S.Si(u.ToXml("旧人补填姓名成功"));
                    UserInfo.CheckQmpAsync(u);
                    return;
                }

                var chk = RealName.Check(name);

                if (chk.Status == RealNameStatus.notFound)
                {
                    e.Reply(Strs.RnNotFound);
                    return;
                }

                if (chk.OccupiedQQ != null && chk.OccupiedQQ != e.FromQQ)
                {
                    e.Reply(Strs.RnOccupied);
                    return;
                }

                if (chk.OccupiedQQ == null)
                {
                    //尝试进行验证
                    u.Name = name;
                    if (u.Verified)
                    {
                        e.Reply(Strs.RnOK); // here set succeeded
                        //update in 3.0.14.2 notice the administration
                        S.Si(u.ToXml("旧人补填姓名成功"));
                        return;
                    }

                    e.Reply(Strs.UnexceptedErr + u.VerifyMsg);
                    UserInfo.CheckQmpAsync(u);
                }
            } // end manual name-filling handling
#endif
        }

        internal static void GroupCardChanged(GroupCardChangedArgs e)
        {
            if (e.Group == G.major)
            {
                UserInfo.CheckQmpAsync(new User(e.QQ), e.NewCard);
            }
        }

        internal static void Msg(GroupMsgArgs e)
        {
#if DEBUG
#else
            if (e.FromGroup == G.si)
                Cmds.SiEnter(e);

            if (e.FromGroup == G.major)
            {
                //UserInfo.CheckQmpAsync(new User(e.FromQQ), e.Card);
                if (e.Msg.StartsWith("sudo "))
                    Cmds.SudoEnter(e);
                Repeater.Enter(e.Msg);
            }
#endif
        }
        internal static void AddFriend(RequestAddFriendArgs e)
        {
            e.Accept();
        }
        internal static void GroupAddMember(GroupAddMemberArgs e)
        {
#if DEBUG
#else
            if (e.Group == G.major)
            {
                MemberList.major.Add(e.BeingOperateQQ);
                var u = new User(e.BeingOperateQQ);
                UserInfo.CheckQmpAsync(u);
                if (u.IsFresh)
                {
                    Setup.New(e.BeingOperateQQ, true);
                }
                else
                {
                    u.Namecard = u.ProperNamecard;
                    u.Status = Status.no;
                    S.Si(u.ToXml(Strs.WizardSkip));
                }
            }

            if (e.Group == G.g2020)
            {
                if (!MemberList.major.Contains(e.BeingOperateQQ))
                {
                    //这里由于不是大群成员所以需要以手动群临时方式发送
                    Robot.Send.Temp(G.g2020, e.BeingOperateQQ, Strs.InviteMajor);
                }
            }
#endif
        }
        internal static void JoinGroupRequest(RequestAddGroupArgs e)
        {
#if DEBUG
#else
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
                    S.Si(u.ToXml(Strs.AddRejected + Strs.Blacklisted) + $"\n申请信息: {msg}");
                    return;
                }

                if (msg.IndexOf(" ", StringComparison.Ordinal) < 0)
                {
                    //TODO 只填了姓名，但是姓名找得到可以同意
                    if (u.VerifyMsg == RealNameVerifingResult.succeed && u.Name == msg)
                    {
                        e.Accept();
                        S.Si(u.ToXml(Strs.AddAccepted + "已实名用户") + $"\n申请信息: {msg}");
                        return;
                    }

                    var chk2 = RealName.Check(msg);
                    if (chk2.OccupiedQQ != null && chk2.OccupiedQQ != e.QQ &&
                        chk2.Status != RealNameStatus.notFound)
                    {
                        e.Reject(Strs.Occupied);
                        S.Si(u.ToXml(Strs.AddRejected + Strs.AddReqOccupied) + $"\n申请信息: {msg}");
                        return;
                    }

                    if (chk2.OccupiedQQ == null && chk2.Status != RealNameStatus.notFound)
                    {
                        //尝试进行验证
                        u.Name = msg;
                        if (u.Verified)
                        {
                            e.Accept();
                            S.Si(u.ToXml(Strs.AddAccepted+Strs.RnOK) + $"\n申请信息: {msg}");
                            return;
                        }

                        var err = u.VerifyMsg;
                        e.Reject(Strs.UnexceptedErr);
                        S.Si(u.ToXml(Strs.AddRejected+Strs.UnexceptedErr) + $"\n申请信息: {msg}\n未预期的错误: {err}");
                        return;
                    }

                    e.Reject(Strs.FormatIncorrect);
                    S.Si(u.ToXml(Strs.AddRejected+ Strs.FormatErr) + $"\n申请信息: {msg}");
                    return;
                }

                var enr = UserInfo.ParseEnrollment(msg.GetLeft(" "));
                var name = msg.GetRight(" ").Trim();

                if (enr < 1970)
                {
                    e.Reject(
                        Strs.EnrFormatErr);
                    S.Si(u.ToXml(Strs.AddRejected+ Strs.EnrFormatErr) + $"\n申请信息: {msg}");
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
#endif
        }
        internal static void InviteGroupRequest(RequestAddGroupArgs e)
        {
            e.Accept();
        }
    }
}
