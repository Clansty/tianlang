namespace Clansty.tianlang
{
    static class S
    {
        internal static void Group(long group, string msg)
        {
            C.QQ.SendGroupMsg(group, msg);
        }

        internal static void Private(long qq, string msg)
        {
            C.QQ.SendTempMsg(G.major, qq, msg);
        }

        internal static void Major(string msg) => Group(G.major, msg);
        internal static void Si(string msg) => Group(G.si, msg);
        internal static void Test(string msg) => Group(G.test, msg);
        internal static void IDE(string msg) => Group(G.iDE, msg);

        /// <summary>
        /// 少用这个
        /// </summary>
        internal static class TG
        {
            internal static void Text(long id, string msg)
            {
                C.TG.SendTextMessageAsync(id, msg);
            }
            internal static void Major(string msg) => Text(G.TG.major, msg);
            internal static void Si(string msg) => Text(G.TG.si, msg);
            internal static void Test(string msg) => Text(G.TG.test, msg);
            internal static void IDE(string msg) => Text(G.TG.iDE, msg);
        }
    }
}