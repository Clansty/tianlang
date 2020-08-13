using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                    s = s.Trim(' ', '\n', '[', ']', '@');
                    if (!long.TryParse(s, out var i) || s == "")
                        return "QQ号格式错误";
                    var u = new User(i);
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
                    s = s.Trim(' ', '\n', '[', ']', '@');
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
                    s = s.Trim(' ', '\n', '[', ']', '@');
                    if (long.TryParse(s, out var i))
                    {
                        var u = new User(i);
                        if (u.Role >= UserType.powerUser)
                            return $"不能拉黑一个 {u.Role}";
                        u.Role = UserType.blackListed;
                        Robot.GroupKickMember(G.major,i);
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
                    s = s.Trim(' ', '\n', '[', ']', '@');
                    if (long.TryParse(s, out var i))
                    {
                        var u = new User(i);
                        if (u.Role >= UserType.powerUser)
                            return $"不能踢一个 {u.Role}";
                        Robot.GroupKickMember(G.major,i);
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
                    s = s.Trim(' ', '\n', '[', ']', '@');
                    var u = UserInfo.FindUser(s);
                    return u.ToString();
                }
            },
        };
        internal static void SiEnter(GroupMsgArgs e)
        {
            try
            {
                var key = (e.Msg.GetLeft(" ") == "" ? e.Msg.ToLower() : e.Msg.GetLeft(" ")).ToLower();
                var act = e.Msg.GetRight(" ");
                key = key.Trim(' ', '\r', '\n');
                act = act.Trim(' ', '\r', '\n');
                if (gcmds.ContainsKey(key))
                {
                    var m = gcmds[key];
                    var u = new User(e.FromQQ);
                    if (u.Role < m.Permission)
                    {
                        e.Reply($"权限不够\n{key} 需要 {m.Permission}，而你属于{u.Role}", true);
                        return;
                    }
                    if (act.Trim() == "" && m.IsParamsNeeded)
                    {
                        e.Reply($"{key} 命令需要提供参数\n{m.Description}\n用法: \n{m.Usage}", true);
                        return;
                    }
                    e.Reply(gcmds[key].Func(act), true);
                }

            }
            catch (Exception ex)
            {
                e.Reply(ex.Message);
            }
        }
        internal static void SudoEnter(GroupMsgArgs e)
        {
            try
            {
                var s = e.Msg.GetRight("sudo ").Trim();
                var key = (s.GetLeft(" ") == "" ? s.ToLower() : s.GetLeft(" ")).ToLower();
                var act = s.GetRight(" ");
                key = key.Trim(' ', '\r', '\n');
                act = act.Trim(' ', '\r', '\n');
                if (gcmds.ContainsKey(key))
                {
                    var m = gcmds[key];
                    var u = new User(e.FromQQ);
                    if (u.Role < m.Permission)
                    {
                        e.Reply($"权限不够\n{key} 需要 {m.Permission}，而你属于{u.Role}, true");
                        return;
                    }
                    if (act.Trim() == "" && m.IsParamsNeeded)
                    {
                        e.Reply($"{key} 命令需要提供参数\n{m.Description}\n用法: \n{m.Usage}", true);
                        return;
                    }
                    e.Reply(gcmds[key].Func(act), true);
                }
                else
                    e.Reply("命令找不到");
            }
            catch (Exception ex)
            {
                e.Reply(ex.Message);
            }
        }

    }
}
