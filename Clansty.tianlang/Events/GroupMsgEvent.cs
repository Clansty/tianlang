using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clansty.tianlang.Events
{
    public class GroupMsgEvent : IGroupMessage
    {
        public void GroupMessage(object sender, CQGroupMessageEventArgs e)
        {
            if (e.FromGroup == G.si)
                Cmds.SiEnter(e);

            if (e.FromGroup == G.major)
            {
                //UserInfo.CheckQmpAsync(new User(e.FromQQ), e.Card);
                if (e.Message.Text.StartsWith("sudo "))
                    Cmds.SudoEnter(e);
                Repeater.Enter(e.Message);
            }

            if (e.FromGroup == G.test)
            {
                //TODO 结构化消息调试

                //if (e.Msg.StartsWith("{") && e.Msg.IndexOf('}') > 0)
                //    e.Reply($"[LQ:lightappelem,type=1,data={e.Msg.Replace(",", "&#44;")},msg_resid=]");
                //else if (e.Msg.StartsWith("[LQ:lightappelem,type=1,data=") && e.Msg.IndexOf(",msg_resid=]") > 0)
                //    e.Reply(e.Msg.Between("[LQ:lightappelem,type=1,data=", ",msg_resid=]").Replace("&#44;", ","));
                //else if (e.Msg.StartsWith("[LQ:richmsg") && e.Msg.IndexOf("]") > 0)
                //    e.Reply(e.Msg.Trim('[', ']'));
                //else if (e.Msg.StartsWith("LQ:richmsg"))
                //    e.Reply($"[{e.Msg.UnEscape()}]");
            }
        }
    }
}