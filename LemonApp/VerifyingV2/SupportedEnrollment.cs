﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clansty.tianlang.VerifyingV2
{
    static class SupportedEnrollment
    {
        static List<int> list = new List<int>()
        {
            2017,
            2018,
            2019
        };
        internal static bool Contains(int enr)
        {
            return list.Contains(enr);
        }
    }
}
