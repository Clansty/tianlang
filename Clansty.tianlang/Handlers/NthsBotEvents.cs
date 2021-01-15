using System;
using System.Threading.Tasks;
using Mirai_CSharp;
using Mirai_CSharp.Extensions;
using Mirai_CSharp.Models;
using Mirai_CSharp.Plugin.Interfaces;

namespace Clansty.tianlang
{
    class NthsBotEvents : IFriendMessage, ITempMessage, IGroupMessage, INewFriendApply, IGroupApply,
        IBotInvitedJoinGroup, IDisconnected, IGroupMemberJoined
    {
#if DEBUG
        internal const long SELF = 2981882373;
#else
        internal const long SELF = 1980853671;
#endif
        public async Task<bool> TempMessage(MiraiHttpSession session, ITempMessageEventArgs e)
        {
            await PrivateMessage(new PrivateMsgArgs
            {
                FromQQ = e.Sender.Id,
                FromNick = e.Sender.Name,
                Chain = e.Chain,
                Robot = session
            });
            return true;
        }

        public async Task<bool> FriendMessage(MiraiHttpSession session, IFriendMessageEventArgs e)
        {
            await PrivateMessage(new PrivateMsgArgs
            {
                FromQQ = e.Sender.Id,
                FromNick = e.Sender.Name,
                Chain = e.Chain,
                Robot = session
            });
            return true;
        }

        private static async Task PrivateMessage(PrivateMsgArgs e)
        {
            var u = new User(e.FromQQ);
            if (u.Status == Status.setup)
                Setup.Enter(e);
            else if (e.Chain.GetPlain().StartsWith("我叫"))
            {
                //here, to line 93 handles manual name-filling event
                var name = e.Chain.GetPlain().GetRight("我叫").Trim();
                if (u.Verified)
                {
                    e.Reply(Strs.RnVerified); //你已经实名认证了，无需再次补填姓名
                    return;
                }

                if (u.VerifyMsg == VerifingResult.unsupported)
                {
                    //尝试进行验证
                    u.Name = name;
                    e.Reply(Strs.RnOK);
                    S.Si(u.ToString(Strs.NameFillInSucceeded));
                    UserInfo.CheckQmpAsync(u);
                    return;
                }

                try
                {
                    var p = Person.Get(name);
                    if (p.User is null)
                    {
                        //尝试进行验证
                        u.Name = name;
                        if (u.Verified)
                        {
                            e.Reply(Strs.RnOK);
                            S.Si(u.ToString(Strs.NameFillInSucceeded));
                            return;
                        }

                        e.Reply(Strs.UnexceptedErr + u.VerifyMsg);
                        UserInfo.CheckQmpAsync(u);
                        return;
                    }

                    e.Reply(Strs.RnOccupied);
                }
                catch (PersonNotFoundException)
                {
                    e.Reply(Strs.RnNotFound);
                }
                catch (Exception ex)
                {
                    C.WriteLn(ex);
                    e.Reply(Strs.UnexceptedErr);
                }
            } // end manual name-filling handling
            else if (e.Chain.GetPlain() == "tg")
            {
                TgBinding.Init(u);
            }
            else if (e.Chain.GetPlain().StartsWith("你已经绑定了一个 Telegram 账号"))
            {
                TgBinding.Init(u, true);
            }
            else if (e.FromQQ == 839827911 && e.Chain.GetPlain() == "update")
            {
                var ret = Cmds.gcmds["update"].Func(null);
                e.Reply(ret);
            }
        }

        public async Task<bool> GroupMessage(MiraiHttpSession session, IGroupMessageEventArgs e)
        {
            C.WriteLn(e.Sender.Group.Id+", "+e.Sender.Id);
            foreach (var i in e.Chain)
            {
                C.Write(i);
            }
            
            Q2Tg.NewGroupMsg(
                session,
                e.Sender.Group.Id,
                e.Sender.Id,
                e.Sender.Name,
                e.Chain
            );

            if (e.Chain.GetPlain().StartsWith("点歌"))
                NetEase.Request(e);

            if (e.Sender.Group.Id == G.check)
            {
                Watchdog.Msg(SELF, e.Sender.Id, e.Chain.GetPlain());
            }

            if (e.Sender.Group.Id == G.si)
                Cmds.Enter(e.Chain.GetPlain(), e.Sender.Id, false);

            if (e.Sender.Group.Id == G.major)
            {
                UserInfo.CheckQmpAsync(new User(e.Sender.Id), e.Sender.Name);
                if (e.Chain.GetPlain().StartsWith("sudo "))
                    Cmds.Enter(e.Chain.GetPlain().GetRight("sudo "), e.Sender.Id, true);
                Repeater.Enter(e.Chain.GetPlain());
            }

            return true;
        }

        public async Task<bool> NewFriendApply(MiraiHttpSession session, INewFriendApplyEventArgs e)
        {
            session.HandleNewFriendApplyAsync(e, FriendApplyAction.Allow);
            return true;
        }

        public async Task<bool> GroupMemberJoined(MiraiHttpSession session, IGroupMemberJoinedEventArgs e)
        {
            if (e.Member.Group.Id == G.major)
            {
                MemberList.major.Add(e.Member.Id);
                var u = new User(e.Member.Id);
                UserInfo.CheckQmpAsync(u);
                if (u.IsFresh)
                {
                    Setup.New(e.Member.Id, true);
                }
                else
                {
                    u.Namecard = u.ProperNamecard;
                    u.Status = Status.no;
                    S.Si(u.ToString(Strs.WizardSkip));
                }
            }

            if (e.Member.Group.Id == G.g2020)
            {
                if (!MemberList.major.Contains(e.Member.Id))
                {
                    //这里由于不是大群成员所以需要以手动群临时方式发送
                    session.SendTempMessageAsync(e.Member.Id, G.g2020, Strs.InviteMajor.MakeChain());
                }
            }

            return true;
        }

        public async Task<bool> GroupApply(MiraiHttpSession session, IGroupApplyEventArgs e)
        {
            if (e.FromGroup == G.major)
            {
                var msg = e.Message.Contains("答案：")
                    ? e.Message.GetRight("答案：").Trim()
                    : e.Message;
                if (msg.Contains('['))
                    msg = msg.GetLeft("[");
                var u = new User(e.FromQQ);
                C.Write("有人 ", ConsoleColor.DarkCyan);
                C.Write(e.FromQQ, ConsoleColor.Cyan);
                C.Write(" 加群 ", ConsoleColor.DarkCyan);
                C.Write(msg, ConsoleColor.Cyan);
                C.WriteLn("");
                if (u.Role == UserType.blackListed)
                {
                    session.HandleGroupApplyAsync(e, GroupApplyActions.Deny, "blacklisted");
                    S.Si(u.ToString(Strs.AddRejected + Strs.Blacklisted) + $"\n申请信息: {msg}");
                    return true;
                }

                if (u.VerifyMsg == VerifingResult.succeed) //本身就实名好了，填什么都给进
                {
                    session.HandleGroupApplyAsync(e, GroupApplyActions.Allow);
                    S.Si(u.ToString(Strs.AddAccepted + Strs.VerifiedUser) + $"\n申请信息: {msg}");
                    return true;
                }

                string name;
                int enr;
                //现在这样，直接把姓名赋值给 u.Name，要是 succeed 就给进，occupied notFound unsupported 都不给进
                if (msg.IndexOf(" ", StringComparison.Ordinal) < 0) //没有空格的情况，20200813 直接用 parseNick 解析吧
                {
                    enr = UserInfo.ParseEnrollment(msg);
                    name = UserInfo.ParseNick(msg);
                }
                else
                {
                    enr = UserInfo.ParseEnrollment(msg.GetLeft(" "));
                    name = msg.GetRight(" ").Trim();
                }

                //其实现在有没有空格都是一样的了，就判断那些字段有没有
                if (string.IsNullOrWhiteSpace(name))
                {
                    session.HandleGroupApplyAsync(e, GroupApplyActions.Deny, Strs.FormatIncorrect);
                    S.Si(u.ToString(Strs.AddRejected + Strs.FormatErr) + $"\n申请信息: {msg}");
                    return true;
                }

                //emptyName 这里已经处理过了接下来不会有
                if (enr > 2000)
                    u.Enrollment = enr;
                u.Name = name;

                #region 这三种情况都是人在受支持的年级的

                if (u.VerifyMsg == VerifingResult.succeed)
                {
                    session.HandleGroupApplyAsync(e, GroupApplyActions.Allow);
                    S.Si(u.ToString(Strs.AddAccepted + Strs.RnOK) + $"\n申请信息: {msg}");
                    return true;
                }

                if (u.VerifyMsg == VerifingResult.occupied)
                {
                    session.HandleGroupApplyAsync(e, GroupApplyActions.Deny, Strs.JoinChkOccupied);
                    S.Si(u.ToString(Strs.AddRejected + Strs.AddReqOccupied) + $"\n申请信息: {msg}");
                    return true;
                }

                if (u.VerifyMsg == VerifingResult.notFound)
                {
                    session.HandleGroupApplyAsync(e, GroupApplyActions.Deny, Strs.PersonNotFound);
                    S.Si(u.ToString(Strs.AddRejected + Strs.PersonNotFound) + $"\n申请信息: {msg}");
                    return true;
                }

                #endregion

                //接下来是 unsupported 再做细分
                if (u.Enrollment < 2000)
                {
                    session.HandleGroupApplyAsync(e, GroupApplyActions.Deny, Strs.EnrFormatErr);
                    S.Si(u.ToString(Strs.AddRejected + Strs.EnrFormatErr) + $"\n申请信息: {msg}");
                    return true;
                }

                //由于年级受支持的一定会返回上面三种情况，所以这里不用判断一定是年级不受支持的
                u.Junior = UserInfo.ParseJunior(msg.GetLeft(" "));
                S.Si(u.ToString(Strs.EnrUnsupported) + $"\n申请信息: {msg}");
                return true;
            }

            return true;
        }

        public async Task<bool> BotInvitedJoinGroup(MiraiHttpSession session, IBotInvitedJoinGroupEventArgs e)
        {
            session.HandleBotInvitedJoinGroupAsync(e, GroupApplyActions.Allow);
            return false;
        }

        public async Task<bool> Disconnected(MiraiHttpSession session, IDisconnectedEventArgs e)
        {
            while (true)
            {
                try
                {
                    await session.ConnectAsync(C.miraiSessionOpinions, SELF);
                    return true;
                }
                catch (Exception)
                {
                    await Task.Delay(1000);
                }
            }
        }
    }
}