using System;

namespace Clansty.tianlang
{
    internal static class Stats
    {
        internal static void New(GroupMsgArgs e)
        {
            User u = new User(e.FromQQ);
            string date = DateTime.Now.ToShortDateString();
            string grade;
            if (u.Enrollment < 1970)
                grade = "unknown";
            else if (u.Enrollment == 10001)
                grade = "america";
            else if (u.Enrollment < 2016)
                grade = "graduated";
            else if (!u.Junior)
                grade = u.Enrollment.ToString();
            else
                grade = "c" + u.Enrollment.ToString();
            if (grade == "c2019")
                grade = "2019";
            int.TryParse(Rds.HGet("stats" + date, grade), out int m);
            m++;
            Rds.HSet("stats" + date, grade, m.ToString());
        }
    }
}