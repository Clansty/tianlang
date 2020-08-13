using System;

namespace Clansty.tianlang
{
    internal class GroupCardChangedArgs : EventArgs
    {
        internal long Group { get; }
        internal long QQ { get; }
        internal string NewCard { get; }

        internal GroupCardChangedArgs(long c, long d, string f)
        {
            Group = c;
            QQ = d;
            NewCard = f;
        }
    }
}