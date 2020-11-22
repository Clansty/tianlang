using CornSDK.ApiTypes;
using Flurl.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Clansty.tianlang;
using Newtonsoft.Json;

namespace CornSDK
{
    /// <summary>
    /// 小栗子 HTTP API 客户端
    /// </summary>
    public class Corn
    {
        internal readonly CornConfig config;
        private HttpListener listener;

        /// <summary>
        /// 初始化一个 API 客户端
        /// </summary>
        /// <param name="cfg"></param>
        public Corn(CornConfig cfg)
        {
            config = cfg;
            listener = new HttpListener();
            listener.Prefixes.Add($"http://{cfg.listenIp}:{cfg.listenPort}/");
            listener.Start();
            listener.BeginGetContext(new AsyncCallback(GetContextCallBack), listener);
            Log("客户端初始化完成", CornLogLevel.Succeed);
        }

        void Log(string text, CornLogLevel level = CornLogLevel.Info) => config.logger.Log(text, level);

        #region HTTP 服务器辅助方法

        void GetContextCallBack(IAsyncResult ar)
        {
            try
            {
                listener = ar.AsyncState as HttpListener;
                HttpListenerContext context = listener.EndGetContext(ar);
                listener.BeginGetContext(new AsyncCallback(GetContextCallBack), listener);
                if (context.Request.HttpMethod.ToLower().Equals("post"))
                {
                    var SourceStream = context.Request.InputStream;
                    var currentChunk = ReadLineAsBytes(SourceStream);
                    var msg = Encoding.Default.GetString(currentChunk).Replace("�", "");
                    HandleMsg(msg);
                }

                Response(context.Response, "");
            }
            catch
            {
                // ignored
            }
        }

        byte[] ReadLineAsBytes(Stream SourceStream)
        {
            var resultStream = new MemoryStream();
            while (true)
            {
                int data = SourceStream.ReadByte();
                resultStream.WriteByte((byte) data);
                if (data <= 10)
                    break;
            }

            resultStream.Position = 0;
            byte[] dataBytes = new byte[resultStream.Length];
            resultStream.Read(dataBytes, 0, dataBytes.Length);
            return dataBytes;
        }

        void Response(HttpListenerResponse response, string responsestring, string contenttype = "application/json")
        {
            response.StatusCode = 200;
            response.ContentType = contenttype;
            response.ContentEncoding = Encoding.UTF8;
            byte[] buffer = Encoding.UTF8.GetBytes(responsestring);
            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }

        internal Task<string> Post(string whattodo, object body) => $"http://{config.ip}:{config.port}/{whattodo}"
            .PostUrlEncodedAsync(body).ReceiveString();

        internal Task<TAT> Post<TAT>(string whattodo, object body) => $"http://{config.ip}:{config.port}/{whattodo}"
            .PostUrlEncodedAsync(body).ReceiveJson<TAT>();

        #endregion

        /// <summary>
        /// 处理 HTTP 服务器接收到的信息
        /// </summary>
        /// <param name="msg"></param>
        void HandleMsg(string msg)
        {
#if DEBUG
            Log("收到消息: " + msg, CornLogLevel.IncommingMsg);
#endif
            try
            {
                var jobj = JObject.Parse(msg);
                if (jobj.Value<string>("Type") == "PrivateMsg")
                {
                    var pm = jobj.ToObject<PMRecv>();
                    if (!config.handlers.ContainsKey(pm.LogonQQ) || pm.LogonQQ == pm.FromQQ.UIN)
                        return;
                    switch (pm.Msg.Type)
                    {
                        //#消息类型_好友通常消息
                        case 166:
                        {
                            var args = new FriendMsgArgs()
                            {
                                Robot = this,
                                FromQQ = pm.FromQQ.UIN,
                                FromNick = pm.FromQQ.NickName,
                                Msg = pm.Msg.Text
                            };
                            config.handlers[pm.LogonQQ].OnFriendMsg(args);
                            break;
                        }
                        //#消息类型_临时会话
                        case 141:
                        {
                            var args = new TempMsgArgs()
                            {
                                Robot = this,
                                FromQQ = pm.FromQQ.UIN,
                                FromGroup = pm.FromGroup.GIN,
                                FromNick = pm.FromQQ.NickName,
                                Msg = pm.Msg.Text
                            };
                            config.handlers[pm.LogonQQ].OnTempMsg(args);
                            break;
                        }
                    }
                }
                else if (jobj.Value<string>("Type") == "GroupMsg")
                {
                    var gm = jobj.ToObject<GMRecv>();
                    if (!config.handlers.ContainsKey(gm.LogonQQ) || gm.LogonQQ == gm.FromQQ.UIN)
                        return;
                    if (gm.Msg.SubType != 134) return;
                    var args = new GroupMsgArgs()
                    {
                        Robot = this,
                        FromQQ = gm.FromQQ.UIN,
                        FromGroup = gm.FromGroup.GIN,
                        FromGroupName = gm.FromGroup.Name,
                        FromCard = gm.FromQQ.Card,
                        FromTitle = gm.FromQQ.SpecTitle,
                        Msg = gm.Msg.Text,
                        ReplyMsg = gm.Msg.Text_Reply,
                        Time = gm.TimeStamp.Send,
                        RecvQQ = gm.LogonQQ
                    };
                    config.handlers[gm.LogonQQ].OnGroupMsg(args);
                }
                else if (jobj.Value<string>("Type") == "EventMsg")
                {
                    var em = jobj.ToObject<EMRecv>();
                    if (!config.handlers.ContainsKey(em.LogonQQ))
                        return;
                    switch (em.Msg.Type)
                    {
                        //好友申请
                        case 105:
                        {
                            var args = new FriendRequestArgs()
                            {
                                Robot = this,
                                FromQQ = em.FromQQ.UIN,
                                FromNick = em.FromQQ.NickName,
                                Msg = em.Msg.Text,
                                Seq = em.Msg.Seq,
                                fromqq = em.LogonQQ
                            };
                            config.handlers[em.LogonQQ].OnFriendRequest(args);
                            break;
                        }
                        //机器人被邀请加群
                        case 1:
                        {
                            var args = new GroupRequestArgs()
                            {
                                Robot = this,
                                FromQQ = em.FromQQ.UIN,
                                OperateQQ = em.OperateQQ.UIN,
                                OperateNick = em.OperateQQ.NickName,
                                FromNick = em.FromQQ.NickName,
                                FromGroup = em.FromGroup.GIN,
                                FromGroupName = em.FromGroup.Name,
                                Type = 1,
                                Msg = em.Msg.Text,
                                Seq = em.Msg.Seq,
                                fromqq = em.LogonQQ
                            };
                            config.handlers[em.LogonQQ].OnGroupInviteRequest(args);
                            break;
                        }
                        //有人申请加群
                        case 3:
                        {
                            var args = new GroupRequestArgs()
                            {
                                Robot = this,
                                FromQQ = em.FromQQ.UIN,
                                OperateQQ = em.OperateQQ.UIN,
                                OperateNick = em.OperateQQ.NickName,
                                FromNick = em.FromQQ.NickName,
                                FromGroup = em.FromGroup.GIN,
                                FromGroupName = em.FromGroup.Name,
                                Type = 3,
                                Msg = em.Msg.Text,
                                Seq = em.Msg.Seq,
                                fromqq = em.LogonQQ
                            };
                            config.handlers[em.LogonQQ].OnGroupJoinRequest(args);
                            break;
                        }
                        //群成员增加
                        case 2:
                        {
                            var args = new GroupMemberChangedArgs()
                            {
                                Robot = this,
                                FromQQ = em.FromQQ.UIN,
                                FromNick = em.FromQQ.NickName,
                                FromGroup = em.FromGroup.GIN,
                                FromGroupName = em.FromGroup.Name,
                            };
                            config.handlers[em.LogonQQ].OnGroupAddMember(args);
                            break;
                        }
                        case 5:
                        //群成员减少
                        case 6:
                        {
                            var args = new GroupMemberChangedArgs()
                            {
                                Robot = this,
                                FromQQ = em.FromQQ.UIN,
                                FromNick = em.FromQQ.NickName,
                                FromGroup = em.FromGroup.GIN,
                                FromGroupName = em.FromGroup.Name,
                            };
                            config.handlers[em.LogonQQ].OnGroupLeftMember(args);
                            break;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Log("发生错误: " + err, CornLogLevel.Error);
            }
        }

        #region 操作

        /// <summary>
        /// 发送私聊消息
        /// </summary>
        /// <param name="toQQ">目标 QQ</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public Task SendPrivateMsg(long toQQ, string content, long fromqq = C.self) => Post("sendprivatemsg", new
        {
            fromqq,
            toqq = toQQ,
            text = content
        });

        /// <summary>
        /// 发送群消息
        /// </summary>
        /// <param name="toGroup"></param>
        /// <param name="content"></param>
        /// <returns>time</returns>
        public async Task<long> SendGroupMsg(long toGroup, string content, bool anonymous = false, long fromqq = C.self)
        {
            string ret = (await Post<dynamic>("sendgroupmsg", new
            {
                fromqq,
                togroup = toGroup,
                text = content,
                anonymous = anonymous.ToString().ToLower()
            })).ret;
            var jobj = JObject.Parse(ret);
            return jobj.Value<long>("time");
        }

        /// <summary>
        /// 发送群临时消息
        /// </summary>
        /// <param name="toGroup"></param>
        /// <param name="toQQ"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public Task SendTempMsg(long toGroup, long toQQ, string content, long fromqq = C.self) => Post(
            "sendgrouptempmsg", new
            {
                fromqq,
                togroup = toGroup,
                toqq = toQQ,
                text = content
            });

        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="toQQ"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public Task AddFriend(long toQQ, string content, string remark = "", long fromqq = C.self) => Post("addfriend",
            new
            {
                fromqq,
                toqq = toQQ,
                text = content,
                remark = remark
            });

        /// <summary>
        /// 添加群
        /// </summary>
        /// <param name="toGroup"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public Task AddGroup(long toGroup, string content, long fromqq = C.self) => Post("addgroup", new
        {
            fromqq,
            togroup = toGroup,
            text = content
        });

        /// <summary>
        /// 删好友
        /// </summary>
        /// <param name="toQQ"></param>
        /// <returns></returns>
        public Task DeleteFriend(long toQQ, long fromqq = C.self) => Post("deletefriend", new
        {
            fromqq,
            toqq = toQQ,
        });

        /// <summary>
        /// 屏蔽好友
        /// </summary>
        /// <param name="toQQ"></param>
        /// <returns></returns>
        public Task BlockFriend(long toQQ, long fromqq = C.self) => Post("setfriendignmsg", new
        {
            fromqq,
            toqq = toQQ,
            ignore = "true"
        });

        /// <summary>
        /// 解除屏蔽好友
        /// </summary>
        /// <param name="toQQ"></param>
        /// <returns></returns>
        public Task UnblockFriend(long toQQ, long fromqq = C.self) => Post("setfriendignmsg", new
        {
            fromqq,
            toqq = toQQ,
            ignore = "false"
        });

        /// <summary>
        /// 特别关心
        /// </summary>
        /// <param name="toQQ"></param>
        /// <returns></returns>
        public Task SetCare(long toQQ, long fromqq = C.self) => Post("setfriendcare", new
        {
            fromqq,
            toqq = toQQ,
            care = "true"
        });

        /// <summary>
        /// 解除特别关心
        /// </summary>
        /// <param name="toQQ"></param>
        /// <returns></returns>
        public Task UnsetCare(long toQQ, long fromqq = C.self) => Post("setfriendcare", new
        {
            fromqq,
            toqq = toQQ,
            care = "false"
        });

        /// <summary>
        /// 发送私聊 JSON 卡片
        /// </summary>
        /// <param name="toQQ"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public Task SendPrivateJsonMsg(long toQQ, string content, long fromqq = C.self) => Post("sendprivatejsonmsg",
            new
            {
                fromqq,
                toqq = toQQ,
                json = content
            });

        /// <summary>
        /// 发送群聊 JSON 卡片
        /// </summary>
        /// <param name="toGroup"></param>
        /// <param name="content"></param>
        /// <param name="anonymous"></param>
        /// <returns></returns>
        public Task SendGroupJsonMsg(long toGroup, string content, bool anonymous = false, long fromqq = C.self) =>
            Post("sendgroupjsonmsg",
                new
                {
                    fromqq,
                    togroup = toGroup,
                    json = content,
                    anonymous = anonymous.ToString().ToLower()
                });

        //TODO sendprivatepic sendgrouppic sendprivateaudio sendgroupaudio uploadfacepic uploadgroupfacepic
        /// <summary>
        /// 设置群名片
        /// </summary>
        /// <param name="toGroup"></param>
        /// <param name="toQQ"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public Task SetGroupCard(long toGroup, long toQQ, string content, long fromqq = C.self) => Post("setgroupcard",
            new
            {
                fromqq,
                togroup = toGroup,
                toqq = toQQ,
                card = content
            });

        /// <summary>
        /// 群踢人
        /// </summary>
        /// <param name="toGroup"></param>
        /// <param name="toQQ"></param>
        /// <param name="ignoreAddGRequest">拒绝再加群申请</param>
        /// <returns></returns>
        public Task GroupKickMember(long toGroup, long toQQ, bool ignoreAddGRequest = false, long fromqq = C.self) =>
            Post("kickgroupmember",
                new
                {
                    fromqq,
                    group = toGroup,
                    toqq = toQQ,
                    ignoreaddgrequest = ignoreAddGRequest.ToString().ToLower()
                });

        /// <summary>
        /// 取昵称
        /// </summary>
        /// <param name="toQQ"></param>
        /// <param name="fromcache"></param>
        /// <returns></returns>
        public async Task<string> GetNick(long toQQ, bool fromcache = true, long fromqq = C.self)
        {
            string ret = (await Post<dynamic>("getnickname", new
            {
                fromqq,
                toqq = toQQ,
                fromcache = fromcache.ToString().ToLower()
            })).ret;
            return Utf.Decode(ret);
        }

        /// <summary>
        /// 取群名
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task<string> GetGroupName(long group, long fromqq = C.self) => (await Post<dynamic>(
            "getgroupnamefromcache", new
            {
                group
            })).ret;

        /// <summary>
        /// 取群成员
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task<List<GroupMember>> GetGroupMembers(long group, long logonqq = C.self) =>
            (await Post<GroupMembers>(
                "getgroupmemberlist", new
                {
                    logonqq,
                    group
                })).List;

        /// <summary>
        /// 取群名片
        /// </summary>
        /// <param name="group"></param>
        /// <param name="toQQ"></param>
        /// <param name="fromqq"></param>
        /// <returns></returns>
        public async Task<string> GetGroupCard(long group, long toQQ, long fromqq = C.self) => (await Post<dynamic>(
            "getgroupcard", new
            {
                fromqq,
                toqq = toQQ,
                group
            })).ret;

        /// <summary>
        /// 取好友
        /// </summary>
        /// <returns></returns>
        public async Task<List<Friend>> GetFriends(long logonqq = C.self) => (await Post<Friends>("getfriendlist", new
        {
            logonqq,
        })).List;

        /// <summary>
        /// 取加入的群
        /// </summary>
        /// <returns></returns>
        public async Task<List<Group>> GetGroups(long logonqq = C.self) => (await Post<Groups>("getgrouplist", new
        {
            logonqq,
        })).List;

        /// <summary>
        /// 上传群图片，获得一个 hash 到时候当成图片发
        /// </summary>
        /// <returns></returns>
        public async Task<string> UploadGroupPic(long togroup, string url, long fromqq = C.self) =>
            (await Post<dynamic>("sendgrouppic", new
            {
                togroup,
                fromqq,
                fromtype = 2,
                url
            })).ret;

        /// <summary>
        /// 上传私聊图片，获得一个 hash 到时候当成图片发
        /// </summary>
        /// <returns></returns>
        public async Task<string> UploadPrivatePic(long toqq, string url, long fromqq = C.self) => (await Post<dynamic>(
            "sendprivatepic", new
            {
                toqq,
                fromqq,
                fromtype = 2,
                url
            })).ret;

        public async Task<string> GetPicUrl(string pic, long group = 0, long fromqq = C.self) => (await Post<dynamic>(
            "getphotourl", new
            {
                group,
                fromqq,
                photo = pic
            })).ret;

        public async Task<string> GetVideoUrl(long loginqq, long fromgroup, long fromqq, string param, string hash1)
        {
            string ret = (await Post<dynamic>(
                "getvideourl", new
                {
                    loginqq,
                    fromgroup,
                    fromqq,
                    param,
                    hash1,
                    fn = DateTime.Now.ToBinary() + ".mp4"
                })).ret;
            var rret = JsonConvert.DeserializeObject<dynamic>(ret);
            string url = rret.downloadurl;
            return url;
        }
        public async Task<string> GetFileUrl(string loginqq, string fromgroup, string id, string fn)
        {
            string ret = (await Post<dynamic>(
                "getvideourl", new
                {
                    loginqq,
                    fromgroup,
                    id,
                    fn
                })).ret;
            // var rret = JsonConvert.DeserializeObject<dynamic>(ret);
            // string url = rret.downloadurl;
            return ret;
        }

        //TODO rest

        #endregion
    }
}