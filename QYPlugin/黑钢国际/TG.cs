using MihaZupan;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace Clansty.tianlang
{
    public static class TG
    {
        private static readonly string token = Rds.HGet("tgcfg", "token");
        private static readonly HttpToSocks5Proxy proxy = new HttpToSocks5Proxy("127.0.0.1", 1081);
        public static readonly TelegramBotClient bot = new TelegramBotClient(token, proxy);

        public static async Task InitAsync()
        {
            var me = await bot.GetMeAsync();
            Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");
            bot.OnMessage += Bot_OnMessage;
            bot.StartReceiving();
        }

        public static void RecordReplyInfo(string group, Message tmsg, string qseq, string qsrc)
        {
            Rds.HSet("qseq2tid" + group, qseq, tmsg.MessageId.ToString());
            Rds.HSet("tid2qsrc" + group, tmsg.MessageId.ToString(), qsrc);
        }
        public static int GetReplyInfo(string group, string qseq)
        {
            try
            {
                return int.Parse(Rds.HGet("qseq2tid" + group, qseq));
            }
            catch
            {
                return 0;
            }
        }
        public static string GetReplyInfo(string group, Message tmsg)
        {
            return Rds.HGet("tid2qsrc" + group, tmsg.MessageId.ToString());
        }

        public const long test = -1001242989028;
        public const long iDE = -1001218396541;

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Chat.Id == test || e.Message.Chat.Id == iDE)
            {
                string reply = "";
                string group = null;
                if (e.Message.Chat.Id == test)
                    group = "test";
                if (e.Message.Chat.Id == iDE)
                    group = "ide";
                if (e.Message.ReplyToMessage != null)
                    reply = GetReplyInfo(group, e.Message.ReplyToMessage);
                if (e.Message.Photo != null)
                {
                    PhotoSize ps = e.Message.Photo.Last();
                    File pf = await bot.GetFileAsync(ps.FileId);
                    WebClient web = new WebClient();
                    web.Proxy = proxy;
                    web.DownloadFile($"https://api.telegram.org/file/bot{token}/{pf.FilePath}", @"C:\ServiceHub\temp\" + pf.FileId);
                    Sd($"{e.Message.From.FirstName} On Telegram:" + LQ.LocalPic(@"C:\ServiceHub\temp\" + pf.FileId));
                    Thread.Sleep(15000);
                    System.IO.File.Delete(@"C:\ServiceHub\temp\" + pf.FileId);
                }
                else if (e.Message.Text != null && e.Message.Photo != null)
                {
                    PhotoSize ps = e.Message.Photo.Last();
                    File pf = await bot.GetFileAsync(ps.FileId);
                    WebClient web = new WebClient();
                    web.Proxy = proxy;
                    web.DownloadFile($"https://api.telegram.org/file/bot{token}/{pf.FilePath}", @"C:\ServiceHub\temp\" + pf.FileId);
                    Sd($"{e.Message.From.FirstName} On Telegram:\n{e.Message.Text}" + LQ.LocalPic(@"C:\ServiceHub\temp\" + pf.FileId));
                    Thread.Sleep(15000);
                    System.IO.File.Delete(@"C:\ServiceHub\temp\" + pf.FileId);
                }
                else if (e.Message.Sticker != null)
                {
                    Sticker ps = e.Message.Sticker;
                    File pf = await bot.GetFileAsync(ps.FileId);
                    WebClient web = new WebClient();
                    web.Proxy = proxy;
                    web.DownloadFile($"https://api.telegram.org/file/bot{token}/{pf.FilePath}", @"C:\ServiceHub\temp\" + pf.FileId);
                    Sd($"{e.Message.From.FirstName} On Telegram:" + LQ.LocalPic(@"C:\ServiceHub\temp\" + pf.FileId));
                    Thread.Sleep(15000);
                    System.IO.File.Delete(@"C:\ServiceHub\temp\" + pf.FileId);
                }
                else if (e.Message.Text != null)
                {
                    Sd($"{e.Message.From.FirstName} On Telegram:\n" +
                       $"{e.Message.Text}");
                }


                void Sd(string msg)
                {
                    if (e.Message.Chat.Id == test)
                        S.Test(reply + msg);
                    if (e.Message.Chat.Id == iDE)
                        S.IDE(reply + msg);
                }
            }
        }
    }
}
