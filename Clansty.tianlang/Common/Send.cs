namespace Clansty.tianlang
{
    static class S
    {
        internal static void Group(long group, string msg)
        {
            C.Robot.SendGroupMsg(group, msg);
        }

        internal static void Private(long qq, string msg)
        {
            C.Robot.SendTempMsg(G.major, qq, msg);
        }

        internal static void Major(string msg) => Group(G.major, msg);
        internal static void Si(string msg) => Group(G.si, msg);
        internal static void Test(string msg) => Group(G.test, msg);
    }
}