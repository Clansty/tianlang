using CornSDK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Clansty.tianlang
{
    static class Cmds
    {
        internal static readonly Dictionary<string, GroupCommand> gcmds = new Dictionary<string, GroupCommand>
        {
            ["info"] = new GroupCommand
            {
                Description = "查询一个用户的信息",
                Usage = "info [QQ号]",
                IsParamsNeeded = true,
                Permission = UserType.powerUser,
                Func = s =>
                {
                    s = s.Trim(' ', '\n', '[', ']', '@', 'L', 'Q', ':');
                    if (!long.TryParse(s, out var i) || s == "")
                        return "QQ号格式错误";
                    var u = new User(i, false);
                    UserInfo.CheckQmpAsync(u);
                    return u.ToString();
                }
            },
            ["help"] = new GroupCommand
            {
                Description = "显示命令行帮助",
                Usage = "help",
                IsParamsNeeded = false,
                Permission = UserType.user,
                Func = _ =>
                {
                    var r = "";
                    foreach (var cmd in gcmds)
                    {
                        r += $"{cmd.Value.Usage}\n" +
                             $"{cmd.Value.Description}\n" +
                             $"需要参数: {cmd.Value.IsParamsNeeded}\n" +
                             $"需要的权限级别: {cmd.Value.Permission}\n" +
                             "---\n";
                    }

                    r += $"甜狼 {C.Version}";
                    return r;
                }
            },
            ["rqns"] = new GroupCommand
            {
                Description = "手动对某个成员启动新人向导",
                Usage = "rqns [QQ号 | @人]",
                IsParamsNeeded = true,
                Permission = UserType.powerUser,
                Func = s =>
                {
                    s = s.Trim(' ', '\n', '[', ']', '@', 'L', 'Q', ':');
                    if (long.TryParse(s, out var i))
                    {
                        Setup.New(i);
                        return $"新人向导已对 {new User(i).Namecard}({s}) 启动";
                    }

                    return $"{s} 不是有效的长整数";
                }
            },
            ["blk"] = new GroupCommand
            {
                Description = "拉黑并踢出某个人，被拉黑的人再次加群会被自动拒绝。可以通过大群命令 sudo blk [人] 来快速使用",
                Usage = "blk [QQ号 | @人]",
                IsParamsNeeded = true,
                Permission = UserType.powerUser,
                Func = s =>
                {
                    s = s.Trim(' ', '\n', '[', ']', '@', 'L', 'Q', ':');
                    if (long.TryParse(s, out var i))
                    {
                        var u = new User(i);
                        if (u.Role >= UserType.powerUser)
                            return $"不能拉黑一个 {u.Role}";
                        u.Role = UserType.blackListed;
                        C.QQ.GroupKickMember(G.major, i); //todo
                        return $"已拉黑 {u.ProperNamecard}({s})";
                    }

                    return $"{s} 不是有效的长整数";
                }
            },
            ["t"] = new GroupCommand
            {
                Description = "踢出某个人，再次加群不会被自动拒绝。可以通过大群命令 sudo t [人] 来快速使用",
                Usage = "t [QQ号 | @人]",
                IsParamsNeeded = true,
                Permission = UserType.powerUser,
                Func = s =>
                {
                    s = s.Trim(' ', '\n', '[', ']', '@', 'L', 'Q', ':');
                    if (long.TryParse(s, out var i))
                    {
                        var u = new User(i);
                        if (u.Role >= UserType.powerUser)
                            return $"不能踢一个 {u.Role}";
                        C.QQ.GroupKickMember(G.major, i);
                        return $"已踢 {u.ProperNamecard}({s})";
                    }

                    return $"{s} 不是有效的长整数";
                }
            },
            ["echo"] = new GroupCommand
            {
                Description = "原样输出文本消息或者处理 XML 或 JSON 结构化消息",
                Usage = "echo [要输出的内容]",
                IsParamsNeeded = true,
                Permission = UserType.powerUser,
                Func = s => s
            },
            ["find"] = new GroupCommand
            {
                Description = "通过姓名查人",
                Usage = "find [姓名]",
                IsParamsNeeded = true,
                Permission = UserType.powerUser,
                Func = s =>
                {
                    var ps = Person.GetPeople(s);
                    var urs = Db.users.Select($"name='{s}'");
                    var uls = new List<long>();
                    var ss = new List<string>();
                    foreach (var u in urs)
                    {
                        uls.Add((long) u["id"]);
                        ss.Add(new User((long) u["id"]).ToString());
                    }

                    foreach (var p in ps)
                    {
                        if (p.User != null)
                            if (uls.Contains(p.User.Uin))
                                continue;
                        ss.Add(p.ToString());
                    }

                    var r = string.Join("\n------\n", ss);
                    return string.IsNullOrWhiteSpace(r) ? "无结果" : r;
                }
            },
            ["set"] = new GroupCommand()
            {
                Description = "修改某个人的信息",
                Usage = "set [QQ号] [{name|nick|enrollment|junior|prefix|step|status|role|tguid}] [值]",
                IsParamsNeeded = true,
                Permission = UserType.powerUser,
                Func = s =>
                {
                    var arg = s.Split(new char[] {' '}, 3);
                    var u = new User(long.Parse(arg[0]));
                    switch (arg[1])
                    {
                        case "junior":
                            var v = bool.Parse(arg[2]);
                            u.Row[arg[1]] = v;
                            break;
                        case "name":
                        case "nick":
                        case "prefix":
                            u.Row[arg[1]] = arg[2];
                            break;
                        case "enrollment":
                        case "step":
                        case "Status":
                        case "role":
                            var i = int.Parse(arg[2]);
                            u.Row[arg[1]] = i;
                            break;
                        case "tguid":
                            var l = long.Parse(arg[2]);
                            u.Row["tg"] = l;
                            break;
                        default:
                            return "字段无效";
                    }

                    var chkQmp = UserInfo.CheckQmpAsync(u);
                    return $"操作成功，群名片 {chkQmp.Result}";
                }
            },
            ["sync"] = new GroupCommand()
            {
                Description = "将对数据库的修改提交到 MySQL",
                Usage = "sync",
                IsParamsNeeded = false,
                Permission = UserType.powerUser,
                Func = s =>
                {
                    Db.Commit();
                    return "\\u2713";
                }
            },
            ["chkqmp"] = new GroupCommand()
            {
                Description = "扫描全群的群名片，请勿频繁使用",
                Usage = "chkqmp",
                IsParamsNeeded = false,
                Permission = UserType.administrator,
                Func = s =>
                {
                    UserInfo.CheckAllQmpAsync();
                    return "已开始扫描";
                }
            },
            ["exit"] = new GroupCommand()
            {
                Description = "结束甜狼进程",
                Usage = "exit",
                IsParamsNeeded = false,
                Permission = UserType.console,
                Func = s =>
                {
                    Program.Exit();
                    return null;
                }
            },
            ["update"] = new GroupCommand()
            {
                Description = "从 Git 存储库编译并更新自己",
                Usage = "update",
                IsParamsNeeded = false,
                Permission = UserType.administrator,
                Func = s =>
                {
                    Db.Commit();
                    var p = Process.Start(new ProcessStartInfo("git", "pull")
                    {
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        WorkingDirectory = "/root/nthsbot"
                    });
                    p.WaitForExit();
                    var ret = p.StandardOutput.ReadToEnd() + p.StandardError.ReadToEnd();
                    if (p.ExitCode != 0)
                        throw new Exception(ret.Trim(' ', '\r', '\n'));
                    ret += "\n三秒后重启...";
                    File.WriteAllText("/tmp/tlupdate", C.Version);
                    Task.Run(() =>
                    {
                        Thread.Sleep(3000);
                        Process.Start("supervisorctl", "restart nthsbot");
                    });
                    return ret.Trim(' ', '\r', '\n');
                }
            },
            ["shell"] = new GroupCommand()
            {
                Description = "运行 shell 命令（危",
                Usage = "shell",
                IsParamsNeeded = false,
                Permission = UserType.administrator,
                Func = s =>
                {
                    if (s.StartsWith("rm"))
                        throw new Exception("操作被禁止");
                    if (s.StartsWith("sudo rm"))
                        throw new Exception("操作被禁止");
                    var args = s.Split(' ', 2);
                    if (args.Length < 1)
                        return "参数不够";
                    Process p;
                    if (args.Length == 2)
                        p = Process.Start(new ProcessStartInfo(args[0], args[1])
                        {
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            WorkingDirectory = "/root/nthsbot"
                        });

                    else
                        p = Process.Start(new ProcessStartInfo(args[0])
                        {
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            WorkingDirectory = "/root/nthsbot"
                        });
                    p.WaitForExit();
                    var ret = p.StandardOutput.ReadToEnd() + p.StandardError.ReadToEnd();
                    if (p.ExitCode != 0)
                        throw new Exception(ret.Trim(' ', '\r', '\n'));
                    return ret;
                }
            },
            ["logecho"] = new GroupCommand
            {
                Description = "原样输出文本消息并在控制台打印",
                Usage = "logecho [要输出的内容]",
                IsParamsNeeded = true,
                Permission = UserType.powerUser,
                Func = s =>
                {
                    C.WriteLn(s);
                    return s;
                }
            },
            ["reload"] = new GroupCommand()
            {
                Description = "重载配置",
                Usage = "reload",
                IsParamsNeeded = false,
                Permission = UserType.administrator,
                Func = s =>
                {
                    Db.Commit();
                    Db.Init(true);
                    return "重载完成";
                }
            },
        };

        internal static void Enter(string msg, long user, bool isMajor)
        {
            try
            {
                msg = msg.Trim();
                var key = (msg.GetLeft(" ") == "" ? msg.ToLower() : msg.GetLeft(" ")).ToLower();
                var act = "";
                if (msg.Contains(" "))
                    act = msg.GetRight(" ");
                key = key.Trim(' ', '\r', '\n');
                act = act.Trim(' ', '\r', '\n');
                if (gcmds.ContainsKey(key))
                {
                    var m = gcmds[key];
                    var u = new User(user);
                    if (u.Role < m.Permission)
                    {
                        var toSend = $"权限不够\n{key} 需要 {m.Permission}，而你属于{u.Role}";
                        if (isMajor)
                        {
                            S.Major(toSend);
                        }
                        else
                        {
                            S.Si(toSend);
                        }

                        return;
                    }

                    if (act.Trim() == "" && m.IsParamsNeeded)
                    {
                        var toSend = $"{key} 命令需要提供参数\n{m.Description}\n用法: \n{m.Usage}";
                        if (isMajor)
                        {
                            S.Major(toSend);
                        }
                        else
                        {
                            S.Si(toSend);
                        }

                        return;
                    }

                    var ret = gcmds[key].Func(act).Trim(' ', '\r', '\n');
                    var res = ret.Split('\n');
                    if (res.Length == 1 && res[0] == "")
                    {
                        const string toSend = "返回内容为空";
                        if (isMajor)
                        {
                            S.Major(toSend);
                        }
                        else
                        {
                            S.Si(toSend);
                        }

                        return;
                    }

                    if (isMajor)
                    {
                        S.TG.Major(ret);
                    }
                    else
                    {
                        S.TG.Si(ret);
                    }

                    var lines = 0;
                    var comb = ""; //字符串十行十行的发
                    foreach (var line in res)
                    {
                        lines++;
                        if (lines == 20)
                        {
                            comb += line;
                            if (isMajor)
                            {
                                S.Major(comb, false);
                            }
                            else
                            {
                                S.Si(comb, false);
                            }

                            Thread.Sleep(500);
                            lines = 0;
                            comb = "";
                        }
                        else
                        {
                            comb += line;
                            comb += "\n";
                        }
                    }

                    comb = comb.Trim('\r', '\n');
                    if (comb.Trim() != "")
                        if (isMajor)
                        {
                            S.Major(comb, false);
                        }
                        else
                        {
                            S.Si(comb, false);
                        }
                }
                else if (isMajor)
                {
                    S.Major(Strs.CmdNotFound);
                }
            }
            catch (Exception ex)
            {
                var toSend = "发生错误\n" + ex;
                if (isMajor)
                {
                    S.Major(toSend);
                }
                else
                {
                    S.Si(toSend);
                }
            }
        }
    }
}