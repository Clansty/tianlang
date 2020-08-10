using System;

namespace Clansty.tianlang
{
    internal class GroupCardChangedArgs : EventArgs
    {
        public string Group { get; }
        public string QQ { get; }
        public string NewCard { get; }

        public GroupCardChangedArgs(long c, long d, string f)
        {
            Group = c.ToString();
            QQ = d.ToString();
            NewCard = f;
        }
    }
}