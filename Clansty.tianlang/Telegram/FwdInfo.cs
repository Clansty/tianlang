using Mirai_CSharp;

namespace Clansty.tianlang
{
    public class FwdInfo
    {
        /// <summary>
        /// qq 号
        /// </summary>
        public MiraiHttpSession host = C.QQ.NthsBot;

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