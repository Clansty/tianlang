using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Native.Sdk.Cqp.Model;

namespace Clansty.tianlang
{
    static class S
    {
        public static void Group(long group, params object[] msg)
        {
            C.CQApi.SendGroupMessage(group, msg);
        }

        public static void Private(QQ qq, params object[] msg)
        {
            Private(qq.Id, msg);
        }

        public static void Private(string qq, params object[] msg)
        {
            Private(long.Parse(qq), msg);
        }

        public static void Private(long longqq, params object[] msg)
        {
            C.CQApi.SendPrivateMessage(longqq, msg);
        }

        public static void Major(params object[] msg) => Group(G.major, msg);
        public static void Si(params object[] msg) => Group(G.si, msg);
        public static void Test(params object[] msg) => Group(G.test, msg);

        internal static void IDE(params object[] msg) => Group(G.iDE, msg);
    }
}
