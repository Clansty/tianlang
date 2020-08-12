using System;

namespace Clansty.tianlang
{
    internal class GroupCardChangedArgs : EventArgs
    {
        public long Group { get; }
        public long QQ { get; }
        public string NewCard { get; }

        public GroupCardChangedArgs(long c, long d, string f)
        {
            Group = c;
            QQ = d;
            NewCard = f;
        }
    }
}