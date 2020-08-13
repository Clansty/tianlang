using LemonApp;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Clansty.tianlang
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
                case 4:
                    //好友消息
                    Events.Msg(new FriendMsgArgs(sq, msg));
                    break;
                case 2:
                    //群消息
                    Events.Msg(new GroupMsgArgs(sq, g, msg));
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
            Events.GroupAddMember(new GroupAddMemberArgs(d, e, f));
            return 0;
        }
        [DllExport(CallingConvention.StdCall)]
        static int _eventRequest_AddGroup(long a, int b, string c, int d, long e, long f, long g, string h, int i)
        {
            C.WriteLn(b.ToString());
            if (b == 104 || b == 102)
            {
                Events.JoinGroupRequest(new RequestAddGroupArgs(c, e, f, i, h));
            }
            if (b == 105)
            {
                Events.InviteGroupRequest(new RequestAddGroupArgs(c, e, f, i, h));
            }
            return 0;
        }
        [DllExport(CallingConvention.StdCall)]
        static int _eventTipsMsg(long a, int b, long c, long d, long e, string f)
        {
            if (b == 112)
            {
#if DEBUG            
                C.WriteLn($"{a}\n{c}\n{d}\n{e}\n{f}\n");            
#else
                Events.GroupCardChanged(new GroupCardChangedArgs(c, d, f));
#endif
            }
            return 0;
        }

        static class Send
        {
            internal static void Group(long group, string msg)
            {
                Api_sendGroupMsg(ac, tianlang.AppInfo.self, group, msg);
            }
            [DllImport("LqrHelper.dll")]
            extern static void Api_sendGroupMsg(int a, long b, long c, string d);
            internal static void Temp(long group, long qq, string msg)
            {
                Api_sendTransieMsg(ac, tianlang.AppInfo.self, group, msg, qq);
            }
            [DllImport("LqrHelper.dll")]
            extern static void Api_sendTransieMsg(int a, long b, long c, string d, long e);
            internal static void Friend(long qq, string msg)
            {
                Temp(G.major, qq, msg);
            }
        }

        internal static void GroupKickMember(long group, long qq)
        {
            Api_KickGroupMember(Clansty.tianlang.AppInfo.self, group, qq, false);
        }
        [DllImport("LqrHelper.dll")]
        extern static void Api_KickGroupMember(long a, long b, long c, bool d);
        internal static string GetGroupMemberCard(long group, long qq)
        {
            //&nbsp;
            var r = Marshal.PtrToStringAnsi(Api_GetGroupMemberCard(tianlang.AppInfo.self, group, qq));
            r = r.Replace("&nbsp;", " ");//nbsp 和空格是有区别的
            r = HttpUtility.HtmlDecode(r);
            return r.Replace("\\/", "/");
        }

        [DllImport("LqrHelper.dll")]
        extern static IntPtr Api_GetGroupMemberCard(long a, long b, long c);
        internal static void SetGroupMemberCard(long group, long qq, string card)
        {
            Api_SetGroupMemberCard(Clansty.tianlang.AppInfo.self, group, qq, card);
        }
        [DllImport("LqrHelper.dll")]
        extern static void Api_SetGroupMemberCard(long a, long b, long c, string d);
        internal static List<long> GetGroupMembers(long group)
        {
            var json = Marshal.PtrToStringAnsi(Api_GetGroupMemberList(Clansty.tianlang.AppInfo.self, group));
            var jobj = JObject.Parse(json);
            var members = jobj.SelectToken("members");
            var dic = members.ToObject<Dictionary<string, object>>();
            var keys = dic.Keys;
            var ret = new List<long>();
            foreach (var i in keys)
                ret.Add(long.Parse(i));
            return ret;
        }
        [DllImport("LqrHelper.dll")]
        extern static IntPtr Api_GetGroupMemberList(long a, long b);
        internal static string GetNick(long qq)
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            var json = client.DownloadString($"https://api.vvhan.com/api/qq?qq={qq}");
            client.Dispose();
            var jo = JObject.Parse(json);
            return jo.Value<string>("name");
        }
    }
}
