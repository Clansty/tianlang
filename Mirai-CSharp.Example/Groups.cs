using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirai_CSharp.Tianlang
{
    public static class G
    {
        public static long Major
        {
            get
            {
                if (C.debug)
                    return 670526569;
                else
                    return 646751705;
            }
        }
        public static long Console
        {
            get
            {
                if (C.debug)
                    return 960701873;
                else
                    return 960696283;
            }
        }
        public const long test = 828390342;
        public const long iDE = 342975953;
    }
}
