using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;

namespace Clansty.tianlang.Events
{
    public class PrivateMsgEvent : IPrivateMessage
    {
        public void PrivateMessage(object sender, CQPrivateMessageEventArgs e)
        {
            var u = new User(e.FromQQ);
            if (u.Status == Status.setup)
                Setup.Enter(e);
            else if (e.Message == "whoami")
            {
                UserInfo.CheckQmpAsync(u);
                e.Reply(u.ToXml("你的信息"));
            }
            else if (e.Message.Text.StartsWith("我叫"))
            {
                //here, to line 93 handles manual name-filling event
                var name = e.Message.Text.GetRight("我叫").Trim();
                if (u.Verified)
                {
                    e.Reply(Strs.Get("rnVerified")); //你已经实名认证了，无需再次补填姓名
                    return;
                }

                if (u.VerifyMsg == RealNameVerifingResult.unsupported)
                {
                    //尝试进行验证
                    u.Name = name;
                    e.Reply(Strs.Get("rnOK")); // here set succeeded
                    //update in 3.0.14.2 notice the administration
                    S.Si(u.ToXml("旧人补填姓名成功"));
                    UserInfo.CheckQmpAsync(u);
                    return;
                }

                var chk = RealName.Check(name);

                if (chk.Status == RealNameStatus.notFound)
                {
                    e.Reply(Strs.Get("rnNotFound"));
                    return;
                }

                if (chk.OccupiedQQ != null && chk.OccupiedQQ != e.FromQQ)
                {
                    e.Reply(Strs.Get("rnOccupied"));
                    return;
                }

                if (chk.Status == RealNameStatus.e2017 && u.Enrollment != 2017)
                {
                    e.Reply(Strs.Get("rnUnmatch", u.Grade));
                    return;
                }

                if (chk.Status == RealNameStatus.e2018 && u.Enrollment != 2018)
                {
                    e.Reply(Strs.Get("rnUnmatch", u.Grade));
                    return;
                }

                if (chk.Status == RealNameStatus.e2019 && u.Enrollment != 2019)
                {
                    e.Reply(Strs.Get("rnUnmatch", u.Grade));
                    return;
                }


                if (chk.OccupiedQQ == null)
                {
                    //尝试进行验证
                    u.Name = name;
                    if (u.Verified)
                    {
                        e.Reply(Strs.Get("rnOK")); // here set succeeded
                        //update in 3.0.14.2 notice the administration
                        S.Si(u.ToXml("旧人补填姓名成功"));
                        return;
                    }

                    e.Reply(Strs.Get("unexceptedErr", u.VerifyMsg));
                    UserInfo.CheckQmpAsync(u);
                }
            } // end manual name-filling handling
        }
    }
}