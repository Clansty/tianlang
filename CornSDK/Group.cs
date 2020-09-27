namespace CornSDK
{
    public class Group
    {
        public long GroupID { get; set; }
        public long GIN { get; set; }
        public long cFlag { get; set; }
        public long GroupInfoSeq { get; set; }
        public long dwGroupFlagExt { get; set; }
        public long dwGroupRankSeq { get; set; }
        public long dwCertificationType { get; set; }
        public long dwShutupTimestamp { get; set; }
        public long dwMyShutupTimestamp { get; set; }
        public long dwCmdUinUinFlag { get; set; }
        public long dwAdditionalFlag { get; set; }
        public long dwGroupTypeFlag { get; set; }
        public long dwGroupSecType { get; set; }
        public long dwGroupSecTypeInfo { get; set; }
        public long dwGroupClassExt { get; set; }
        public long dwAppPrivilegeFlag { get; set; }
        public long dwSubscriptionUin { get; set; }
        public long dwMemberNum { get; set; }
        public long dwMemberNumSeq { get; set; }
        public long dwMemberCardSeq { get; set; }
        public long dwGroupFlagExt3 { get; set; }
        public long dwGroupOwnerUin { get; set; }
        public long cIsConfGroup { get; set; }
        public long cIsModifyConfGroupFace { get; set; }
        public long cIsModifyConfGroupName { get; set; }
        public long dwCmduinJolongime { get; set; }
        public string strGroupName { get; set; }
        public string strGroupMemo { get; set; }
    }
}
