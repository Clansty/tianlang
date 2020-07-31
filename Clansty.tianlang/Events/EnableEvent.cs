using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using System;

namespace Clansty.tianlang.Events
{
    public class EnableEvent : IAppEnable
    {
        public void AppEnable(object sender, CQAppEnableEventArgs e)
        {
            UserInfo.InitQmpCheckTask();
        }
    }
}