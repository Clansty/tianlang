using System;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;

namespace Clansty.tianlang.Events
{
    public class StartupEvent : ICQStartup
    {
        public void CQStartup(object sender, CQStartupEventArgs e)
        {
            C.CQApi = e.CQApi;
            C.AllocConsole();
            Console.Title = $@"甜狼 {C.Version}";
            new Menu().Show();
        }
    }
}