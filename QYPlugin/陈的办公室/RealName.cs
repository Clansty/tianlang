using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clansty.tianlang
{
    /// <summary>
    /// 龙门近卫局时刻关注安全情况
    /// </summary>
    static class RealName
    {
        /// <summary>
        /// 检查姓名状态
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static RealNameCheckingResult Check (string name)
        {
            var res = Rds.HGet("rn2018", name);
            if (res != "") //新高二
            {
                if (res == "0")
                    return new RealNameCheckingResult(RealNameStatus.e2018);
                else
                    return new RealNameCheckingResult(RealNameStatus.e2018, res);
            }
            res = Rds.HGet("rn2017", name);
            if (res != "") //新高三
            {
                if (res == "0")
                    return new RealNameCheckingResult(RealNameStatus.e2017);
                else
                    return new RealNameCheckingResult(RealNameStatus.e2017, res);
            }
            return new RealNameCheckingResult(RealNameStatus.notFound);
        }

        /// <summary>
        /// 绑定实名身份
        /// </summary>
        /// <param name="qq"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static RealNameBindingResult Bind(string qq,string name)
        {
            var chk = Check(name);
            if (chk.Status == RealNameStatus.notFound)
                return RealNameBindingResult.notFound;
            if (chk.OccupiedQQ == qq)
                return RealNameBindingResult.noNeed;
            if (chk.OccupiedQQ != null)
                return RealNameBindingResult.occupied;
            if (chk.Status == RealNameStatus.e2017)
                Rds.HSet("rn2017", name, qq);
            if (chk.Status == RealNameStatus.e2018)
                Rds.HSet("rn2018", name, qq);
            return RealNameBindingResult.succeed;
        }
    }
}
