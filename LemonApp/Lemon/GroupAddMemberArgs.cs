using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clansty.tianlang
{
    class GroupAddMemberArgs : EventArgs
    {
        internal long Group { get; }
        internal long FromQQ { get; }
        internal long BeingOperateQQ { get; }
        internal GroupAddMemberArgs(long a, long b, long c)
        {
            Group = a;
            FromQQ = b;
            BeingOperateQQ = c;
        }
    }
}
