using System;
using System.Collections.Generic;
using System.Text;

namespace CornSDK.ApiTypes
{
    public class GroupMember
    {
        public long UIN { get; set; }
        public int Age { get; set; }
        public Sex Sex { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Card { get; set; }
        public string Remark { get; set; }
        public string SpecTitle { get; set; }
        public string Phone { get; set; }
        public long SpecTitleExpired { get; set; }
        public long MuteTime { get; set; }
        public long AddGroupTime { get; set; }
        public long LastMsgTime { get; set; }
        public int GroupLevel { get; set; }
    }
}
