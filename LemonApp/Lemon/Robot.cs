using System.Runtime.InteropServices;
using System.Web.Script.Serialization;

namespace LemonApp
{
    static class Robot
    {
        static int ac = 0;
        [DllExport(CallingConvention.StdCall)]
        static string AppInfo() => new JavaScriptSerializer().Serialize(new AppInfo());
        [DllExport(CallingConvention.StdCall)]
        static int Initialize(int acc)
        {
            ac = acc;
            return 0;
        }
        [DllExport(CallingConvention.StdCall)]
        static int _eventExit()
        {
            Events.Exit();
            return 0;
        }
        [DllExport(CallingConvention.StdCall)]
        static int _eventEnable()
        {
            Events.Enable();
            return 0;
        }
        [DllExport(CallingConvention.StdCall)]
        static int _eventDisable()
        {
            Events.Disable();
            return 0;
        }
        [DllExport(CallingConvention.StdCall)]
        static int _eventRecvMsg(long recvQQ, string msgId, string msgse, int t, int type, long g, long sq, string msg)
        {
            if (recvQQ == sq)
                return 0;
            switch (type)
            {
                case 1:
                    //好友消息
                    Events.Msg(new FriendMsgArgs(sq, msg));
                    break;
                case 2:
                    //群消息
                    Events.Msg(new GroupMsgArgs(sq, g, msg));
                    break;
                case 3:
                    //群临时
                    Events.Msg(new TempMsgArgs(sq, g, msg));
                    break;
            }
            return 0;
        }
        [DllExport(CallingConvention.StdCall)]
        static int _eventRequest_AddFriend(long qqid, int st, int q, string msg)
        {
            Events.AddFriend(new RequestAddFriendArgs(q, msg));
            return 0;
        }
        [DllExport(CallingConvention.StdCall)]
        static int _eventSystem_GroupMemberIncrease(long a, int b, int c, long d, long e, long f)
        {
            Events.GroupAddMember(new GroupAddMemberArgs(d.ToString(), e.ToString(), f.ToString()));
            return 0;
        }
        [DllExport(CallingConvention.StdCall)]
        static int _eventRequest_AddGroup(long a, int b, string c, int d, long e, long f, long g, string h, int i)
        {
            if (b == 104)
            {
                Events.JoinGroupRequest(new RequestAddGroupArgs(c, e, f, i, h));
            }
            if (b == 105)
            {
                Events.InviteGroupRequest(new RequestAddGroupArgs(c, e, f, i, h));
            }
            return 0;
        }

        public static class Send
        {
            public static void Group(string group, string msg)
            {
                Api_sendGroupMsg(ac, LemonApp.AppInfo.self, long.Parse(group), msg);
            }
            [DllImport("LqrHelper.dll")]
            extern static void Api_sendGroupMsg(int a, long b, long c, string d);
            public static void Temp(string group,string qq,string msg)
            {
                Api_sendTransieMsg(ac, LemonApp.AppInfo.self, long.Parse(group), msg, long.Parse(qq));
            }
            [DllImport("LqrHelper.dll")]
            extern static void Api_sendTransieMsg(int a, long b, long c, string d, long e);
            public static void Friend(string qq, string msg)
            {
                Api_sendPrivateMsg(ac, LemonApp.AppInfo.self, long.Parse(qq), msg);
            }
            [DllImport("LqrHelper.dll")]
            extern static void Api_sendPrivateMsg(int a, long b, long c, string d);
        }
    }
}
