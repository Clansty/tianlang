using System;

namespace LemonApp
{
    internal class FriendMsgArgs : EventArgs
    {
        public FriendMsgArgs(long fq, string m)
        {
            FromQQ = fq.ToString();
            Msg = m;
        }
        /// <summary>
        /// 发送这条消息的 QQ
        /// </summary>
        public string FromQQ { get; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Msg { get; }

        /// <summary>
        /// 快捷回复这条消息，即向 FromQQ 发送好友消息
        /// </summary>
        /// <param name="msg">回复消息内容</param>
        /// <returns>是否成功</returns>
        public void Reply(string msg) => Robot.Send.Friend(FromQQ, msg);
    }
}