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

        internal static void Private(User u, string msg)
        {
            C.QQ.SendTempMsg(G.major, u.Uin, msg);
        }

        internal static void Major(string msg, bool syncToTg = true)
        {
            Group(G.major, msg);
            if (syncToTg)
                TG.Major(msg);
        }

        internal static void Si(string msg, bool syncToTg = true)
        {
            Group(G.si, msg);
            if (syncToTg)
                TG.Si(msg);
        }

        internal static void IDE(string msg, bool syncToTg = true)
        {
            Group(G.iDE, msg);
            if (syncToTg)
                TG.IDE(msg);
        }

        /// <summary>
        /// 少用这个
        /// </summary>
        internal static class TG
        {
            internal static void Text(long id, string msg, bool disableWebPagePreview = false)
            {
                C.TG.SendTextMessageAsync(id, msg, disableWebPagePreview: disableWebPagePreview);
            }

            internal static void Major(string msg, bool disableWebPagePreview = false) =>
                Text(G.TG.major, msg, disableWebPagePreview);

            internal static void Si(string msg, bool disableWebPagePreview = false) =>
                Text(G.TG.si, msg, disableWebPagePreview);

            internal static void IDE(string msg) => Text(G.TG.iDE, msg);
        }
    }
}