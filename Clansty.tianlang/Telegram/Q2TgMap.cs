using System.Linq;
using Mirai_CSharp;

namespace Clansty.tianlang
{
    public static class Q2TgMap
    {
        public static readonly FwdInfo[] infos =
        {
            new()
            {
                gin = G.iDE,
                tg = G.TG.iDE
            },
            new()
            {
                gin = G.si,
                tg = G.TG.si
            },
            new()
            {
                gin = G.dorm,
                tg = G.TG.dorm
            },
            new()
            {
                gin = G.major,
                tg = G.TG.major
            },
            new()
            {
                gin = G.wxb,
                tg = G.TG.wxb
            },
            new()
            {
                //计软院
                host = C.QQ.Clansty,
                gin = 954226654,
                tg = -1001342293974,
                includeSender = false
            },
            new()
            {
                //程序设计基础
                host = C.QQ.Clansty,
                gin = 892697611,
                tg = -416480031,
                includeSender = false
            },
            new()
            {
                //软合一班
                host = C.QQ.Clansty,
                gin = 1022579554,
                tg = -426831224,
                includeSender = false
            },
            new()
            {
                //软合二班
                host = C.QQ.Clansty,
                gin = 1078188647,
                tg = -1001318314287,
                includeSender = false
            },
            new()
            {
                //科协
                host = C.QQ.Clansty,
                gin = 598094369,
                tg = -337216611,
                includeSender = false
            },
            new()
            {
                //科协技术部
                host = C.QQ.Clansty,
                gin = 865604042,
                tg = -433260120,
                includeSender = false
            },
            new()
            {
                //英语提高3班
                host = C.QQ.Clansty,
                gin = 1156757956,
                tg = -279581459,
                includeSender = false
            },
            new()
            {
                //高数
                host = C.QQ.Clansty,
                gin = 915287147,
                tg = -411013453,
                includeSender = false
            },
            new()
            {
                //家长群
                host = C.QQ.Clansty,
                gin = G.parentsFwd,
                tg = -481322235,
                includeSender = false
            },
            new()
            {
                //计算机基础
                host = C.QQ.Clansty,
                gin = 1130629129,
                tg = -494959345,
                includeSender = false
            },
            new()
            {
                //web
                host = C.QQ.Clansty,
                gin = 935726131,
                tg = -409372799,
                includeSender = false
            },
            new()
            {
                gin = G.testNew,
                tg = G.TG.testNew,
                includeSender = false
            },
            new()
            {
                //xiaoshuiqun
                host = C.QQ.Clansty,
                gin = 1057087079,
                tg = -1001166128384,
                includeSender = false
            },
            new()
            {
                //huashui
                host = C.QQ.Clansty,
                gin = 593764793,
                tg = -497638833,
                includeSender = false
            },
            new()
            {
                //dc
                host = C.QQ.Clansty,
                gin = 1161139803,
                tg = -429181992,
                includeSender = false
            },
            new()
            {
                //母校行
                host = C.QQ.Clansty,
                gin = 648795598,
                tg = -1001234778158,
                includeSender = false
            },
            new()
            {
                //nths@nuist
                host = C.QQ.Clansty,
                gin = 311153291,
                tg = -455439414,
                includeSender = false
            },
            new()
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