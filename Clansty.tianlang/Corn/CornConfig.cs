namespace CornSDK
{
    /// <summary>
    /// 客户端配置
    /// </summary>
    public class CornConfig
    {
        /// <summary>
        /// HTTP API 运行的 IP
        /// </summary>
        public string ip = "127.0.0.1";
        /// <summary>
        /// HTTP API 设置的监听端口
        /// </summary>
        public int port = 10429;
        /// <summary>
        /// 事件上报 HTTP 服务器监听 IP
        /// </summary>
        public string listenIp = "127.0.0.1";
        /// <summary>
        /// 事件上报 HTTP 服务器监听端口
        /// </summary>
        public int listenPort = 8080;
        /// <summary>
        /// 机器人 QQ 号
        /// </summary>
        public long selfQQ = 0;
        /// <summary>
        /// 日志记录器
        /// </summary>
        public ICronLogger logger = new DefaultCronLogger();
        /// <summary>
        /// 指定处理好友消息的处理器
        /// </summary>
        public IFriendMsgHandler friendMsgHandler = new DoNothingHandler();
        /// <summary>
        /// 
        /// </summary>
        public ITempMsgHandler tempMsgHandler = new DoNothingHandler();
        /// <summary>
        /// 
        /// </summary>
        public IGroupMsgHandler groupMsgHandler = new DoNothingHandler();
        public IFriendRequestHandler friendRequestHandler = new DoNothingHandler();
        public IGroupInviteRequestHandler groupInviteRequestHandler = new DoNothingHandler();
        public IGroupJoinRequestHandler groupJoinRequestHandler = new DoNothingHandler();
        public IGroupAddMemberHandler groupAddMemberHandler = new DoNothingHandler();
        public IGroupLeftMemberHandler groupLeftMemberHandler = new DoNothingHandler();
    }
}