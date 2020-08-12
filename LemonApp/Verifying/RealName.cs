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
            return null;//TODO
        }

        /// <summary>
        /// 绑定实名身份
        /// </summary>
        /// <param name="qq"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static RealNameBindingResult Bind(long qq, string name)
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
