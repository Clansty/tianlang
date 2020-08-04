using System.Windows.Forms;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;

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
                if (C.recording)
                {
                    var u = new User(e.FromQQ);
                    if (u.Enrollment == 2020 || u.Enrollment == 2017 && u.Junior)
                    {
                        Rds.SAdd("elec2020", u.Uin);
                    }
                }
            }
        }
    }
}