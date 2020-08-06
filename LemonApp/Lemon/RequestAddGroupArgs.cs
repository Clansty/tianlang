using System;
using System.Runtime.InteropServices;

namespace LemonApp
{
    class RequestAddGroupArgs : EventArgs
    {
        string Seq { get; }
        public string Group { get; }
        public string QQ { get; }
        int Flag { get; }
        public string Msg { get; }
        public RequestAddGroupArgs(string a, long b, long c, int d, string e)
        {
            Seq = a;
            Group = b.ToString();
            QQ = c.ToString();
            Flag = d;
            Msg = e;
        }
        public void Accept()
        {
            Api_GroupHandleEvent(AppInfo.self, Seq, long.Parse(Group), long.Parse(QQ), 1, "", Flag);
        }
        public void Reject(string reason = "")
        {
            Api_GroupHandleEvent(AppInfo.self, Seq, long.Parse(Group), long.Parse(QQ), 2, reason, Flag);
        }
        [DllImport("LqrHelper.dll")]
        extern static void Api_GroupHandleEvent(long a, string b, long c, long d, int e, string f, int g);
    }
}
