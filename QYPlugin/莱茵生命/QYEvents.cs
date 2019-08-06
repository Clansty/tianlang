using System;

namespace Clansty.tianlang
{
    public static class PluginInfo //插件的相关信息
    {
        public const string name = "甜狼 v3"; //名称
        public const string appid = "Clansty.tianlang.v3"; //标识
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
            TG.InitAsync();
        }
        public static void FriendMsg(FriendMsgArgs e)
        {
            User u = new User(e.FromQQ);
            if (u.Status == Status.setup)
                Setup.Enter(e);
            if (e.Msg == "setup")
                Setup.New(e.FromQQ, false);
            if (e.Msg == "whoami")
                e.Reply(u.ToXml("你的信息"));
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
                if (e.Msg.StartsWith("sudo "))
                    Cmds.SudoEnter(e);
                Repeater.Enter(e.Msg);
                Stats.New(e);
            }

            if (e.FromGroup == G.iDE)
            {
                Q2TG.IDE(e);
            }

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
                else
                    Q2TG.Test(e);
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
                User u = new User(e.FromQQ);
                C.Write("有人 ", ConsoleColor.DarkCyan);
                C.Write(e.FromQQ, ConsoleColor.Cyan);
                C.Write(" 加群 ", ConsoleColor.DarkCyan);
                C.Write(e.Msg, ConsoleColor.Cyan);
                C.WriteLn("");
                if (u.Role == UserType.blackListed)
                {
                    e.Reject("blacklisted");
                    S.Si(u.ToXml("加群申请已拒绝: 黑名单用户"));
                    return;
                }
                // TODO: 实名验证审核
                if (e.Msg.IndexOf(" ") < 0)
                {
                    e.Reject("请按正确格式填写");
                    S.Si(u.ToXml("加群申请已拒绝: 格式错误"));
                    return;
                }

                var enr = UserInfo.ParseEnrollment(e.Msg.GetLeft(" "));
                var name = e.Msg.GetRight(" ").Trim();

                if (u.Enrollment > 1970 && enr != u.Enrollment)
                {
                    e.Reject("年级与登记的不匹配，请联系管理员");
                    S.Si(u.ToXml("加群申请已拒绝: 年级与登记的不匹配") + $"\n申请信息: {e.Msg}");
                    return;
                }

                u.Enrollment = enr;

                if (enr != 2017 && enr != 2018)
                {                    
                    u.Name = name;
                    u.Junior = UserInfo.ParseJunior(e.Msg.GetLeft(" "));
                    S.Si(u.ToXml("非高二高三请手动审核"));
                    return;
                }

                if (u.Verified)
                {
                    if (u.Name != name)
                    {
                        e.Reject("已实名账户请用登记姓名加入，如有问题请联系管理员");
                        S.Si(u.ToXml("加群申请已拒绝: 已实名账户尝试 override") + $"\n申请信息: {e.Msg}");
                        return;
                    }
                    e.Accept();
                    S.Si(u.ToXml("加群申请已同意: 已实名用户"));
                    return;
                }

                var chk = RealName.Check(name);

                if(chk.Status == RealNameStatus.notFound)
                {
                    e.Reject("查无此人");
                    S.Si(u.ToXml("加群申请已拒绝: 查无此人") + $"\n申请信息: {e.Msg}");
                    return;
                }
                
                if(chk.OccupiedQQ!=null && chk.OccupiedQQ != e.FromQQ)
                {
                    e.Reject("此身份已有一个账户加入，如有疑问请联系管理员");
                    S.Si(u.ToXml("加群申请已拒绝: 此人已存在") + $"\n申请信息: {e.Msg}");
                    return;
                }

                if (chk.OccupiedQQ == null)
                {
                    //尝试进行验证
                    u.Name = name;
                    u.Junior = UserInfo.ParseJunior(e.Msg.GetLeft(" "));
                    if (u.Verified)
                    {
                        e.Accept();
                        S.Si(u.ToXml("加群申请已同意: 实名认证成功"));
                        return;
                    }
                    var err = u.VerifyMsg;
                    if(err== RealNameVerifingResult.unmatch)
                    {
                        e.Reject("姓名与年级不匹配，请检查");
                        S.Si(u.ToXml("加群申请已拒绝: 姓名与年级不匹配") + $"\n申请信息: {e.Msg}");
                        return;
                    }
                    e.Reject("玄学错误，请联系管理员");
                    S.Si(u.ToXml("加群申请已拒绝: 玄学错误，此错误不应该由本段代码处理") + $"\n申请信息: {e.Msg}\n未预期的错误: {err}");
                    return;
                }

                if (Rds.SContains("knownUsers", e.FromQQ))
                {
                    e.Accept();
                    S.Si(u.ToXml("加群申请已同意: 已知用户"));
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
                    S.Private(member, u.ToXml("欢迎回来"));
                    S.Private(member, Strs.Get("welcomeBack"));//"如果以上信息有误，你可以回复 <setup> 来重新设置"
                    S.Si(u.ToXml("来了个旧人"));
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
