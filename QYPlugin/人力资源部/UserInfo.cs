﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
                Task.Run(SendEng);
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
            else if (s.IndexOf("金阊") > -1)
                s = s.GetRight("金阊").Trim();
            return s;
        }
        public static async Task<CheckQmpRes> CheckQmpAsync(User u, string card = null)
        {
            return await Task.Run(() =>
            {
                if (card is null)
                    card = u.Namecard;
                if (card.StartsWith("<") && card.IndexOf(">") > -1)
                    card = card.GetRight(">");

                if (card == u.ProperNamecard)
                {
                    return CheckQmpRes.noNeed;
                }

                u.Nick = ParseNick(card);
                // TODO: 只有未定义名片的人能修改年级
                if (u.Enrollment == 0 || u.Enrollment == -1)
                    u.Enrollment = ParseEnrollment(card);
                u.Junior = ParseJunior(card);

                if (card == u.ProperNamecard)
                {
                    C.WriteLn($"{u.Uin} updated", System.ConsoleColor.DarkGreen);
                    return CheckQmpRes.updated;
                }

                u.Namecard = u.ProperNamecard;

                C.WriteLn($"{u.Uin} modified", System.ConsoleColor.Yellow);
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
                CheckQmpRes r = await CheckQmpAsync(new User(member.QQ), member.Card);
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
        public static INamedUser FindUser(string name)
        {
            var chk = RealName.Check(name);
            if (chk.Status == RealNameStatus.notFound)
                throw new System.Exception("找不到此人");
            if (chk.OccupiedQQ is null)
                return new UnRegUser(name);
            return new User(chk.OccupiedQQ);
        }

        private static void SendEng()
        {
            var d = DateTime.Now;
            var m = d.Month - 1;
            var month = "err";
            switch (m)
            {
                case 0:
                    month = "Jan";
                    break;
                case 1:
                    month = "Feb";
                    break;
                case 2:
                    month = "Mar";
                    break;
                case 3:
                    month = "Apr";
                    break;
                case 4:
                    month = "May";
                    break;
                case 5:
                    month = "Jun";
                    break;
                case 6:
                    month = "Jul";
                    break;
                case 7:
                    month = "Aug";
                    break;
                case 8:
                    month = "Sept";
                    break;
                case 9:
                    month = "Oct";
                    break;
                case 10:
                    month = "Nov";
                    break;
                case 11:
                    month = "Dec";
                    break;
            }

            var dt = $"{d.Month}.{d.Day}";
            var loc = @"S:\群组资料库\英语\" + month + @"\" + dt;
            if (Directory.Exists(loc))
            {
                if (File.Exists(@"S:\群组资料库\英语\uploadRecord\" + dt))
                {
                    return;
                }

                File.Create(@"S:\群组资料库\英语\uploadRecord\" + dt).Close();

                var z = new Process();
                z.StartInfo.FileName = @"C:\Program Files\7-Zip\7z.exe";
                z.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                z.StartInfo.Arguments = "a " + @"C:\Users\Administrator\Desktop\engUpTmp\" + dt + ".7z " + loc;
                z.Start();
                z.WaitForExit();
                z.Close();
                z.Dispose();

                Robot.Group.UploadFile("117076933", @"C:\Users\Administrator\Desktop\engUpTmp\" + dt + ".7z");
                C.WriteLn("咱发英语了", ConsoleColor.Cyan);
            }
        }
    }
}
