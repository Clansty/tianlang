using System;
using System.Collections.Generic;

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
        /// 日志记录器
        /// </summary>
        public ICronLogger logger = new DefaultCronLogger();

        public Dictionary<long, ICornEventHandler> handlers;
    }
}