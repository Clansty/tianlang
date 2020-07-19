using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mirai_CSharp.Models;
using Mirai_CSharp.Plugin.Interfaces;

namespace Mirai_CSharp.Tianlang
{
    class GroupMessageHandler : IGroupMessage
    {
        public async Task<bool> GroupMessage(MiraiHttpSession session, IGroupMessageEventArgs e)
        {
            Console.WriteLine(e.Raw);
            Repeater.Enter(string.Join(null, e.Raw));
            return false;
        }
    }
}
