using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clansty.tianlang
{
    /// <summary>
    /// 请在这里填写应用的信息
    /// </summary>
    class AppInfo
    {
        /// <summary>
        /// 固定 不要改
        /// </summary>
        public readonly int apiVer = 6;
        /// <summary>
        /// 应用ID  1.尽量唯一  2.编译出来需要以 .lq.dll 结尾 方便框架识别
        /// </summary>
        public readonly string appId = "com.lwqwq.nthsbot";
        /// <summary>
        /// 应用名称
        /// </summary>
        public readonly string name = "甜狼";
        /// <summary>
        /// 应用版本
        /// </summary>
        public readonly string ver = C.Version;
        /// <summary>
        /// 开发者ID ‘预留  随便填
        /// </summary>
        public readonly string authkey = "123456";
        /// <summary>
        /// 作者名字
        /// </summary>
        public readonly string author = "凌莞";
        /// <summary>
        /// 应用简介
        /// </summary>
        public readonly string description = "样例应用简介";
        /// <summary>
        /// 应用详情显示的 URL
        /// </summary>
        public readonly string url = "https://github.com/Clansty/Clansty.tianlang";
        /// <summary>
        /// 0 表示不需要取 cookies，需要请改为 1
        /// </summary>
        public readonly int ck = 0;
        /// <summary>
        /// 机器人自己的 QQ，以后将不需要这个
        /// ☆请勿催更，我没有义务做这个 SDK
        /// </summary>
# if DEBUG
        internal static long self = 2981882373;
#else
        internal static long self = 1980853671;
#endif
    }
}
