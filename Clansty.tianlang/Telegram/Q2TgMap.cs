using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Clansty.tianlang
{
    public static class Q2TgMap
    {
        public static FwdInfo[] infos =
            JsonConvert.DeserializeObject<FwdInfo[]>(File.ReadAllText("/root/data/q2tg.json"));

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