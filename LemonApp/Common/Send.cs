namespace Clansty.tianlang
{
    static class S
    {
        public static void Group(long group, string msg)
        {
            Robot.Send.Group(group, msg);
        }

        public static void Private(long qq, string msg)
        {
            Robot.Send.Friend(qq, msg);
        }

        public static void Major(string msg) => Group(G.major, msg);
        public static void Si(string msg) => Group(G.si, msg);
        public static void Test(string msg) => Group(G.test, msg);
    }
}