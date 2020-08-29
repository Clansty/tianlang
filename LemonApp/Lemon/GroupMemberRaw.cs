using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clansty.tianlang
{
    //如果好用，请收藏地址，帮忙分享。
    public class MemberItem
    {
        /// <summary>
        /// 
        /// </summary>
        public long uin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int age { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int sex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long jointime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long speaktime { get; set; }
        /// <summary>
        /// 甜狼辅助账号
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string card { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string specialtitle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long specialtitletime { get; set; }
    }

    public class AdminItem
    {
        /// <summary>
        /// 
        /// </summary>
        public long uin { get; set; }
    }

    public class GroupMemberRaw
    {
        /// <summary>
        /// 
        /// </summary>
        public List<MemberItem> member { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<AdminItem> admin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long owner { get; set; }
    }
}
