using CornSDK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Telegram.Bot;

namespace Clansty.tianlang
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        static void Main()
        {
// #if DEBUG
//             Test.Do();
// #else
            Console.CancelKeyPress += Exit;
            Console.Title = $@"甜狼 {C.Version}";
            var nthsBotHandler = new NthsBotEvents();
            var privateHandler = new PrivateEvents();
            C.TG = new TelegramBotClient(Tg.Token);
            C.TG.OnMessage += Tg.OnMsg;
            Db.Init();
#if !DEBUG
            FireList.Init();
            Timers.Init();

            C.QQ = new Corn(new CornConfig()
            {
                ip = "172.17.11.76",
                listenIp = "172.17.11.73",
                listenPort = 7284,
                handlers = new Dictionary<long, ICornEventHandler>()
                {
                    [C.self] = nthsBotHandler,
                    [839827911] = privateHandler,
                    [2603367939] = privateHandler
                },
                logger = C.logger
            });
            C.TG.StartReceiving();
            MemberList.UpdateMajor();
#endif
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
            System.Diagnostics.Process tt =
                System.Diagnostics.Process.GetProcessById(System.Diagnostics.Process.GetCurrentProcess().Id);
            tt.Kill();
        }
    }
}