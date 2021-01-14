using System.Collections.Generic;

namespace Clansty.tianlang
{
    static class SupportedEnrollment
    {
        static readonly List<long> list = new()
        {
            2017,
            2018,
            2019,
            2020
        };
        internal static bool Contains(long enr)
        {
            return list.Contains(enr);
        }
    }
}
