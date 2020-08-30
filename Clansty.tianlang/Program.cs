using CornSDK;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clansty.tianlang
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            Test.Do();
#else
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Console.CancelKeyPress += delegate
            {
                Db.Commit();
                System.Diagnostics.Process tt = System.Diagnostics.Process.GetProcessById(System.Diagnostics.Process.GetCurrentProcess().Id);
                tt.Kill();
            };
            Console.Title = $@"ΜπΐΗ {C.Version}";
            var handler = new Events();
            C.Robot = new Corn(new CornConfig()
            {
                selfQQ = C.self,
                friendMsgHandler = handler,
                groupAddMemberHandler = handler,
                groupInviteRequestHandler = handler,
                groupJoinRequestHandler = handler,
                groupMsgHandler = handler,
                friendRequestHandler = handler,
                tempMsgHandler = handler
            });
            Db.Init();
            Task.Run(() =>
            {
                while (true)
                {
                    var em = Console.ReadLine();
                    if (em is null)
                        continue;
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
                        C.WriteLn(ex.Message, ConsoleColor.Red);
                    }
                }
            });
            Application.Run(new Menu());
#endif
        }
    }
}
