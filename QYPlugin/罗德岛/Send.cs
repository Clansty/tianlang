using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Clansty.tianlang
{
    static class S
    {
        public static void Group(string group, string msg)
        {
            Robot.Send.Group(group, msg);
            Thread.Sleep(100);
        }

        public static void Private(string qq, string msg)
        {
            Robot.Send.Friend(qq, msg);
            Thread.Sleep(100);
        }

        public static void Major(string msg) => Group(G.major, msg);
        public static void Si(string msg) => Group(G.si, msg);
        public static void Test(string msg) => Group(G.test, msg);

        internal static void IDE(string msg) => Group(G.iDE, msg);
    }
}
