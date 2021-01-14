using CornSDK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Mirai_CSharp;
using Mirai_CSharp.Models;
using Telegram.Bot;

namespace Clansty.tianlang
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        static async Task Main()
        {
            Console.CancelKeyPress += Exit;
            Console.Title = $@"ClanstyBots Server {C.Version}";
            //init telegram bots
#if !DEBUG
            C.TG = new TelegramBotClient(Tg.Token);
            C.TG.OnMessage += Tg.OnMsg;
            C.TG.StartReceiving();
#endif
            //init qq bots
            var nthsBotHandler = new NthsBotEvents();
            var privateHandler = new PrivateEvents();
            C.QQ.NthsBot = new MiraiHttpSession();
            C.QQ.NthsBot.AddPlugin(nthsBotHandler);
            C.QQ.Clansty = new MiraiHttpSession();
            //init system component
            Db.Init();
            Timers.Init();
            MemberList.UpdateMajor();
            if (File.Exists("/tmp/tlupdate"))
            {
                var oldver = File.ReadAllText("/tmp/tlupdate").Trim(' ', '\r', '\n');
                File.Delete("/tmp/tlupdate");
                try
                {
                    var p = Process.Start(new ProcessStartInfo("git", "log -1")
                    {
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        WorkingDirectory = "/root/nthsbot"
                    });
                    p.WaitForExit();
                    var ret = p.StandardOutput.ReadToEnd() + p.StandardError.ReadToEnd();
                    S.Si($"更新已完成:\n{oldver} -> {C.Version}\n{ret.Trim()}");
                }
                catch
                {
                    S.Si($"更新已完成:\n{oldver} -> {C.Version}");
                }
            }

            while (true)
            {
                var em = Console.ReadLine();
                if (em is null)
                    continue;
#if DEBUG
                var key = (em.GetLeft(" ") == "" ? em : em.GetLeft(" ")).ToLower();
                var act = em.GetRight(" ");
                if (Cmds.gcmds.ContainsKey(key))
                {
                    var m = Cmds.gcmds[key];
                    C.WriteLn(Cmds.gcmds[key].Func(act));
                }
#else
                try
                {
                    var key = (em.GetLeft(" ") == "" ? em : em.GetLeft(" ")).ToLower();
                    var act = em.GetRight(" ");
                    if (Cmds.gcmds.ContainsKey(key))
                    {
                        var m = Cmds.gcmds[key];
                        C.WriteLn(Cmds.gcmds[key].Func(act));
                    }
                }
                catch (Exception ex)
                {
                    C.WriteLn(ex, ConsoleColor.Red);
                }
#endif
            }
        }

        internal static void Exit(object a = null, object b = null)
        {
            Db.Commit();
            var tt = Process.GetProcessById(Process.GetCurrentProcess().Id);
            tt.Kill();
        }
    }
}