using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace tianlang
{
    public struct IRQQApi
    {
        
        /// <summary>
        /// 管理员邀请对象入群，每次只能邀请一个对象，频率过快会失败
        /// </summary>
        /// <param name="RobotQQ">机器人QQ</param>
        /// <param name="ObjQQ">被邀请对象QQ</param>
        /// <param name="GroupNum">欲邀请加入的群号</param>
        [DllImport("../IRapi.dll")]
        public static extern void Api_AdminInviteGroup(string RobotQQ, string ObjQQ, string GroupNum);

        /// <summary>
        /// 取得好友列表，返回获取到的原始JSON格式信息，需自行解析
        /// </summary>
        /// <param name="RobotQQ">机器人QQ</param>
        [DllImport("../IRapi.dll")]
        public static extern string Api_GetFriendList(string RobotQQ);

        /// <summary>
        /// 取对象群名片
        /// </summary>
        /// <param name="RobotQQ">机器人QQ</param>
        /// <param name="GroupNum">群号</param>
        /// <param name="ObjQQ">欲取得群名片的QQ号码</param>
        [DllImport("../IRapi.dll")]
        public static extern string Api_GetGroupCard(string RobotQQ, string GroupNum, string ObjQQ);

        ///// <summary>
        ///// 取得群成员列表，返回获取到的原始JSON格式信息，需自行解析
        ///// </summary>
        ///// <param name="RobotQQ">机器人QQ</param>
        ///// <param name="GroupNum">欲取群成员列表群号</param>
        //[DllImport("../IRapi.dll")]
        //public static extern IntPtr Api_GetGroupMemberList(string RobotQQ, string GroupNum);

        /// <summary>
        /// 取得群成员列表
        /// </summary>
        /// <param name="RobotQQ">机器人QQ</param>
        /// <param name="GroupNum">欲取群成员列表群号</param>
        /// <returns>返回QQ号和身份Json格式信息 失败返回空</returns>
        [DllImport("../IRapi.dll")]
        public static extern IntPtr Api_GetGroupMemberList_B(string RobotQQ, string GroupNum);
        
        /// <summary>
        /// 取QQ群名
        /// </summary>
        /// <param name="RobotQQ">响应的QQ</param>
        /// <param name="GroupNum">群号</param>
        [DllImport("../IRapi.dll")]
        public static extern string Api_GetGroupName(string RobotQQ, string GroupNum);

        /// <summary>
        /// 取对象昵称
        /// </summary>
        /// <param name="RobotQQ">机器人QQ</param>
        /// <param name="ObjQQ">欲取得的QQ号码</param>
        [DllImport("../IRapi.dll")]
        public static extern string Api_GetNick(string RobotQQ, string ObjQQ);

        /// <summary>
        /// 根据图片GUID取得图片下载链接
        /// </summary>
        /// <param name="RobotQQ">机器人QQ</param>
        /// <param name="PicType">图片类型</param>
        /// <param name="ReferenceObj">参考对象</param>
        /// <param name="PicGUID">图片GUID</param>
        [DllImport("../IRapi.dll")]
        public static extern string Api_GetPicLink(string RobotQQ, int PicType, string ReferenceObj, string PicGUID);

        [DllImport("../IRapi.dll")]
        /// <summary>
        /// 取个性签名
        /// </summary>
        /// <param name="RobotQQ">响应的QQ</param>
        /// <param name="ObjQQ">对象QQ</param>
        public static extern string Api_GetSign(string RobotQQ, string ObjQQ);

        /// <summary>
        /// 处理框架所有事件请求
        /// </summary>
        /// <param name="RobotQQ">机器人QQ</param>
        /// <param name="ReQuestType">请求类型：213请求入群，214我被邀请加入某群，215某人被邀请加入群，101某人请求添加好友</param>
        /// <param name="ObjQQ">对象QQ：申请入群，被邀请人，请求添加好友人的QQ（当请求类型为214时这里请为空）</param>
        /// <param name="GroupNum">群号：收到请求的群号（好友添加时留空）</param>
        /// <param name="Handling">处理方式：10同意 20拒绝 30忽略</param>
        /// <param name="AdditionalInfo">附加信息：拒绝入群附加信息</param>
        [DllImport("../IRapi.dll")]
        public static extern void Api_HandleEvent(string RobotQQ, int ReQuestType,
            string ObjQQ, string GroupNum,
            int Handling, string AddintionalInfo);

        /// <summary>
        /// 是否QQ好友，好友返回真，非好友或获取失败返回假
        /// </summary>
        /// <param name="RobotQQ">响应的QQ</param>
        /// <param name="OBjQQ">对象QQ</param>
        [DllImport("../IRapi.dll")]
        public static extern bool Api_IfFriend(string RobotQQ, string ObjQQ);
        public static bool IsFriend(string qq) => Api_IfFriend(C.W, qq);


        [DllImport("../IRapi.dll")]
        /// <summary>
        /// 将对象移出群
        /// </summary>
        /// <param name="RobotQQ">机器人QQ</param>
        /// <param name="GroupNum">群号</param>
        /// <param name="ObjQQ">被执行对象</param>
        public static extern void Api_KickGroupMBR(string RobotQQ, string GroupNum, string ObjQQ);
        [DllImport("../IRapi.dll")]
        /// <summary>
        /// 非管理员邀请对象入群，每次只能邀请一个对象，频率过快会失败
        /// </summary>
        /// <param name="RobotQQ">机器人QQ</param>
        /// <param name="ObjQQ">被邀请人QQ号码</param>
        /// <param name="GroupNum">群号</param>
        public static extern void Api_NoAdminInviteGroup(string RobotQQ, string ObjQQ, string GroupNum);
        [DllImport("../IRapi.dll")]
        /// <summary>
        /// 向IRQQ日志窗口发送一条本插件的日志，可用于调试输出需要的内容，或定位插件错误与运行状态
        /// </summary>
        /// <param name="Log">日志信息</param>
        public static extern void Api_OutPutLog(string Log);
        [DllImport("../IRapi.dll")]
        /// <summary>
        /// 发布群公告（成功返回真，失败返回假），调用此API应保证响应QQ为管理员
        /// <summary>
        /// <param name="RobotQQ">机器人QQ</param>
        /// <param name="GroupNum">欲发布公告的群号</param>
        /// <param name="Title">公告标题</param>
        /// <param name="Content">内容</param>
        public static extern bool Api_PBGroupNotic(string RobotQQ, string GroupNum, string Title, string Content);
        [DllImport("../IRapi.dll")]
        /// <summary>
        /// 退群
        /// </summary>
        /// <param name="RobotQQ">机器人QQ</param>
        /// <param name="GroupNum">欲退出的群号</param>
        public static extern void Api_QuitGroup(string RobotQQ, string GroupNum);
        [DllImport("../IRapi.dll")]
        /// <summary>
        /// 发送普通文本消息
        /// </summary>
        /// <param name="RobotQQ">机器人QQ</param>
        /// <param name="MsgType">信息类型：1好友 2群 3讨论组 4群临时会话 5讨论组临时会话</param>
        /// <param name="MsgTo">收信对象群_讨论组：发送群、讨论组、临时会话时填写</param>
        /// <param name="ObjQQ">收信QQ</param>
        /// <param name="Msg">内容</param>
        /// <param name="ABID">气泡ID</param>
        public static extern void Api_SendMsg(string RobotQQ, int MsgType, string MsgTo,
           string ObjQQ, string Msg, int ABID);
        /// <summary>
        /// 发送XML消息
        /// </summary>
        /// <param name="RobotQQ">机器人QQ</param>
        /// <param name="SendType">发送方式：1普通 2匿名（匿名需要群开启）</param>
        /// <param name="MsgType">信息类型：1好友 2群 3讨论组 4群临时会话 5讨论组临时会话</param>
        /// <param name="MsgTo">收信对象群、讨论组：发送群、讨论组、临时时填写，如MsgType为好友可空</param>
        /// <param name="ObjQQ">收信对象QQ</param>
        /// <param name="ObjectMsg">XML代码</param>
        /// <param name="ObjCType">结构子类型：00基本 02点歌</param>
        [DllImport("../IRapi.dll")]
        public static extern void Api_SendXML(string RobotQQ, int SendType, int MsgType,
           string MsgTo, string ObjQQ, string ObjectMsg, int ObjCType);
        /// <summary>
        /// 修改对象群名片，成功返回真，失败返回假
        /// </summary>
        /// <param name="RobotQQ">机器人QQ</param>
        /// <param name="GroupNum">群号</param>
        /// <param name="ObjQQ">对象QQ：被修改名片人QQ</param>
        /// <param name="NewCard">需要修改的群名片</param>
        /// <returns>是否成功</returns>
        [DllImport("../IRapi.dll")]
        public static extern bool Api_SetGroupCard(string RobotQQ, string GroupNum,
           string ObjQQ, string NewCard);
        [DllImport("../IRapi.dll")]
        /// <summary>
        /// 修改机器人在线状态，昵称，个性签名等
        /// </summary>
        /// <param name="RobotQQ">响应的QQ</param>
        /// <param name="type">1 我在线上 2 Q我吧 3 离开 4 忙碌 5 请勿打扰 6 隐身 7 修改昵称 8 修改个性签名</param>
        /// <param name="ChangeText">修改内容，类型7和8时填写，其他为""</param>
        public static extern void Api_SetRInf(string RobotQQ, int type, string ChangeText);
        [DllImport("../IRapi.dll")]
        /// <summary>
        /// 禁言群内某人
        /// </summary>
        /// <param name="RobotQQ">机器人QQ</param>
        /// <param name="GroupNum">欲操作的群号</param>
        /// <param name="ObjQQ">欲禁言对象，如留空且机器人QQ为管理员，将设置该群为全群禁言</param>
        /// <param name="Time">禁言时间：0解除（秒），如为全群禁言，参数为非0，解除全群禁言为0</param>
        public static extern void Api_ShutUP(string RobotQQ, string GroupNum,
           string ObjQQ, int Time);
        [DllImport("../IRapi.dll")]
        /// <summary>
        /// QQ群签到，成功返回真失败返回假
        /// </summary>
        /// <param name="RobotQQ">机器人QQ</param>
        /// <param name="GroupNum">群号</param>
        public static extern bool Api_SignIn(string RobotQQ, string GroupNum);

        /// <summary>
        /// 调用一次点一下，成功返回空，失败返回理由如：每天最多给他点十个赞哦，调用此Api时应注意频率，每人每日10次，至多50人
        /// </summary>
        /// <param name="RobotQQ">机器人QQ</param>
        /// <param name="ObjQQ">被赞人QQ</param>
        [DllImport("../IRapi.dll")]
        public static extern string Api_UpVote(string RobotQQ, string ObjQQ);

        /// <summary>
        /// 查询对象或自身群聊等级 
        /// Pro可用
        /// </summary>
        /// <param name="wolf">机器人QQ</param>
        /// <param name="group">查询群号</param>
        /// <param name="QQ">需查询对象或机器人QQ</param>
        /// <returns>返回实际等级 失败返回-1</returns>
        [DllImport("../IRapi.dll")]
        public static extern int Api_GetGroupChatLv(string wolf, string group, string QQ);

        /// <summary>
        /// 消息撤回
        /// </summary>
        /// <param name="wolf"></param>
        /// <param name="group"></param>
        /// <param name="消息序号"></param>
        /// <param name="消息ID"></param>
        /// <returns></returns>
        [DllImport("../IRapi.dll")]
        public static extern int Api_WithdrawMsg(string wolf, string group, string 消息序号, string 消息ID);

    }
}
