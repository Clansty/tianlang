using System;

namespace Clansty.tianlang
{
    internal class TempMsgArgs : EventArgs
    {
        public TempMsgArgs(long fq, long fg, string m)
        {
            FromGroup = fg.ToString();
            FromQQ = fq.ToString();
            Msg = m;
        }
        /// <summary>
        /// 发送者所在的群号
        /// </summary>
        public string FromGroup { get; }
        /// <summary>
        /// 发送这条消息的 QQ
        /// </summary>
        public string FromQQ { get; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Msg { get; }

        /// <summary>
        /// 快捷回复这条消息，即向 FromGroup 的 FromQQ 发送群临时消息
        /// </summary>
        /// <param name="msg">回复消息内容</param>
        /// <returns>是否成功</returns>
        public void Reply(string msg) => Robot.Send.Temp(FromGroup, FromQQ, msg);
    }
}