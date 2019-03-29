
namespace tianlang
{
    public class Repeater
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
                        if (C.isTest)
                            S.Test(dg);
                        else
                            S.Major(dg);
                        last = dg;
                    }
                    else
                    {
                        if (C.isTest)
                            S.Test(dd);
                        else
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
