using System.Linq;
using Mirai_CSharp;

namespace Clansty.tianlang
{
    public static class Q2TgMap
    {
        public static readonly FwdInfo[] infos =
        {
            new FwdInfo
            {
                gin = G.iDE,
                tg = G.TG.iDE
            },
            new FwdInfo
            {
                gin = G.si,
                tg = G.TG.si
            },
            new FwdInfo
            {
                gin = G.dorm,
                tg = G.TG.dorm
            },
            new FwdInfo
            {
                gin = G.major,
                tg = G.TG.major
            },
            new FwdInfo
            {
                gin = G.wxb,
                tg = G.TG.wxb
            },
            new FwdInfo
            {
                gin = G.testNew,
                tg = G.TG.testNew,
                includeSender = false
            },
            new FwdInfo
            {
                gin = 628301340,
                tg = -1001192668953,
            },
        };

        internal static FwdInfo Q2Tg(MiraiHttpSession uin, long gin)
        {
            return infos.FirstOrDefault(i => i.host == uin && i.gin == gin);
        }

        internal static FwdInfo Tg2Q(long tguid)
        {
            return infos.FirstOrDefault(i => i.tg == tguid);
        }
    }
}