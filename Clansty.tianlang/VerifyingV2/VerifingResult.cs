using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clansty.tianlang
{
    /// <summary>
    /// 目前只有 User.VerifyMsg 会返回这个东西
    /// </summary>
    enum VerifingResult
    {
        /// <summary>
        /// 一定是年级支持的
        /// </summary>
        notFound,
        unsupported,
        /// <summary>
        /// 一定是年级支持的。没名单的新高一和有名单的重名是 unsupported
        /// </summary>
        occupied,
        succeed,
        nameEmpty,
        error
    }
}
