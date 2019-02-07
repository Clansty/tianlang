
namespace tianlang
{
    public struct S
    {
        /// <summary>
        /// 发群消息
        /// </summary>
        /// <param name="to">群号</param>
        /// <param name="msg">内容</param>
        public static void Group(string to, string msg)
        {
            if (msg.IndexOf("</msg>") > -1)
                IRQQApi.Api_SendXML(C.W, 1, 2, to, to, msg, 0);
            else
                IRQQApi.Api_SendMsg(C.W, 2, to, to, msg, 600028);
        }
        /// <summary>
        /// 发到测试群
        /// </summary>
        /// <param name="msg">内容</param>
        public static void Test(string msg) => Group(G.test, msg);
        /// <summary>
        /// 发到特调处
        /// </summary>
        /// <param name="msg"></param>
        public static void Si(string msg) => Group(G.si, msg);

        /// <summary>
        /// 发到大群
        /// </summary>
        /// <param name="msg"></param>
        public static void Major(string msg) => Group(G.major, msg);

        /// <summary>
        /// 私聊
        /// </summary>
        /// <param name="qq"></param>
        /// <param name="msg"></param>
        public static void P(string qq, string msg, bool alternative = false)
        {
            bool isXml = msg.IndexOf("</msg>") > -1;
            if (isXml)
            {
                if (IRQQApi.Api_IfFriend(C.W, qq))
                    IRQQApi.Api_SendXML(C.W, 1, 1, qq, qq, msg, 0);
                else if (alternative)
                {
                    string session = C.GetSession(qq);
                    if (session != "" && session != null)
                        IRQQApi.Api_SendXML(C.W, 1, 4, session, qq, msg, 0);
                    else
                        IRQQApi.Api_SendXML(C.W, 1, 4, G.major, qq, msg, 0);
                }
                else
                {
                    IRQQApi.Api_SendXML(C.W, 1, 4, G.major, qq, msg, 0);
                }
            }
            else
            {
                if (IRQQApi.Api_IfFriend(C.W, qq))
                    IRQQApi.Api_SendMsg(C.W, 1, qq, qq, msg, 600028);
                else if (alternative)
                {
                    string session = C.GetSession(qq);
                    if (session != "" && session != null)
                        IRQQApi.Api_SendMsg(C.W, 4, session, qq, msg, 600028);
                    else
                        IRQQApi.Api_SendMsg(C.W, 4, G.major, qq, msg, 600028);
                }
                else
                {
                    IRQQApi.Api_SendMsg(C.W, 4, G.major, qq, msg, 600028);
                }


                //if (IRQQApi.Api_IfFriend(C.w, qq))
                //    IRQQApi.Api_SendMsg(C.w, 1, qq, qq, msg, 600028);
                //else if (C.IsMember(qq))
                //    IRQQApi.Api_SendMsg(C.w, 4, G.major, qq, msg, 600028);
                //else
                //{
                //    string session = C.GetSession(qq);
                //    if (session != "" && session != null)
                //        IRQQApi.Api_SendMsg(C.w, 4, session, qq, msg, 600028);
                //    else
                //        Test($"向非好友且非大群成员 {qq} 发送 {msg} 失败");
                //}
            }
        }
        public static void P(int uid, string msg, bool alternative = false) => P(new User(uid).QQ, msg, alternative);
        public static void P(GroupMember m, string msg) => P(m.uin.ToString(), msg, true);
    }
}
