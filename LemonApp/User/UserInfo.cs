using System;using System.Collections.Generic;
using System.Threading;using System.Threading.Tasks;namespace Clansty.tianlang{    static class UserInfo //用户信息辅助类
    {        internal static readonly Dictionary<string, int> GradeToEnrollment = new Dictionary<string, int>
        {
            ["高一"] = 2020,
            ["高1"] = 2020,
            ["高二"] = 2019,
            ["高2"] = 2019,
            ["高三"] = 2018,
            ["高3"] = 2018,
        };        internal static int ParseEnrollment(string s)        {            if (s == null || s == "")                return 0;            if (s.StartsWith("<"))                s = s.GetRight(">");            s = s.Trim();            var m = 0;            s = s.Replace("丨", " | ");            s = s.Replace("｜", " | ");            if (s.IndexOf('|') > -1)                m = ParseEnrollment(s.GetLeft("|"));            if (m > 0)                return m;            foreach (var kvp in GradeToEnrollment)
            {
                if (s.Contains(kvp.Key))
                    return kvp.Value;
            }            if (s.Contains("届"))                s = s.GetLeft("届");            s = s.Trim();            var i = int.TryParse(s, out m);            if (i && m > 100)                return m - 3;            if (i && m > 0)                return m + 1997;            return 0;        }        internal static bool ParseJunior(string s)        {            if (string.IsNullOrEmpty(s))                return false;            if (s.StartsWith("<"))                s = s.GetRight(">");            s = s.Trim();            s = s.Replace("丨", " | ");            s = s.Replace("｜", " | ");            if (s.IndexOf('|') > -1)                return ParseJunior(s.GetLeft("|"));            if (s.IndexOf("初中") > -1 || s.IndexOf("初三") > -1 || s.IndexOf("初3") > -1)                return true;            return false;        }        internal static string ParseNick(string s)        {            if (string.IsNullOrEmpty(s))                return "";            if (s.StartsWith("<"))                s = s.GetRight(">");            s = s.Trim();            s = s.Replace("丨", " | ");            s = s.Replace("｜", " | ");            if (s.IndexOf("|") > -1)                s = s.GetRight("|").Trim();            else if (s.IndexOf("高一") > -1)                s = s.GetRight("高一").Trim();            else if (s.IndexOf("高二") > -1)                s = s.GetRight("高二").Trim();            else if (s.IndexOf("高三") > -1)                s = s.GetRight("高三").Trim();            else if (s.IndexOf("高二/2019届初中") > -1)                s = s.GetRight("高二/2019届初中").Trim();            else if (s.IndexOf("高一/2020届初中") > -1)                s = s.GetRight("高一/2020届初中").Trim();            else if (s.IndexOf("初中") > -1)                s = s.GetRight("初中").Trim();            else if (s.IndexOf("届") > -1)                s = s.GetRight("届").Trim();            else if (s.IndexOf("金阊") > -1)                s = s.GetRight("金阊").Trim();            return s;        }        internal static Task<CheckQmpRes> CheckQmpAsync(User u, string card = null)        {//现在取成员列表不自带 card 了所以需要重新取
            return Task.Run(() =>
            {
                if (string.IsNullOrEmpty(card))
                {
                    card = u.Namecard;
                }
                if (card.StartsWith("<") && card.IndexOf(">") > -1)
                    card = card.GetRight(">");

                if (card == u.ProperNamecard)
                {
                    return CheckQmpRes.noNeed;
                }

                u.Nick = ParseNick(card);
                // 年级锁着
                if (u.Enrollment == 0 || u.Enrollment == -1)
                    u.Enrollment = ParseEnrollment(card);
                u.Junior = ParseJunior(card);

                if (card == u.ProperNamecard)
                {
                    C.WriteLn($"{u.Uin} updated", ConsoleColor.DarkGreen);
                    return CheckQmpRes.updated;
                }

                u.Namecard = u.ProperNamecard;

                C.WriteLn($"{u.Uin} modified: {card} -> {u.ProperNamecard}", ConsoleColor.Yellow);
                return CheckQmpRes.modified;
            });        }        internal enum CheckQmpRes        {            noNeed,            updated,            modified        }        internal static async Task CheckAllQmpAsync()        {            C.WriteLn("开始检查群名片");            var l = Robot.GetGroupMembers(G.major);
            //这里要更新memberlist信息
            MemberList.UpdateMajor(l);            int n = 0, u = 0, m = 0;            foreach (var member in l.member)            {                var r = await CheckQmpAsync(new User(member.uin), member.card);                switch (r)                {                    case CheckQmpRes.modified:                        m++;                        Thread.Sleep(5000);                        break;                    case CheckQmpRes.noNeed:                        n++;                        break;                    case CheckQmpRes.updated:                        u++;                        break;                }            }            C.WriteLn($"major: noneed{n}, updated{u}, modified{m}");        }        internal static bool NameVerify(string s)        {            foreach (var i in s.ToCharArray())            {                if (i < 0x4100 || i > 0x9fbb)                {                    return false;                }            }            if (s.Length > 4 || s.Length < 2)            {                return false;            }            return true;        }    }}