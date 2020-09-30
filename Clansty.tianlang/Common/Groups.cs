using System.Collections.Generic;

namespace Clansty.tianlang
{
    static class G
    {
#if DEBUG
        internal const long major = 670526569;
        internal const long si = 960701873;
#else
        internal const long major = 646751705;
        internal const long si = 690696283;
#endif
        internal const long test = 828390342;
        internal const long iDE = 342975953;
        internal const long g2020 = 1132458399;
        internal const long parents = 1034335441;
        internal const long parentsFwd = 654690796;
        internal const long dorm = 543652326;

        public static readonly Dictionary<long, long> Map = new Dictionary<long, long>()
        {
            [iDE] = TG.iDE,
            [test] = TG.test,
            [TG.test] = test,
            [TG.iDE] = iDE,
            [si] = TG.si,
            [TG.si] = si,
            [TG.dorm] = dorm,
            [dorm] = TG.dorm
        };

        public static class TG
        {
            internal const long iDE = -1001218396541;
            internal const long test = -346751886;
            internal const long si = -1001198326304;
            internal const long major = -1001414122709;
            internal const long dorm = -389685119;
        }
    }
}