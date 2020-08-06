using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clansty.tianlang
{
    class GroupAddMemberArgs : EventArgs
    {
        public string Group { get; }
        public string FromQQ { get; }
        public string BeingOperateQQ { get; }
        public GroupAddMemberArgs(string a, string b, string c)
        {
            Group = a;
            FromQQ = b;
            BeingOperateQQ = c;
        }
    }
}
