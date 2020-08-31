﻿using System;
using System.Threading;
    {
        {
            ["高一"] = 2020,
            ["高1"] = 2020,
            ["高二"] = 2019,
            ["高2"] = 2019,
            ["高三"] = 2018,
            ["高3"] = 2018,
        };
            {
                if (s.Contains(kvp.Key))
                    return kvp.Value;
            }
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
            });
            C.WriteLn("开始检查群名片");
            var l = await C.Robot.GetGroupMembers(G.major);
            //这里要更新memberlist信息
            MemberList.UpdateMajor(l);
            int n = 0, u = 0, m = 0;
            foreach (var member in l)
            {
                var r = await CheckQmpAsync(new User(member.UIN), member.Card);
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

            C.WriteLn($"major: noneed{n}, updated{u}, modified{m}");