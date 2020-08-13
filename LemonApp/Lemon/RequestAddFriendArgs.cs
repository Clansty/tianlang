using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Clansty.tianlang
{
    class RequestAddFriendArgs : EventArgs
    {
        internal RequestAddFriendArgs(long a, string b)
        {
            FromQQ = a.ToString();
            Msg = b;
        }

        /// <summary>
        /// 申请加你的人
        /// </summary>
        internal string FromQQ { get; }
        /// <summary>
        /// 附加信息
        /// </summary>
        internal string Msg { get; }
        /// <summary>
        /// 同意该请求
        /// </summary>
        /// <param name="remark">你给别人的备注</param>
        internal void Accept(string remark = "") => Api_SetFriendAddRequest(AppInfo.self, long.Parse(FromQQ), 1, remark);
        /// <summary>
        /// 拒绝该请求
        /// </summary>
        /// <param name="reason">理由</param>
        internal void Reject(string reason = "") => Api_SetFriendAddRequest(AppInfo.self, long.Parse(FromQQ), 2, reason);
        [DllImport("LqrHelper.dll")]
        private extern static void Api_SetFriendAddRequest(long a, long b, int c, string d);

    }
}
