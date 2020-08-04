using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LemonApp
{
    /// <summary>
    /// 请在这里填写应用的信息
    /// </summary>
    static class AppInfo
    {
        /// <summary>
        /// 固定 不要改
        /// </summary>
        public const int apiVer = 6;
        /// <summary>
        /// 应用ID  1.尽量唯一  2.编译出来需要以 .lq.dll 结尾 方便框架识别
        /// </summary>
        public const string appId = "com.lwqwq.lemon.sdk";
        /// <summary>
        /// 应用名称
        /// </summary>
        public const string name = "柠檬乐园样例应用";
        /// <summary>
        /// 应用版本
        /// </summary>
        public const string ver = "1.0";
        /// <summary>
        /// 开发者ID ‘预留  随便填
        /// </summary>
        public const string authkey = "123456";
        /// <summary>
        /// 作者名字
        /// </summary>
        public const string author = "凌莞";
        /// <summary>
        /// 应用简介
        /// </summary>
        public const string description = "样例应用简介";
        /// <summary>
        /// 应用详情显示的 URL
        /// </summary>
        public const string url = "https://github.com/Clansty/LemonApp";
        /// <summary>
        /// 0 表示不需要取 cookies，需要请改为 1
        /// </summary>
        public const int ck = 0;
    }
}
