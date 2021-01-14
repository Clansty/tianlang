using Mirai_CSharp.Extensions;
using Mirai_CSharp.Models;

namespace Clansty.tianlang
{
    static class S
    {
        internal static void Group(long group, string msg)
        {
            Group(group, new PlainMessage(msg));
        }

        internal static void Group(long group, params IMessageBase[] msg)
        {
            C.QQ.NthsBot.SendGroupMessageAsync(group, msg);
        }

        internal static void Private(long qq, string msg)
        {
            Private(qq, new PlainMessage(msg));
        }

        internal static void Private(long qq, params IMessageBase[] msg)
        {
            C.QQ.NthsBot.SendTempMessageAsync(qq, G.major, msg);
        }

        internal static void Private(User u, string msg)
        {
            Private(u, new PlainMessage(msg));
        }

        internal static void Private(User u, params IMessageBase[] msg)
        {
            Private(u.Uin, msg);
        }

        internal static void Major(string msg, bool syncToTg = true)
        {
            Major(new IMessageBase[] {new PlainMessage(msg)}, syncToTg);
        }

        internal static void Major(IMessageBase[] msg, bool syncToTg = true)
        {
            Group(G.major, msg);
            if (syncToTg)
                TG.Major(msg.GetPlain());
        }

        internal static void Si(string msg, bool syncToTg = true)
        {
            Si(new IMessageBase[] {new PlainMessage(msg)}, syncToTg);
        }

        internal static void Si(IMessageBase[] msg, bool syncToTg = true)
        {
            Group(G.si, msg);
            if (syncToTg)
                TG.Si(msg.GetPlain());
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
        }
    }
}