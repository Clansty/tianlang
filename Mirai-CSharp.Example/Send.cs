using Mirai_CSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Mirai_CSharp.Tianlang
{
    static class S
    {
        public static void Group(long group, params IMessageBase[] msg)
        {
            Program.session.SendGroupMessageAsync(group, msg);
        }
        public static void Group(long group, string msg)
        {
            Group(group, new IMessageBase[]
            {
                new PlainMessage(msg)
            });
        }

        public static void Private(long qq, params IMessageBase[] msg)
        {
            Program.session.SendFriendMessageAsync(qq, msg);
        }
        public static void Private(long qq, string msg)
        {
            Program.session.SendFriendMessageAsync(qq, new IMessageBase[]
            {
                new PlainMessage(msg)
            });
        }

        public static void Major(string msg) => Group(G.Major, msg);
        public static void Si(string msg) => Group(G.Console, msg);
        public static void Test(string msg) => Group(G.test, msg);

        public static void IDE(string msg) => Group(G.iDE, msg);
        public static void Major(params IMessageBase[] msg) => Group(G.Major, msg);
        public static void Si(params IMessageBase[] msg) => Group(G.Console, msg);
        public static void Test(params IMessageBase[] msg) => Group(G.test, msg);

        public static void IDE(params IMessageBase[] msg) => Group(G.iDE, msg);
    }
}
