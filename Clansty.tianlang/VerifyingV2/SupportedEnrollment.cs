using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clansty.tianlang
{
    static class SupportedEnrollment
    {
        static readonly List<long> list = new List<long>()
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
