using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Clansty.tianlang
{
    static class Menu
    {
        public const string menu =
            "{\"name\":\"导出名单\",\"function\":\"ExportNames\"}," +
            "{\"name\":\"群名片检查\",\"function\":\"CheckQmps\"}," +
            "{\"name\":\"群名片检查假\",\"function\":\"TmpQmp\"}," +
            "{\"name\":\"赞\",\"function\":\"Likes\"}," +
            "{\"name\":\"测试续火\",\"function\":\"Ftest\"}," +
            "{\"name\":\"测试新版点赞\",\"function\":\"Ftest2\"}," +
            "{\"name\":\"名单入库\",\"function\":\"ExportNamesToDb\"}," +
            "{\"name\":\"推送工具\",\"function\":\"Pushtool\"}"
         ;

        private const string major = "646751705";
        private const string test = "828390342";
        private const string si = "690696283";

        [DllExport(CallingConvention.StdCall)]
        private static int ExportNames()
        {
            Console.WriteLine("开始导出名单");
            List<GroupMember> gms = Robot.Group.GetMembers(major);
            string res = "QQ\trole\tNamecard\tenrollment\tjunior\tbranch\n";
            foreach (GroupMember m in gms)
            {
                string card = m.Card == "" ? m.Nick : m.Card;
                string r = $"{m.QQ}\t{m.Role}\t{card}\t{UserInfo.ParseEnrollment(card)}\t{UserInfo.ParseJunior(card)}\t{UserInfo.ParseBranch(card)}\n";
                res += r;
                Console.WriteLine(r);
            }
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\cards.txt", res);
            Console.WriteLine("成功");
            return 0;
        }
        [DllExport(CallingConvention.StdCall)]
        private static int ExportNamesToDb()
        {
            new Thread(ExportNamesEx).Start();
            return 0;
        }
        private static void ExportNamesEx()
        {
            Console.WriteLine("开始存储名单");
            List<GroupMember> gms = Robot.Group.GetMembers(major);
            string res = "QQ\trole\tNamecard\tenrollment\tjunior\tbranch\tnick\tpropernc\n";
            foreach (GroupMember m in gms)
            {
                string card = m.Card == "" ? m.Nick : m.Card;
                User u = new User(m.QQ);
                u.Branch = UserInfo.ParseBranch(card);
                u.Enrollment = UserInfo.ParseEnrollment(card);
                u.Junior = UserInfo.ParseJunior(card);
                u.Nick = UserInfo.ParseNick(card);
                string r = $"{m.QQ}\t{m.Role}\t{card}\t{UserInfo.ParseEnrollment(card)}\t{UserInfo.ParseJunior(card)}\t{UserInfo.ParseBranch(card)}\t{u.Nick}\t{u.ProperNamecard}\n";
                res += r;
                Console.WriteLine(r);
            }
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\cardsex.txt", res);
            Console.WriteLine("成功");


        }

        [DllExport(CallingConvention.StdCall)]
        private static int CheckQmps()
        {
            new Thread(UserInfo.CheckAllQmp).Start();
            return 0;
        }
        [DllExport(CallingConvention.StdCall)]
        private static int Likes()
        {
            new Thread(() =>
                {
                    Fired.EnLike(839827911);
                }).Start();
            return 0;
        }
        [DllExport(CallingConvention.StdCall)]
        private static int Ftest()
        {
            new Thread(() =>
            {
                Fired.Cron(true);
            }).Start();
            return 0;
        }
        [DllExport(CallingConvention.StdCall)]
        private static int Ftest2()
        {
            new Thread(() =>
            {
                Fired.EnLikeNew();
            }).Start();
            return 0;
        }
        [DllExport(CallingConvention.StdCall)]
        private static int TmpQmp()
        {
            new Thread(() =>
            {
                C.WriteLn("开始检查群名片");
                List<GroupMember> l = Robot.Group.GetMembers(G.major);
                int n = 0, un = 0, m = 0;
                foreach (GroupMember member in l)
                {
                    User u = new User(member.QQ);
                    string card = member.Card;
                    u.Nick = UserInfo.ParseNick(card);
                    if (u.Enrollment != 10086 && UserInfo.ParseEnrollment(card) > 1970)
                        u.Enrollment = UserInfo.ParseEnrollment(card);
                    u.Junior = UserInfo.ParseJunior(card);
                    u.Branch = UserInfo.ParseBranch(card);

                    if (u.ProperNamecard != card)
                        C.Write("1");
                }
                C.WriteLn($"noneed{n}, updated{un}, modified{m}");

            }).Start();
            return 0;
        }
        [DllExport(CallingConvention.StdCall)]
        private static int Pushtool()
        {
            new Push().Show();
            return 0;
        }

    }
}
