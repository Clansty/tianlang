using System;
using System.Net;
using System.Runtime.InteropServices;

namespace tianlang
{
    public class Main
    {


        [DllExport(ExportName = nameof(IR_Event), CallingConvention = CallingConvention.StdCall)]
        public static int IR_Event(string RobotQQ, int MsgType, int MsgCType, string MsgFrom, string TigObjF, string TigObjC, string Msg,string MsgNum ,string MsgID,string RawMsg,string Json, int pText)
        {
            ///RobotQQ		机器人QQ				多Q版用于判定哪个QQ接收到该消息
            ///MsgType		消息类型				接收到消息类型，该类型可在常量表中查询具体定义，此处仅列举： - 1 未定义事件 1 好友信息 2, 群信息 3, 讨论组信息 4, 群临时会话 5, 讨论组临时会话 6, 财付通转账
            ///MsgCType		消息子类型			此参数在不同消息类型下，有不同的定义，暂定：接收财付通转账时 1为好友 2为群临时会话 3为讨论组临时会话    有人请求入群时，不良成员这里为1
            ///MsgFrom		消息来源				此消息的来源，如：群号、讨论组ID、临时会话QQ、好友QQ等
            ///TigObjF		触发对象_主动			主动发送这条消息的QQ，踢人时为踢人管理员QQ
            ///TigObjC		触发对象_被动			被动触发的QQ，如某人被踢出群，则此参数为被踢出人QQ
            ///MsgNum		消息序号				此参数暂定用于消息回复，消息撤回
            ///MsgID		消息ID				此参数暂定用于消息回复，消息撤回
            ///Msg			消息内容				常见为：对方发送的消息内容，但当IRC_消息类型为 某人申请入群，则为入群申请理由
            ///RawMsg		原始信息				特殊情况下会返回JSON结构（本身返回的就是JSON）
            ///Json			Json信息				为后期新参数预留，方便无限扩展
            ///pText		信息回传文本指针		此参数用于插件加载拒绝理由  用法：写到内存（“拒绝理由”，IRC_信息回传文本指针_Out）

            try
            {
                string QQ;
                switch (MsgType)
                {
                    case 1101: //登录成功
                        switch (RobotQQ)
                        {
                            case C.wp:
                                C.isTest = false;
                                IRQQApi.Api_OutPutLog("生产模式");
                                break;
                            case C.wt:
                                C.isTest = true;
                                IRQQApi.Api_OutPutLog("测试模式");
                                break;
                            default:
                                IRQQApi.Api_OutPutLog("登录的账号不对");
                                Environment.Exit(233);
                                break;
                        } //决定是否是测试模式
                        Db.Connect();
                        break;
                    case 12002: //插件禁用
                        Db.DisConnect();
                        break;
                    case 219:
                    case 212: //群成员增加
                        //C.UpdateMemberList();
                        QQ = TigObjC;
                        if (MsgFrom == (C.isTest ? G.test : G.major))
                            InfoSetup.Start(QQ);
                        break;
                    case 1: //好友
                    case 4: //群临时
                    case 5: //讨论组临时
                        //C.UpdateMemberList(true);
                        QQ = TigObjF;
                        Status status = C.GetStatus(QQ);
                        if (Msg == "cancel" || Msg == "取消" || Msg == "主菜单")
                        {
                            C.SetStatus(QQ, Status.no);
                            S.P(QQ, "你的状态已重置");
                        }
                        else if (Msg == "whoami")
                        {
                            User u = new User(QQ);
                            string x = u.ToXml("你的信息");
                            S.P(QQ, x);
                        }
                        else if (status == Status.infoSetup)
                            InfoSetup.Enter(QQ, Msg);
                        else if (status == Status.clubMan)
                            new ClubMan(QQ, Msg);
                        else if (status == Status.showPic)
                        {
                            if (Msg.IndexOf("[IR:pic=") > -1)
                            {
                                string link = IRQQApi.Api_GetPicLink(C.W, 2, QQ, Msg);
                                WebClient client = new WebClient();
                                string path = "C:\\Users\\Administrator\\Pictures\\show\\" + QQ + new DateTime().ToFileTime().ToString() + ".jpg";
                                client.DownloadFile(link, path);
                                S.Major($"[IR:ShowPic={path},type=2]");
                                S.P(QQ, "秀图成功");
                            }
                            else
                                S.P(QQ, "发送图片哦\n" +
                                        "再次发送<秀图>可再进入秀图状态");
                            C.SetStatus(QQ, Status.no);
                        }
                        else if (Msg == "秀图t")
                        {
                            C.SetStatus(QQ, Status.showPic);
                            S.P(QQ, "请发送图片，回复 cancel 取消");
                        }
                        else if(QQ == "839827911" && Msg.IndexOf("[IR:pic=") > -1)
                        {
                            string link = IRQQApi.Api_GetPicLink(C.W, 2, QQ, Msg);
                            S.P(QQ, link);
                        }
                        else if (QQ == "839827911" && Msg.StartsWith("ismember"))
                        {
                            Msg = Msg.GetRight("ismember").Trim();
                            S.P(QQ, C.IsMember(Msg).ToString());
                            //S.P(QQ, IRQQApi.Api_GetGroupChatLv(C.w, G.major, Msg).ToString());
                        }
                        else
                            S.P(QQ, "无法处理的消息");
                        break;
                    case 2: //群
                        //C.UpdateMemberList(true);
                        //点歌
                        if (Msg.StartsWith("点歌"))
                            NetEase.Enter(MsgFrom, Msg.GetRight("点歌").Trim());
                        else if (Msg.StartsWith("来首"))
                            NetEase.Enter(MsgFrom, Msg.GetRight("来首").Trim());
                        else if (Msg.StartsWith("甜狼点歌"))
                            NetEase.Enter(MsgFrom, Msg.GetRight("甜狼点歌").Trim());
                        else if (Msg.StartsWith("音乐"))
                            NetEase.Enter(MsgFrom, Msg.GetRight("音乐").Trim());
                        else if (Msg.StartsWith("网易云音乐"))
                            NetEase.Enter(MsgFrom, Msg.GetRight("网易云音乐").Trim());
                        else if (Msg.StartsWith("网易云"))
                            NetEase.Enter(MsgFrom, Msg.GetRight("网易云").Trim());
                        //测试群转发 XML
                        else if (Msg.IndexOf("</msg>") >= 0 && MsgFrom == G.test && TigObjF == "839827911")
                            IRQQApi.Api_SendXML(C.W, 1, 2, G.test, G.test, Msg, 0);
                        else if (C.isTest) //测试模式
                        {
                            if (MsgFrom == G.test)
                                try
                                {
                                    if (Msg == "list")
                                        S.Test(IRQQApi.Api_GetGroupMemberList(C.W, G.test));
                                    else if (Msg == "enroll")
                                    {
                                        if (C.GetMaster(MsgFrom).uin.ToString() == TigObjF)
                                            new ClubMan(MsgFrom);
                                        else
                                            S.Group(MsgFrom, "只有群主可以使用此功能");
                                    }
                                    Repeater.Enter(Msg);
                                    new Si(Msg);

                                }
                                catch (Exception e)
                                {
                                    S.Test(e.Message);
                                }
                        }
                        else //生产模式
                        {
                            if (MsgFrom == G.major)
                            {
                                if (Msg.Trim().Trim('\n').Trim() == "收到福袋，请使用新版手机QQ查看")
                                    new AntiFukubukuro(TigObjF);
                                else
                                    Repeater.Enter(Msg);
                            }
                            else if (MsgFrom == G.si)
                            {
                                new Si(Msg);
                            }
                            else if (Msg == "enroll")
                            {
                                if (C.GetMaster(MsgFrom).uin.ToString() == TigObjF)
                                    new ClubMan(MsgFrom);
                                else
                                    S.Group(MsgFrom, "只有群主可以使用此功能");
                            }
                        }
                        break;
                }


            }
            catch (Exception e)
            {
                switch (MsgType)
                {
                    case 1:
                    case 4:
                    case 5:
                        S.P(TigObjF, e.Message);
                        break;
                    case 2:
                        S.Group(MsgFrom, e.Message);
                        break;
                    default:
                        S.Test(e.Message);
                        break;
                }
            }



            return 0;
        }



        [DllExport(ExportName = nameof(IR_Create), CallingConvention = CallingConvention.StdCall)]
        public static string IR_Create()
        {
            string szBuffer = "插件名称{甜狼}\n插件版本{" + C.version.ToString() + "}\n插件作者{凌莞}\n插件说明{十中第一人工智障}\n插件skey{8956RTEWDFG3216598WERDF3}插件sdk{S3}";
            return szBuffer;
        }
        [DllExport(ExportName = nameof(IR_Message), CallingConvention = CallingConvention.StdCall)]
        public static int IR_Message(string RobotQQ, int MsgType, string Msg, string Cookies, string SessionKey, string ClientKey)
        {
            return 1;
        }
        [DllExport(ExportName = nameof(IR_SetUp), CallingConvention = CallingConvention.StdCall)]
        public static void IR_SetUp()
        {
        }
        [DllExport(ExportName = nameof(IR_DestroyPlugin), CallingConvention = CallingConvention.StdCall)]
        public static int IR_DestroyPlugin()
        {

            return 0;
        }
    }
}
