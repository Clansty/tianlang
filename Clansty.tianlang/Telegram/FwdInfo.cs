namespace Clansty.tianlang
{
    public class FwdInfo
    {
        /// <summary>
        /// qq 号
        /// </summary>
        public long uin;

        /// <summary>
        /// 群号
        /// </summary>
        public long gin;

        /// <summary>
        /// tguid
        /// </summary>
        public long tg;

        public bool includeSender = true;
        public bool preferRemark = true;
    }
}