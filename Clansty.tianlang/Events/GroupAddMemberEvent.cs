using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;

namespace Clansty.tianlang.Events
{
    public class GroupAddMemberEvent : IGroupMemberIncrease
    {
        public void GroupMemberIncrease(object sender, CQGroupMemberIncreaseEventArgs e)
        {
            if (e.FromGroup == G.major)
            {
                MemberList.major.Add(e.BeingOperateQQ);
                var u = new User(e.BeingOperateQQ);
                if (u.IsFresh)
                {
                    Setup.New(e.BeingOperateQQ, true);
                }
                else
                {
                    u.Namecard = u.ProperNamecard;
                    u.Status = Status.no;
                    var tip = Strs.Get($"setupTip{u.Enrollment}"); //根据年级来分的提示
                    if (tip != "")
                        S.Private(e.BeingOperateQQ, tip); //你是高一的，建议同时加入 2018 级新高一年级群 //??2019级新生即将来临?敬请期待??? 
                    tip = Strs.Get("setupTipAll"); //给所有人的广告位
                    if (tip != "")
                        S.Private(e.BeingOperateQQ, tip); //目前是这个：【西花园事务所】是江苏省苏州第十中学校学生自建生活服务平台，添加【西花园事务所】为特别关心，可以第一时间收到最新消息 
                    //S.Private(member, u.ToXml("欢迎回来"));
                    //S.Private(member, Strs.Get("welcomeBack"));//"如果以上信息有误，你可以回复 <setup> 来重新设置"
                    S.Si(u.ToXml("数据库存在此人信息，新人向导跳过"));
                    UserInfo.CheckQmpAsync(u);
                }
            }

            if (e.FromGroup == G.g2020)
            {
                MemberList.g2020.Add(e.BeingOperateQQ);
                var u = new User(e.BeingOperateQQ);
                UserInfo.CheckG2020QmpAsync(u);
                if (!MemberList.major.Contains(u.Uin))
                {
                    S.Private(e.BeingOperateQQ, "看起来你还没有加入十中大群的说\n加入苏州十中跨年级大群 ", G.major, "，解锁更多好玩的");
                }
            }
        }
    }
}