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
        public static RealNameCheckingResult Check(string name)
        {
            var 
            res = Rds.HGet("rn2019", name);
            if (res != "") //新高1
                return ret(RealNameStatus.e2019);
            res = Rds.HGet("rn2019jc", name);
            if (res != "") //金1
                return ret(RealNameStatus.e2019jc);
            res = Rds.HGet("rn2018", name);
            if (res != "") //新高二
                return ret(RealNameStatus.e2018);
            res = Rds.HGet("rn2018jc", name);
            if (res != "") //新高二
                return ret(RealNameStatus.e2018jc);
            res = Rds.HGet("rn2017", name);
            if (res != "") //新高三\
                return ret(RealNameStatus.e2017);
            return new RealNameCheckingResult(RealNameStatus.notFound);
            RealNameCheckingResult ret(RealNameStatus r)
            {
                if (res == "0")
                    return new RealNameCheckingResult(r);
                return new RealNameCheckingResult(r, res);
            }
        }

        /// <summary>
        /// 绑定实名身份
        /// </summary>
        /// <param name="qq"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static RealNameBindingResult Bind(string qq, string name)
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
            if (chk.Status == RealNameStatus.e2018jc)
                Rds.HSet("rn2018jc", name, qq);
            if (chk.Status == RealNameStatus.e2019)
                Rds.HSet("rn2019", name, qq);
            if (chk.Status == RealNameStatus.e2019jc)
                Rds.HSet("rn2019jc", name, qq);
            return RealNameBindingResult.succeed;
        }
    }
}
