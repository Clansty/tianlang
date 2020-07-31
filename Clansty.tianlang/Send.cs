using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Native.Sdk.Cqp;
using Native.Sdk.Cqp.Model;

namespace Clansty.tianlang
{
    static class S
    {
        public static void Group(long group, object msg, CQApi api = null)
        {
            if (api is null)
                api = C.CQApi;
            api.SendGroupMessage(group, msg);
        }

        public static void Private(QQ qq, object msg, CQApi api = null)
        {
            Private(qq.Id, msg, api);
        }

        public static void Private(string qq, object msg, CQApi api = null)
        {
            Private(long.Parse(qq), msg, api);
        }

        public static void Private(long longqq, object msg, CQApi api = null)
        {
            if (api is null)
                api = C.CQApi;
            api.SendPrivateMessage(longqq, msg);
        }

        public static void Major(object msg, CQApi api = null) => Group(G.major, msg, api);
        public static void Si(object msg, CQApi api = null) => Group(G.si, msg, api);
        public static void Test(object msg, CQApi api = null) => Group(G.test, msg, api);
    }
}