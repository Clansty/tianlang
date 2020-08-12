using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clansty.tianlang
{
    class GroupAddMemberArgs : EventArgs
    {
        public long Group { get; }
        public long FromQQ { get; }
        public long BeingOperateQQ { get; }
        public GroupAddMemberArgs(long a, long b, long c)
        {
            Group = a;
            FromQQ = b;
            BeingOperateQQ = c;
        }
    }
}
