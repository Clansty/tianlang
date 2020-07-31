﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

// 建议用户不要更改此文件，除非你确信你看得懂这里面所有代码并且你的操作没有问题
namespace Clansty.tianlang
{
    /// <summary>
    /// 执行 QY 机器人相关操作
    /// </summary>
    public static class Robot
    {
        private static class DllEvents
        {
            [DllExport(CallingConvention.StdCall)]
            public static int _eventPrivateMsg(long QQID, int subType, long sendTime, long fromQQ, long fromID, string fromInfo, string msg, string info, int test)
            {
                if (QQID != LongQQ) return 0;
                QYEvents.FriendMsg(new FriendMsgArgs(fromQQ, msg));
                return 0;
            }
            [DllExport(CallingConvention.StdCall)]
            public static int _eventGroupMsg(long QQID, int subType, long sendTime, long fromGroup, long fromQQ, string fromInfo, string msg, string info, int test)
            {
                if (QQID != LongQQ) return 0;
                Dictionary<string, string> ifo = InfoParse(fromInfo);

                Unpack u = new Unpack(ifo["GExtraInfo"]);
                string nick = u.NextStr;
                string gc = u.NextStr;
                u.Skip(12);
                string title = u.NextStr;
                u.Dispose();
                QYEvents.GroupMsg(new GroupMsgArgs(fromQQ, fromGroup, msg, ifo["srcmsg"].ToString(), gc == "" ? nick : gc, title, ifo["MsgInfo"], AuthCode));

                return 0;
            }
            [DllExport(CallingConvention.StdCall)]
            public static int _eventPushMsgEvent(long QQID, int subType, long sendTime, long fromID, long fromQQ, long fromQQID, string msg, string info, int test)
            {
                if (QQID != LongQQ) return 0;
                if (subType == 1)
                    QYEvents.GroupCardChanged(fromID.ToString(), fromQQ.ToString(), msg);
                return 0;
            }
            [DllExport(CallingConvention.StdCall)]
            public static int _eventSystem_GroupAdmin(long QQID, int subType, long sendTime, long fromGroup, long beingOperateQQ)
            {
                if (QQID != LongQQ) return 0;
                switch (subType)
                {
                    case 1:
                        QYEvents.GroupAdminRemoved(new GroupAdminChangedArgs(fromGroup, beingOperateQQ));
                        break;
                    case 2:
                        QYEvents.GroupAdminAdded(new GroupAdminChangedArgs(fromGroup, beingOperateQQ));
                        break;
                }
                return 0;
            }
            [DllExport(CallingConvention.StdCall)]
            public static int _eventSystem_GroupMemberDecrease(long QQID, int subType, long sendTime, long fromGroup, long fromQQ, long beingOperateQQ)
            {
                return 0;
            }
            [DllExport(CallingConvention.StdCall)]
            public static int _eventSystem_GroupMemberIncrease(long QQID, int subType, long sendTime, long fromGroup, long fromQQ, long beingOperateQQ)
            {
                if (QQID != LongQQ) return 0;
                QYEvents.GroupAddMember(fromGroup.ToString(), beingOperateQQ.ToString());
                return 0;
            }
            [DllExport(CallingConvention.StdCall)]
            public static int _eventFriendEvent(long QQID, int subType, long sendTime, long fromQQ, string info)
            {
                return 0;
            }
            [DllExport(CallingConvention.StdCall)]
            public static int _eventRequest_AddFriend(long QQID, int subType, long sendTime, long fromQQ, string source, string msg, string responseFlag)
            {
                if (QQID != LongQQ) return 0;
                QYEvents.RequestAddFriend(new RequestAddFriendArgs(fromQQ, msg, responseFlag, AuthCode));
                return 0;
            }
            [DllExport(CallingConvention.StdCall)]
            public static int _eventRequest_AddGroup(long QQID, int subType, long sendTime, long fromGroup, long fromQQ, string source, string msg, string responseFlag)
            {
                if (QQID != LongQQ) return 0;
                switch (subType)
                {
                    case 1:
                    case 2:
                        QYEvents.RequestAddGroup(new RequestGroupArgs(fromGroup, msg, responseFlag, AuthCode, fromQQ.ToString(), subType));
                        break;
                    case 3:
                        QYEvents.RequestInviteGroup(new RequestGroupArgs(fromGroup, msg, responseFlag, AuthCode, source, subType));
                        break;
                }
                return 0;
            }

            private static Dictionary<string, string> InfoParse(string base64)
            {
                Dictionary<string, string> pairs = new Dictionary<string, string>();
                Unpack u = new Unpack(base64);
                int count = u.NextInt;
                for (int i = 0; i < count; i++)
                {
                    Unpack n = new Unpack(u.NextToken);
                    string key = n.NextStr;
                    pairs[key] = n.NextStr;
                    n.Dispose();
                }
                u.Dispose();
                return pairs;
            }
        }


        public static int AuthCode { get; private set; }
        private static int ProtocolType { get; set; }

        /// <summary>
        /// 获取应用数据目录，末尾带“\”
        /// <para>返回如：D:\robot\plugin\com.contract.testapp\</para>
        /// <para>应用的所有数据、配置【必须】存放于此目录，避免给用户带来困扰。</para>
        /// </summary>
        public static string AppDirectory => Marshal.PtrToStringAnsi(QY_getAppDirectory(AuthCode));
        [DllImport("QYOffer.dll")]
        private static extern IntPtr QY_getAppDirectory(int authCode);

        /// <summary>
        /// 获取当前登录的 QQ 号
        /// </summary>
        public const string LoginQQ = "1980853671";
        public const long LongQQ = 1980853671;
        //[DllImport("QYOffer.dll")]
        //private static extern IntPtr QY_getLoginQQList(int authCode);

        /// <summary>
        /// 获取当前登录 QQ 的昵称
        /// </summary>
        public static string LoginNick => Marshal.PtrToStringAnsi(QY_getLoginNick(AuthCode, LongQQ));
        [DllImport("QYOffer.dll")]
        private static extern IntPtr QY_getLoginNick(int authCode, long qqID);

        /// <summary>
        /// 获取当前框架登录 QQ 的在线状态
        /// </summary>
        public static OnlineStatus Status => (OnlineStatus)QY_getFrameAccountState(AuthCode, LongQQ);
        [DllImport("QYOffer.dll")]
        private static extern int QY_getFrameAccountState(int authCode, long qqID);

        /// <summary>
        /// 用于机器人框架发送消息
        /// </summary>
        public static class Send
        {
            ///// <summary>
            ///// 发送好友消息 
            ///// </summary>
            ///// <param name="qq">收件人 QQ</param>
            ///// <param name="msg">消息内容</param>
            ///// <returns>是否成功</returns>
            //public static bool Friend(string qq, string msg) => GTmp(G.m, qq, msg);
            //public static bool Friend(string qq, string msg)
            //{
            //    bool res = long.TryParse(qq, out long targ);
            //    if (!res)
            //        return false;
            //    int ret = QY_sendFriendMsg(AuthCode, LongQQ, targ, msg);
            //    return ret == 0;
            //}
            //[DllImport("QYOffer.dll")]
            //private static extern int QY_sendFriendMsg(int authCode, long qqID, long target, string msg);

            /// <summary>
            /// 发送群消息 
            /// </summary>
            /// <param name="group">群号</param>
            /// <param name="msg">消息内容</param>
            /// <returns>是否成功</returns>
            public static bool Group(string group, string msg)
            {
                bool res = long.TryParse(group, out long targ);
                if (!res)
                    return false;
                int ret = QY_sendGroupMsg(AuthCode, LongQQ, targ, msg);
                return ret == 0;
            }
            [DllImport("QYOffer.dll")]
            private static extern int QY_sendGroupMsg(int authCode, long qqID, long target, string msg);

            /// <summary>
            /// 发送群临时消息
            /// </summary>
            /// <param name="qq">收件人 QQ</param>
            /// <param name="msg">待发送消息内容</param>
            /// <returns>是否成功</returns>
            public static bool Friend(string qq, string msg)
            {
                bool res = long.TryParse(qq, out long targqq);
                if (!res)
                    return false;
                int ret = QY_sendGroupTmpMsg(AuthCode, LongQQ, 646751705, targqq, msg);
                return ret == 0;
            }
            [DllImport("QYOffer.dll")]
            public static extern int QY_sendGroupTmpMsg(int authCode, long qqID, long target, long targqq, string msg);

        }

        /// <summary>
        /// 在框架中记录日志
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="eventt">事件</param>
        /// <param name="color">颜色</param>
        public static void Log(string content, string eventt = "日志", LogColor color = LogColor.deepPink) => QY_addLog(AuthCode, 0, (int)color, eventt, content);
        [DllImport("QYOffer.dll")]
        private static extern int QY_addLog(int authCode, long qqID, int a, string b, string c);

        /// <summary>
        /// 点赞，用一次赞一下，每天可赞五十下
        /// </summary>
        /// <param name="target">目标</param>
        /// <returns>结果的枚举</returns>
        public static LikeResult Like(string target)
        {
            bool isok = long.TryParse(target, out long t);
            if (!isok)
                return LikeResult.notQQ;
            int res = QY_sendLikeFavorite(AuthCode, LongQQ, t);
            switch (res)
            {
                case 0:
                    return LikeResult.success;
                case 1:
                    return LikeResult.notAllowed;
                case 51:
                    return LikeResult.full;
            }
            return LikeResult.other;
        }
        [DllImport("QYOffer.dll")]
        private static extern int QY_sendLikeFavorite(int authCode, long qqID, long targ);

        /// <summary>
        /// 进行与群组相关的操作
        /// </summary>
        public static class Group
        {
            /// <summary>
            /// 设置群全员禁言
            /// </summary>
            /// <param name="group">群号</param>
            /// <returns>是否成功</returns>
            public static bool AllMuteOn(string group)
            {
                bool isok = long.TryParse(group, out long t);
                if (!isok)
                    return false;
                return QY_setGroupWholeBanSpeak(AuthCode, LongQQ, t, 1) == 0;
            }
            /// <summary>
            /// 解除群全员禁言
            /// </summary>
            /// <param name="group">群号</param>
            /// <returns>是否成功</returns>
            public static bool AllMuteOff(string group)
            {
                bool isok = long.TryParse(group, out long t);
                if (!isok)
                    return false;
                return QY_setGroupWholeBanSpeak(AuthCode, LongQQ, t, 0) == 0;
            }
            [DllImport("QYOffer.dll")]
            private static extern int QY_setGroupWholeBanSpeak(int authCode, long qqID, long targ, int s);

            /// <summary>
            /// 群成员禁言
            /// </summary>
            /// <param name="group">群号</param>
            /// <param name="member">成员 QQ</param>
            /// <param name="time">以秒为单位的时间，解禁则为 0</param>
            /// <returns>是否成功</returns>
            public static bool MuteMember(string group, string member, long time = 0)
            {
                bool isok = long.TryParse(group, out long t);
                if (!isok)
                    return false;
                isok = long.TryParse(member, out long m);
                if (!isok)
                    return false;
                return QY_setGroupMembersBanSpeak(AuthCode, LongQQ, t, m, time) == 0;
            }
            [DllImport("QYOffer.dll")]
            private static extern int QY_setGroupMembersBanSpeak(int authCode, long qqID, long targ, long s, long a);

            /// <summary>
            /// 更改群成员名片
            /// </summary>
            /// <param name="group">群号</param>
            /// <param name="member">成员 QQ</param>
            /// <param name="name">新的群名片</param>
            /// <returns>是否成功</returns>
            public static bool SetCard(string group, string member, string name)
            {
                bool isok = long.TryParse(group, out long t);
                if (!isok)
                    return false;
                isok = long.TryParse(member, out long m);
                if (!isok)
                    return false;
                //XXX: 要封也是测试服    没有这个
                return QY_setModifyGroupMemberCard(AuthCode, LongQQ, t, m, name) == 0;
            }
            [DllImport("QYOffer.dll")]
            private static extern int QY_setModifyGroupMemberCard(int authCode, long qqID, long targ, long s, string a);

            /// <summary>
            /// 群踢人
            /// </summary>
            /// <param name="group">群号</param>
            /// <param name="member">被踢的人</param>
            /// <param name="RejectJoin">是否拒绝其再次申请加入</param>
            /// <returns>是否成功</returns>
            public static bool RemoveMember(string group, string member, bool RejectJoin = false)
            {
                bool isok = long.TryParse(group, out long t);
                if (!isok)
                    return false;
                isok = long.TryParse(member, out long m);
                if (!isok)
                    return false;
                return QY_setGroupMembersKick(AuthCode, LongQQ, t, m, RejectJoin ? 1 : 0) == 0;
            }
            [DllImport("QYOffer.dll")]
            private static extern int QY_setGroupMembersKick(int authCode, long qqID, long targ, long s, int a);

            /// <summary>
            /// 允许群匿名聊天
            /// </summary>
            /// <param name="group">群号</param>
            /// <returns>是否成功</returns>
            public static bool EnableAnonymous(string group)
            {
                bool isok = long.TryParse(group, out long t);
                if (!isok)
                    return false;
                return QY_setGroupAnonymousBan(AuthCode, LongQQ, t, 0) == 0;
            }
            /// <summary>
            /// 禁止群匿名聊天
            /// </summary>
            /// <param name="group">群号</param>
            /// <returns>是否成功</returns>
            public static bool DisableAnonymous(string group)
            {
                bool isok = long.TryParse(group, out long t);
                if (!isok)
                    return false;
                return QY_setGroupAnonymousBan(AuthCode, LongQQ, t, 1) == 0;
            }
            [DllImport("QYOffer.dll")]
            private static extern int QY_setGroupAnonymousBan(int authCode, long qqID, long targ, int s);

            /// <summary>
            /// 设置管理员
            /// </summary>
            /// <param name="group">群号</param>
            /// <param name="member">群成员</param>
            /// <returns>是否成功</returns>
            public static bool AddAdmin(string group, string member)
            {
                bool isok = long.TryParse(group, out long t);
                if (!isok)
                    return false;
                isok = long.TryParse(member, out long m);
                if (!isok)
                    return false;
                return QY_setGroupAdmini(AuthCode, LongQQ, t, m, 1) == 0;
            }
            /// <summary>
            /// 撤销管理员
            /// </summary>
            /// <param name="group">群号</param>
            /// <param name="member">成员</param>
            /// <returns></returns>
            public static bool RemoveAdmin(string group, string member)
            {
                bool isok = long.TryParse(group, out long t);
                if (!isok)
                    return false;
                isok = long.TryParse(member, out long m);
                if (!isok)
                    return false;
                return QY_setGroupAdmini(AuthCode, LongQQ, t, m, 0) == 0;
            }
            [DllImport("QYOffer.dll")]
            private static extern int QY_setGroupAdmini(int authCode, long qqID, long targ, long s, int a);

            /// <summary>
            /// 退出群，如果指定自己创建的群，则解散群
            /// </summary>
            /// <param name="group">群号</param>
            /// <returns></returns>
            public static bool Exit(string group)
            {
                bool isok = long.TryParse(group, out long t);
                if (!isok)
                    return false;
                return QY_setExitGroupChat(AuthCode, LongQQ, t, 1) == 0;
            }
            [DllImport("QYOffer.dll")]
            private static extern int QY_setExitGroupChat(int authCode, long qqID, long targ, int s);

            /// <summary>
            /// 设置专属头衔
            /// </summary>
            /// <param name="group">群号</param>
            /// <param name="member">成员</param>
            /// <param name="title">专属头衔，删除留空</param>
            /// <returns></returns>
            public static bool SetTitle(string group, string member, string title = "")
            {
                bool isok = long.TryParse(group, out long t);
                if (!isok)
                    return false;
                isok = long.TryParse(member, out long m);
                if (!isok)
                    return false;
                return QY_setGroupSpecialTitle(AuthCode, LongQQ, t, m, title, -1) == 0;
            }
            [DllImport("QYOffer.dll")]
            private static extern int QY_setGroupSpecialTitle(int authCode, long qqID, long targ, long s, string q, int a);

            /// <summary>
            /// 添加群
            /// </summary>
            /// <param name="group">群号</param>
            /// <param name="msg">附加申请信息</param>
            /// <returns></returns>
            public static bool Add(string group, string msg = "")
            {
                bool isok = long.TryParse(group, out long t);
                if (!isok)
                    return false;
                return QY_setAddGroup(AuthCode, LongQQ, t, msg) == 0;
            }
            [DllImport("QYOffer.dll")]
            private static extern int QY_setAddGroup(int authCode, long qqID, long targ, string s);

            /// <summary>
            /// 上传群文件
            /// </summary>
            /// <param name="group">群号</param>
            /// <param name="path">文件本地路径</param>
            /// <param name="folder">群内的文件夹</param>
            /// <returns></returns>
            public static bool UploadFile(string group, string path, string folder = "/")
            {
                bool res = long.TryParse(group, out long t);
                if (!res)
                    return false;
                res = File.Exists(path);
                if (!res)
                    return false;
                return QY_setGroupFileUpload(AuthCode, LongQQ, t, folder, path) == 0;
            }
            [DllImport("QYOffer.dll")]
            private static extern int QY_setGroupFileUpload(int authCode, long qqID, long targ, string s, string a);

            /// <summary>
            /// 取群名片
            /// </summary>
            /// <param name="group">群号</param>
            /// <param name="member">成员</param>
            /// <returns></returns>
            public static string GetCard(string group, string member)
            {
                bool isok = long.TryParse(group, out long t);
                if (!isok)
                    return "";
                isok = long.TryParse(member, out long m);
                if (!isok)
                    return "";
                string b64 = Marshal.PtrToStringAnsi(QY_getGroupMemberCard(AuthCode, LongQQ, t, m, false));
                Unpack u = new Unpack(b64);
                return u.NextStr;
            }
            [DllImport("QYOffer.dll")]
            private static extern IntPtr QY_getGroupMemberCard(int authCode, long qqID, long targ, long s, bool a);

            /// <summary>
            /// 取群组的信息
            /// </summary>
            /// <param name="group">群号</param>
            /// <returns></returns>
            public static GroupInfo GetInfo(string group)
            {
                long.TryParse(group, out long t);
                string b64 = Marshal.PtrToStringAnsi(QY_getGroupInfo(AuthCode, LongQQ, t));
                Unpack u = new Unpack(b64);
                long a = u.NextLong;
                long b = u.NextLong;
                u.Skip(4);
                int c = u.NextInt;
                int d = u.NextInt;
                string e = u.NextStr;
                u.Skip(u.NextShort);
                int f = u.NextInt;
                return new GroupInfo(a, b, c, d, e, f, u.NextStr);
            }
            [DllImport("QYOffer.dll")]
            private static extern IntPtr QY_getGroupInfo(int authCode, long qqID, long targ);

            /// <summary>
            /// 允许群临时会话
            /// </summary>
            /// <param name="group">群号</param>
            /// <returns>是否成功</returns>
            public static bool EnableTmpSession(string group)
            {
                bool isok = long.TryParse(group, out long t);
                if (!isok)
                    return false;
                return QY_setGroupPrivateSession(AuthCode, LongQQ, t, 1) == 0;
            }
            /// <summary>
            /// 禁止群临时会话
            /// </summary>
            /// <param name="group">群号</param>
            /// <returns>是否成功</returns>
            public static bool DisableTmpSession(string group)
            {
                bool isok = long.TryParse(group, out long t);
                if (!isok)
                    return false;
                return QY_setGroupPrivateSession(AuthCode, LongQQ, t, 0) == 0;
            }
            [DllImport("QYOffer.dll")]
            private static extern int QY_setGroupPrivateSession(int authCode, long qqID, long targ, int s);

            /// <summary>
            /// 允许发起多人聊天
            /// </summary>
            /// <param name="group">群号</param>
            /// <returns>是否成功</returns>
            public static bool EnableCreateMultiChat(string group)
            {
                bool isok = long.TryParse(group, out long t);
                if (!isok)
                    return false;
                return QY_setGroupManyPeopleChat(AuthCode, LongQQ, t, 1) == 0;
            }
            /// <summary>
            /// 禁止发起多人聊天
            /// </summary>
            /// <param name="group">群号</param>
            /// <returns>是否成功</returns>
            public static bool DisableCreateMultiChat(string group)
            {
                bool isok = long.TryParse(group, out long t);
                if (!isok)
                    return false;
                return QY_setGroupManyPeopleChat(AuthCode, LongQQ, t, 0) == 0;
            }
            [DllImport("QYOffer.dll")]
            private static extern int QY_setGroupManyPeopleChat(int authCode, long qqID, long targ, int s);

            /// <summary>
            /// 取群成员列表
            /// </summary>
            /// <param name="group">群号</param>
            /// <returns></returns>
            public static List<GroupMember> GetMembers(string group)
            {
                long.TryParse(group, out long t);
                string b64 = Marshal.PtrToStringAnsi(QY_getGroupMemberList(AuthCode, LongQQ, t));
                Unpack u = new Unpack(b64);
                int count = u.NextInt;
                List<GroupMember> l = new List<GroupMember>();
                for (int i = 0; i < count; i++)
                {
                    Unpack n = new Unpack(u.NextToken);
                    l.Add(new GroupMember(n.NextLong, n.NextStr, n.NextStr, n.NextInt, n.NextInt, n.NextStr, n.NextInt, n.NextInt, n.NextStr, n.NextInt, n.NextInt, n.NextStr, n.NextInt, n.NextInt));
                }
                return l;
            }
            [DllImport("QYOffer.dll")]
            private static extern IntPtr QY_getGroupMemberList(int authCode, long qqID, long targ);

        }

        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="target">被删除的好友QQ</param>
        /// <returns></returns>
        public static bool RemoveFriend(string target)
        {
            bool res = long.TryParse(target, out long targ);
            if (!res)
                return false;
            return QY_setDelFriend(AuthCode, LongQQ, targ) == 0;
        }
        [DllImport("QYOffer.dll")]
        private static extern int QY_setDelFriend(int authCode, long qqID, long targ);

        public static string GetNick(string qq)
        {
            string b64 = Marshal.PtrToStringAnsi(QY_getSummaryInfo(AuthCode, LongQQ, long.Parse(qq)));
            Unpack u = new Unpack(b64);
            _ = u.NextLong;
            _ = u.NextInt;
            string r = u.NextStr;
            u.Dispose();
            return r;
        }
        [DllImport("QYOffer.dll")]
        private static extern IntPtr QY_getSummaryInfo(int authCode, long qqID, long targ);
    }

}
