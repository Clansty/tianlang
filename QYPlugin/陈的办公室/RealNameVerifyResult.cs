using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clansty.tianlang.陈的办公室
{
    class RealNameVerifyResult
    {
        public RealNameVerifyResult(RealNameStatus rns, string occupiedQQ = null)
        {
            Status = rns;
            OccupiedQQ = occupiedQQ;
        }
        public RealNameStatus Status { get; }
        public string OccupiedQQ { get; }
    }
}
