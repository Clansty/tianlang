namespace Clansty.tianlang
{
    static class Repeater
    {
        private static string last;
        private static int i;

        private const string dd = "打断";
        private const string dg = "出现了打断怪";

        public static void Enter(string msg)
        {
            if (last == msg)
            {
                i++;
                if (i == 4)
                {
                    if (msg == dd)
                    {
                        S.Major(dg);
                        last = dg;
                    }
                    else
                    {
                        S.Major(dd);
                        last = dd;
                    }
                    i = 1;
                }
            }
            else
            {
                last = msg;
                i = 1;
            }
        }
    }
}
