using ServiceStack.Redis;
using System;
using System.Collections.Generic;

namespace Clansty.tianlang
{
    public static class Cmds
    {
        public static readonly Dictionary<string, GroupCommand> gcmds = new Dictionary<string, GroupCommand>()
        {
            ["info"] = new GroupCommand()
            {
                Description = "查询一个用户的信息",
                Usage = "info [QQ号]",
                IsParamsNeeded = true,
                Permission = UserType.powerUser,
                Func = s =>
                {
                    s = s.Trim(' ', '\n');
                    if ((!ulong.TryParse(s, out _)) || s == "")
                        return "QQ号格式错误";
                    User u = new User(s);
                    return u.ToXml();
                }
            },
            ["help"] = new GroupCommand()
            {
                Description = "显示命令行帮助",
                Usage = "help",
                IsParamsNeeded = false,
                Permission = UserType.user,
                Func = _ =>
                {
                    string r = "";
                    foreach (KeyValuePair<string, GroupCommand> cmd in gcmds)
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
            ["hset"] = new GroupCommand()
            {
                Description = "设置数据库中某个哈希链表中某一项的值",
                Usage = "hset [哈希链表 ID] [项 ID] [值]",
                IsParamsNeeded = true,
                Permission = UserType.administrator,
                Func = s =>
                {
                    if (s.IndexOf(" ") == s.LastIndexOf(" "))
                    {
                        return "参数的数量不够";
                    }
                    string hashid = s.GetLeft(" ");
                    s = s.GetRight(" ");
                    string keyid = s.GetLeft(" ");
                    s = s.GetRight(" ");
                    return $"完成，返回值为 {Rds.HSet(hashid, keyid, s)}";
                }
            },
            ["hget"] = new GroupCommand()
            {
                Description = "获取数据库中某个哈希链表中某一项的值",
                Usage = "hget [哈希链表 ID] [项 ID]",
                IsParamsNeeded = true,
                Permission = UserType.powerUser,
                Func = s =>
                {
                    if (s.IndexOf(" ") < 0)
                    {
                        return "参数的数量不够";
                    }
                    string hashid = s.GetLeft(" ");
                    s = s.GetRight(" ");
                    return Rds.HGet(hashid, s);
                }
            },
            ["rqns"] = new GroupCommand()
            {
                Description = "手动对某个成员启动新人向导",
                Usage = "rqns [QQ号 | @人]",
                IsParamsNeeded = true,
                Permission = UserType.powerUser,
                Func = s =>
                {
                    s = s.Trim(' ', '\n', '[', ']', '@');
                    if (long.TryParse(s, out _))
                    {
                        Setup.New(s, false);
                        return $"新人向导已对 {new User(s).Namecard}({s}) 启动";
                    }
                    return $"{s} 不是有效的长整数";
                }
            },
            ["blk"] = new GroupCommand()
            {
                Description = "拉黑并踢出某个人，被拉黑的人再次加群会被自动拒绝。可以通过大群命令 sudo blk [人] 来快速使用",
                Usage = "blk [QQ号 | @人]",
                IsParamsNeeded = true,
                Permission = UserType.powerUser,
                Func = s =>
                {
                    s = s.Trim(' ', '\n', '[', ']', '@');
                    if (long.TryParse(s, out _))
                    {
                        User u = new User(s);
                        if (u.Role >= UserType.powerUser)
                            return $"不能拉黑一个 {u.Role}";
                        u.Role = UserType.blackListed;
                        Robot.Group.RemoveMember(G.major, s);
                        return $"已拉黑 {u.ProperNamecard}({s})";
                    }
                    return $"{s} 不是有效的长整数";
                }
            },
            ["t"] = new GroupCommand()
            {
                Description = "踢出某个人，再次加群不会被自动拒绝。可以通过大群命令 sudo t [人] 来快速使用",
                Usage = "t [QQ号 | @人]",
                IsParamsNeeded = true,
                Permission = UserType.powerUser,
                Func = s =>
                {
                    s = s.Trim(' ', '\n', '[', ']', '@');
                    if (long.TryParse(s, out _))
                    {
                        User u = new User(s);
                        if (u.Role >= UserType.powerUser)
                            return $"不能踢一个 {u.Role}";
                        Robot.Group.RemoveMember(G.major, s);
                        return $"已踢 {u.ProperNamecard}({s})";
                    }
                    return $"{s} 不是有效的长整数";
                }
            },
            ["echo"] = new GroupCommand()
            {
                Description = "原样输出文本消息或者处理 XML 或 JSON 结构化消息",
                Usage = "echo [要输出的内容]",
                IsParamsNeeded = true,
                Permission = UserType.powerUser,
                Func = s => s
            },
            ["gstrs"] = new GroupCommand()
            {
                Description = "获取语句模板文本",
                Usage = "gstrs [模板 ID]",
                IsParamsNeeded = true,
                Permission = UserType.powerUser,
                Func = Strs.Get
            },
            ["sstrs"] = new GroupCommand()
            {
                Description = "设置语句模板文本",
                Usage = "sstrs [模板 ID] [内容]",
                IsParamsNeeded = true,
                Permission = UserType.administrator,
                Func = s =>
                {
                    if (s.IndexOf(" ") < 0)
                    {
                        return "参数的数量不够";
                    }
                    string hashid = s.GetLeft(" ");
                    s = s.GetRight(" ");
                    Strs.Set(hashid, s);
                    return "完成";
                }
            },
            ["strs"] = new GroupCommand()
            {
                Description = "获取现在所有的语句模板",
                Usage = "strs",
                IsParamsNeeded = false,
                Permission = UserType.powerUser,
                Func = _ =>
                {
                    IRedisClient client = Rds.GetClient();
                    string r = "";
                    foreach (string cmd in client.GetHashKeys("strs"))
                        r += cmd + "\n";
                    r = r.Trim();
                    client.Dispose();
                    return r;
                }
            },
            ["stats"] = new GroupCommand()
            {
                Description = "查看统计数据",
                Usage = "stats",
                IsParamsNeeded = false,
                Permission = UserType.powerUser,
                Func = _ =>
                {
                    string r = "";
                    string date = DateTime.Now.ToShortDateString();
                    IRedisClient client = Rds.GetClient();
                    Dictionary<string, string> kvps = client.GetAllEntriesFromHash("stats" + date);
                    client.Dispose();
                    int tot = 0;
                    foreach (KeyValuePair<string, string> kvp in kvps)
                    {
                        r += $"{kvp.Key}: {kvp.Value}\n";
                        tot += int.Parse(kvp.Value);
                    }
                    r += $"合计: {tot}";
                    return r;
                }
            },
        };
        public static void SiEnter(GroupMsgArgs e)
        {
            try
            {
                string key = (e.Msg.GetLeft(" ") == "" ? e.Msg : e.Msg.GetLeft(" ")).UnEscape().ToLower();
                string act = e.Msg.GetRight(" ").UnEscape();
                if (gcmds.ContainsKey(key))
                {
                    GroupCommand m = gcmds[key];
                    User u = new User(e.FromQQ);
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
        public static void SudoEnter(GroupMsgArgs e)
        {
            try
            {
                string s = e.Msg.GetRight("sudo ").Trim();
                string key = (s.GetLeft(" ") == "" ? s : s.GetLeft(" ")).ToLower();
                string act = s.GetRight(" ");
                if (gcmds.ContainsKey(key))
                {
                    GroupCommand m = gcmds[key];
                    User u = new User(e.FromQQ);
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
