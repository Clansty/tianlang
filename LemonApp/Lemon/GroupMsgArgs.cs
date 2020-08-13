using System;

namespace Clansty.tianlang
{
    internal class GroupMsgArgs : EventArgs
    {
        internal GroupMsgArgs(long a, long b, string c)
        {
            FromQQ = a;
            FromGroup = b;
            Msg = c;
        }
        /// <summary>
        /// 发送这条消息的人
        /// </summary>
        internal long FromQQ { get; }
        /// <summary>
        /// 来源群组
        /// </summary>
        internal long FromGroup { get; }
        /// <summary>
        /// 消息内容
        /// </summary>
        internal string Msg { get; }
        /// <summary>
        /// 快捷回复
        /// </summary>
        /// <param name="msg">消息内容</param>
        /// <param name="includeSrcMsg">是否引用原始消息，设为 true 相当于在 msg 开头加入 SrcMsg</param>
        internal void Reply(string msg, bool _ = false) => Robot.Send.Group(FromGroup, msg);
        //TODO 撤回
    }
}