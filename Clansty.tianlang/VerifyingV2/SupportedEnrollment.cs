using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clansty.tianlang
{
    static class SupportedEnrollment
    {
        static readonly List<int> list = new List<int>()
        {
            2017,
            2018,
            2019,
            2020
        };
        internal static bool Contains(int enr)
        {
            return list.Contains(enr);
        }
    }
}
