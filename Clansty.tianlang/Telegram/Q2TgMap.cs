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