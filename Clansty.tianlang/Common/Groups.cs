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
        public static readonly Dictionary<long, long> Map = new Dictionary<long, long>()
        {
            [342975953] = -1001218396541,
            [828390342] = -346751886,
            [-346751886] = 828390342,
            [-1001218396541] = 342975953,
            [690696283] = -1001198326304,
            [-1001198326304] = 690696283
        };

    }
}
