using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tianlang
{
    public static class S
    {
        /// <summary>
        /// 发群消息
        /// </summary>
        /// <param name="to">群号</param>
        /// <param name="msg">内容</param>
        public static void Group(string to, string msg)
        {
            if (msg.IndexOf("</msg>") > -1)
                IRQQApi.Api_SendXML(C.w, 1, 2, to, to, msg, 0);
            else
                IRQQApi.Api_SendMsg(C.w, 2, to, to, msg, -2);
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
        public static void P(string qq,string msg)
        {
            bool isXml = msg.IndexOf("</msg>") > -1;
            if (isXml)
            {
                if (IRQQApi.Api_IfFriend(C.w, qq))
                    IRQQApi.Api_SendXML(C.w, 1, 1, qq, qq, msg, 0);
                else
                    IRQQApi.Api_SendXML(C.w, 1, 4, G.major, qq, msg, 0);
            }
            else
            {
                if (IRQQApi.Api_IfFriend(C.w, qq))
                    IRQQApi.Api_SendMsg(C.w, 1, qq, qq, msg, -2);
                else
                    IRQQApi.Api_SendMsg(C.w, 4, G.major, qq, msg, -2);
            }
        }
        
    }
}
