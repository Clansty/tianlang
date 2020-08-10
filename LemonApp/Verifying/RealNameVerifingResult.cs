namespace Clansty.tianlang
{
    enum RealNameVerifingResult
    {
        /// <summary>
        /// 找不到此身份
        /// </summary>
        notFound,
        /// <summary>
        /// 2019 级新生暂时无法进行实名认证
        /// </summary>
        unsupported,
        /// <summary>
        /// 该身份已被占用
        /// </summary>
        occupied,
        /// <summary>
        /// 姓名与年级无法对应（已弃用）
        /// </summary>
        unmatch,
        /// <summary>
        /// 验证状态正常
        /// </summary>
        succeed,
        /// <summary>
        /// 玄学错误，应该不会发生
        /// </summary>
        wtf
    }
}
