using Ac682.Hyperai.Clients.Mirai;
using Clansty.tianlang;
using Hyperai.Serialization;
using Microsoft.Extensions.Logging;
using System;

namespace NthsBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = $"甜狼 {C.Version}";
            var options = new MiraiClientOptions()
            {
                AuthKey = "QwQQAQOwO",
                Port = 8080,
                Host = "localhost",
                SelfQQ = 2981882373
            };
            var loggerFactory = LoggerFactory.Create(builder => {
                builder.AddConsole();
            }); 
            var client = new MiraiClient(options, loggerFactory.CreateLogger<MiraiClient>(), new HyperCodeFormatter());
            client.Connect();
            UserInfo.InitQmpCheckTask();
            while (true)
            {
                var input = Console.ReadLine();
                //TODO 输入处理
            }
        }
    }
}
