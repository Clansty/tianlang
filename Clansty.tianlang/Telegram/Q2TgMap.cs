using System.Linq;

namespace Clansty.tianlang
{
    public static class Q2TgMap
    {
        public static readonly FwdInfo[] infos =
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
                tg = -1001342293974,
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
                tg = -1001318314287,
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
            {//科协技术部
                uin = 839827911,
                gin = 865604042,
                tg = -433260120,
                includeSender = false
            },
            new FwdInfo()
            {//英语提高3班
                uin = 839827911,
                gin = 1156757956,
                tg = -279581459,
                includeSender = false
            },
            new FwdInfo()
            {//高数
                uin = 839827911,
                gin = 915287147,
                tg = -411013453,
                includeSender = false
            },
            new FwdInfo()
            {//家长群
                uin = 839827911,
                gin = G.parentsFwd,
                tg = -481322235,
                includeSender = false
            },
            new FwdInfo()
            {//计算机基础
                uin = 839827911,
                gin = 1130629129,
                tg = -494959345,
                includeSender = false
            },
            new FwdInfo()
            {//web
                uin = 839827911,
                gin = 935726131,
                tg = -409372799,
                includeSender = false
            },
            new FwdInfo()
            {
                uin = C.self,
                gin = G.testNew,
                tg = G.TG.testNew,
                includeSender = false
            },
            new FwdInfo()
            {//xiaoshuiqun
                uin = 839827911,
                gin = 1057087079,
                tg = -1001166128384,
                includeSender = false
            },
            new FwdInfo()
            {//huashui
                uin = 839827911,
                gin = 593764793,
                tg = -497638833,
                includeSender = false
            },
            new FwdInfo()
            {//dc
                uin = 839827911,
                gin = 1161139803,
                tg = -429181992,
                includeSender = false
            },
        };

        internal static FwdInfo Q2Tg(long uin, long gin)
        {
            return infos.FirstOrDefault(i => i.uin == uin && i.gin == gin);
        }

        internal static FwdInfo Tg2Q(long tguid)
        {
            return infos.FirstOrDefault(i => i.tg == tguid);
        }
    }
}