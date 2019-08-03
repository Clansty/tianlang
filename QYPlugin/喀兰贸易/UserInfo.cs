﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Clansty.tianlang
{
    static class UserInfo //用户信息辅助类
    {
        private static System.Timers.Timer t;
        public static void InitQmpCheckTask()
        {
            t = new System.Timers.Timer(1000 * 60 * 60);
            t.Elapsed += (_, __) =>
            {
                Fired.CronAsync();
                CheckAllQmpAsync();
            };
            t.AutoReset = true;
            t.Enabled = true;
            C.WriteLn("init timer");
        }

        public static int ParseEnrollment(string s)
        {
            if (s == null || s == "")
                return -1;
            if (s.StartsWith("<"))
                s = s.GetRight(">");
            s = s.Trim();
            int m = -1;
            s = s.Replace("丨", " | ");
            s = s.Replace("｜", " | ");
            if (s.IndexOf('|') > -1)
                m = ParseEnrollment(s.GetLeft("|"));
            if (m > 0)
                return m;
            if (s.IndexOf("高二") > -1 || s.IndexOf("高2") > -1)
                return 2018;
            if (s.IndexOf("高一") > -1 || s.IndexOf("高1") > -1)
                return 2019;
            if (s.IndexOf("初三") > -1 || s.IndexOf("高三") > -1 || s.IndexOf("高3") > -1 || s.IndexOf("初3") > -1)
                return 2017;
            if (s.IndexOf("美高") > -1)
                return 10001;
            if (s.IndexOf("届") > -1)
                s = s.GetLeft("届");
            s = s.Trim();
            bool i = int.TryParse(s, out m);
            if (i && m > 100)
                return m - 3;
            if (i && m > 0)
                return m + 1997;
            return -1;
        }
        public static bool ParseJunior(string s)
        {
            if (s == null || s == "")
                return false;
            if (s.StartsWith("<"))
                s = s.GetRight(">");
            s = s.Trim();
            s = s.Replace("丨", " | ");
            s = s.Replace("｜", " | ");
            if (s.IndexOf('|') > -1)
                return ParseJunior(s.GetLeft("|"));
            if (s.IndexOf("初中") > -1 || s.IndexOf("初三") > -1 || s.IndexOf("初3") > -1)
                return true;
            return false;
        }
        public static bool ParseBranch(string s) => s.IndexOf("金阊") > -1;
        public static string ParseNick(string s)
        {
            if (s == null || s == "")
                return "";
            if (s.StartsWith("<"))
                s = s.GetRight(">");
            s = s.Trim();
            s = s.Replace("丨", " | ");
            s = s.Replace("｜", " | ");
            if (s.IndexOf("|") > -1)
                s = s.GetRight("|").Trim();
            else if (s.IndexOf("高一") > -1)
                s = s.GetRight("高一").Trim();
            else if (s.IndexOf("高二") > -1)
                s = s.GetRight("高二").Trim();
            else if (s.IndexOf("高三") > -1)
                s = s.GetRight("高三").Trim();
            else if (s.IndexOf("高一/2019届初中") > -1)
                s = s.GetRight("高一/2019届初中").Trim();
            else if (s.IndexOf("初二") > -1)
                s = s.GetRight("初二").Trim();
            else if (s.IndexOf("初三") > -1)
                s = s.GetRight("初三").Trim();
            else if (s.IndexOf("初中") > -1)
                s = s.GetRight("初中").Trim();
            else if (s.IndexOf("届") > -1)
                s = s.GetRight("届").Trim();
            else if (s.IndexOf("美高班") > -1)
                s = s.GetRight("美高班").Trim();
            else if (s.IndexOf("金阊") > -1)
                s = s.GetRight("金阊").Trim();
            return s;
        }
        public static async Task<CheckQmpRes> CheckQmpAsync(User u)
        {
            return await Task.Run(() =>
            {
                string card = u.Namecard;
                if (card.StartsWith("<") && card.IndexOf(">") > -1)
                    card = card.GetRight(">");

                if (card == u.ProperNamecard)
                {
                    return CheckQmpRes.noNeed;
                }

                u.Nick = ParseNick(card);
                if (u.Enrollment != 10086 && ParseEnrollment(card) > 1970)
                    u.Enrollment = ParseEnrollment(card);
                u.Junior = ParseJunior(card);
                u.Branch = ParseBranch(card);

                if (u.Enrollment > 1970 && u.Status == Status.setup)
                {
                    if (u.Step > 1)
                        return CheckQmpRes.noNeed;
                    u.Status = Status.no;
                    S.Private(u.Uin, Strs.Get("setupSelfSetOK"));
                    S.Si(u.ToXml("新人手动改名片了"));
                }

                if (card == u.ProperNamecard)
                {
                    C.WriteLn($"{u.Uin} updated", System.ConsoleColor.DarkGreen);
                    return CheckQmpRes.updated;
                }

                u.Namecard = u.ProperNamecard;

                C.WriteLn($"{u.Uin} updated", System.ConsoleColor.Yellow);
                return CheckQmpRes.modified;

            });
        }
        public enum CheckQmpRes
        {
            noNeed,
            updated,
            modified
        }
        public static async Task CheckAllQmpAsync()
        {
            C.WriteLn("开始检查群名片");
            List<GroupMember> l = Robot.Group.GetMembers(G.major);
            int n = 0, u = 0, m = 0;
            foreach (GroupMember member in l)
            {
                CheckQmpRes r = await CheckQmpAsync(new User(member.QQ));
                switch (r)
                {
                    case CheckQmpRes.modified:
                        m++;
                        Thread.Sleep(5000);
                        break;
                    case CheckQmpRes.noNeed:
                        n++;
                        break;
                    case CheckQmpRes.updated:
                        u++;
                        break;
                }
            }
            C.WriteLn($"noneed{n}, updated{u}, modified{m}");
        }
    }
}
