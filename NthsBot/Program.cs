using Ac682.Hyperai.Clients.Mirai;
using Hyperai.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;

namespace NthsBot
{
    class Program
    {
        static void Main(string[] args)
        {
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

            while (true)
            {
                var input = Console.ReadLine();
                //TODO 输入处理
            }
        }
    }
}
