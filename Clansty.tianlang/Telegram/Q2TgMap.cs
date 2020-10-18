using System.IO;

namespace Clansty.tianlang
{
    public class Q2TgMap
    {
        private static readonly FwdInfo[] infos =
        {
            new FwdInfo()
            {
                uin = C.self,
                gin = G.iDE,
                tg = G.TG.iDE
            },
            new FwdInfo()
            {
                uin = C.self,
                gin = G.test,
                tg = G.TG.test
            },
            new FwdInfo()
            {
                uin = C.self,
                gin = G.si,
                tg = G.TG.si
            },
            new FwdInfo()
            {
                uin = C.self,
                gin = G.dorm,
                tg = G.TG.dorm
            },
            new FwdInfo()
            {
                uin = C.self,
                gin = G.major,
                tg = G.TG.major
            },
            new FwdInfo()
            {
                uin = C.self,
                gin = G.ddf,
                tg = G.TG.ddf
            },
            new FwdInfo()
            {
                uin = C.self,
                gin = G.wxb,
                tg = G.TG.wxb
            },
            new FwdInfo()
            {//计软院
                uin = 839827911,
                gin = 954226654,
                tg = -342881810,
                includeSender = false
            },
            new FwdInfo()
            {//程序设计基础
                uin = 839827911,
                gin = 892697611,
                tg = -416480031,
                includeSender = false
            },
            new FwdInfo()
            {//软合一班
                uin = 839827911,
                gin = 1022579554,
                tg = -426831224,
                includeSender = false
            },
            new FwdInfo()
            {//软合二班
                uin = 839827911,
                gin = 1078188647,
                tg = -425421691,
                includeSender = false
            },
            new FwdInfo()
            {//科协
                uin = 839827911,
                gin = 598094369,
                tg = -337216611,
                includeSender = false
            },
            new FwdInfo()
            {//英语提高3班
                uin = 839827911,
                gin = 1156757956,
                tg = -279581459,
                includeSender = false
            },
        };

        internal static FwdInfo Q2Tg(long uin, long gin)
        {
            foreach (var i in infos)
            {
                if (i.uin == uin && i.gin == gin)
                {
                    return i;
                }
            }

            return null;
        }

        internal static FwdInfo Tg2Q(long tguid)
        {
            foreach (var i in infos)
            {
                if (i.tg == tguid)
                    return i;
            }

            return null;
        }
    }
}