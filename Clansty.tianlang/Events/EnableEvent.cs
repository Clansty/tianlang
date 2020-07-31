using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using ServiceStack.Redis;
using System.Threading.Tasks;

namespace Clansty.tianlang.Events
{
    public class EnableEvent : IAppEnable
    {
        public void AppEnable(object sender, CQAppEnableEventArgs e)
        {
            C.CQApi = e.CQApi;
            UserInfo.InitQmpCheckTask();
            UpdateList();
        }
        async void UpdateList()
        {
            await Task.Delay(2333);
            MemberList.UpdateMajor();
            MemberList.UpdateG2020();
            C.WriteLn("Memberlist updated");
        }
    }
}