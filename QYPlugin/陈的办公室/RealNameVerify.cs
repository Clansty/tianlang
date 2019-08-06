using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clansty.tianlang.陈的办公室
{
    /// <summary>
    /// 龙门近卫局时刻关注安全情况
    /// </summary>
    static class RealNameVerify
    {
        public static RealNameVerifyResult Check (string name)
        {
            var res = Rds.HGet("rn2018", name);
            if (res != "") //新高二
            {
                if (res == "0")
                    return new RealNameVerifyResult(RealNameStatus.e2018);
                else
                    return new RealNameVerifyResult(RealNameStatus.e2018, res);
            }
            res = Rds.HGet("rn2017", name);
            if (res != "") //新高三
            {
                if (res == "0")
                    return new RealNameVerifyResult(RealNameStatus.e2017);
                else
                    return new RealNameVerifyResult(RealNameStatus.e2017, res);
            }
            return new RealNameVerifyResult(RealNameStatus.notFound);
        }
    }
}
