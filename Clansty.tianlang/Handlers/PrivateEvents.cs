using System;
using System.Threading.Tasks;
using Mirai_CSharp;
using Mirai_CSharp.Extensions;
using Mirai_CSharp.Models;
using Mirai_CSharp.Plugin.Interfaces;

namespace Clansty.tianlang
{
    public class PrivateEvents : IGroupMessage, IDisconnected
    {
        private const long SELF = 839827911;

        public async Task<bool> GroupMessage(MiraiHttpSession session, IGroupMessageEventArgs e)
        {
            Q2Tg.NewGroupMsg(
                session, 
                e.Sender.Group.Id, 
                e.Sender.Id, 
                e.Sender.Name, 
                e.Chain);
            
            if (e.Sender.Group.Id == G.check)
            {
                Watchdog.Msg(SELF, e.Sender.Id, e.Chain.GetPlain());
            }

            if (e.Sender.Group.Id == G.parents)
            {
                var chainToSend = new MessageBuilder();
                chainToSend.AddPlainMessage(e.Sender.Name + ":\n");
                foreach (var i in e.Chain)
                {
                    chainToSend.Add(i);
                }

                session.SendGroupMessageAsync(G.parentsFwd, chainToSend);
            }

            return false;
        }

        public async Task<bool> Disconnected(MiraiHttpSession session, IDisconnectedEventArgs e)
        {
            while (true)
            {
                try
                {
                    await session.ConnectAsync(C.miraiSessionOpinions, SELF);
                    return true;
                }
                catch (Exception)
                {
                    await Task.Delay(1000);
                }
            }
        }
    }
}