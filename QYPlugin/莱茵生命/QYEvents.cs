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
                if (u.Role == UserType.blackListed)
                {
                    e.Reject("blacklisted");
                    S.Si(u.ToXml("加群申请已拒绝: 黑名单用户"));
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
