using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Data.SqlClient;
//using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

namespace tianlang
{
    public class C
    {
        /* 更新日志
         * 35: C# 版初代版本
         *     采用 IRQQ 框架
         * 36: 打断复读可用
         * 37: 登录成功再调用初始化函数
         * 38: 开始制作 SI 类
         *     SI 指令 sql exec 可用
         *     开始制作 query
         * 39: query 基本完成
         * 40: GetUid 测试
         * 41: Student 类
         *     测试 Fill 方法和从数据库初始化
         * 42: Student 完善，生产版本部署
         * 43: 测试生成 测试进群向导
         * 44: 解决 status 存储 bug
         * 45: 尝试修复小问题
         *     似乎可生产了
         * 46: 修复向导完成时不会自动重置状态的毛病
         * 47: 网易云点歌开发
         * 50: 网易云点歌完成
         * 51: 点歌发送空文本时显示提示
         * 52: 修复安卓 QQ 无法显示的问题
         * 53: 修复管理员拉人无法触发向导
         * 54: 测试群转发 XML
         * 55: 用户信息返回 XML
         * 56: 增加网易云点歌调用方法
         * 57: 发福袋随机禁言
         * 58: 修复打断 bug
         * 59: 福袋禁言信息使用 XML
         * 60: 社团信息注册与导入成员
         * 61: 发送秀图
         * 62: 调整群成员判断方法
         *     关闭并继续调试秀图
         *     增加私聊 whoami 命令
         * 63: 测试
         * 64: 继续调试秀图
         * 65: 秀图分布
         * 66: 部分函数细微调整，提高稳定性
         * 67: 自动接受申请
         * 68: 加强禁言套餐规格
         * 71: 福袋判定改为收到福袋
         * 72: 移除福袋
         *     开始甜狼 CMS
         * 73: 凌霄希望工程
         * 75: 删删删
         */

        public const int version = 75;
        
        public const string wp= "1980853671";
        public const string wt = "2125742312";
        public static string W { get => isTest ? wt : wp; }

        public static bool isTest;

        public const string err = "出现了一些错误";

        //private static string memberList = "";

        public static int GetUid(string qq)
        {
            SqlDataReader r = Db.QueryReader($"SELECT uid FROM user_info WHERE QQ='{qq}'");
            int uid=0;
            while (r.Read())
                uid = Convert.ToInt32(r[0]);
            r.Close();
            if (uid == 0)
                if (Db.Exec($"INSERT INTO user_info (QQ) VALUES ('{qq}')") == 1)
                {
                    r = Db.QueryReader($"SELECT uid FROM user_info WHERE QQ='{qq}'");
                    while (r.Read())
                        uid = Convert.ToInt32(r[0]);
                    r.Close();
                };
            return uid;
        }

        public static void SetStatus(string qq, Status status)
        {
            Db.Exec($"UPDATE user_info SET status={(int)status} WHERE QQ='{qq}'");
            Db.Exec($"UPDATE user_info SET step=0 WHERE QQ='{qq}'");
            Db.Exec($"UPDATE user_info SET substep=0 WHERE QQ='{qq}'");
        }
        public static void SetStatus(int uid, Status status)
        {
            Db.Exec($"UPDATE user_info SET status={(int)status} WHERE uid={uid}");
            Db.Exec($"UPDATE user_info SET step=0 WHERE uid={uid}");
            Db.Exec($"UPDATE user_info SET substep=0 WHERE uid={uid}");
        }
        //public static void SetStatus(GroupMember member, Status status) => SetStatus(member.uin.ToString(), status);
        public static void SetStatus(User u, Status status) => SetStatus(u.Uid, status);

        public static Status GetStatus(string qq)
        {
            SqlDataReader r = Db.QueryReader($"SELECT status FROM user_info WHERE QQ='{qq}'");
            int s = 0;
            while (r.Read())
                s = Convert.ToInt32(r[0]);
            r.Close();
            return (Status)s;
        }
        public static Status GetStatus(int uid)
        {
            SqlDataReader r = Db.QueryReader($"SELECT status FROM user_info WHERE uid={uid}");
            int s = 0;
            while (r.Read())
                s = Convert.ToInt32(r[0]);
            r.Close();
            return (Status)s;
        }
        //public static Status GetStatus(GroupMember member) => GetStatus(member.uin.ToString());
        public static Status GetStatus(User u) => GetStatus(u.Uid);


        public static string GetSession(int uid)
        {
            SqlDataReader r = Db.QueryReader($"SELECT session FROM user_info WHERE uid={uid}");
            string s = "";
            while (r.Read())
                s = r[0].ToString();
            r.Close();
            return s;
        }
        public static string GetSession(string QQ)
        {
            SqlDataReader r = Db.QueryReader($"SELECT session FROM user_info WHERE QQ='{QQ}'");
            string s = "";
            while (r.Read())
                s = r[0].ToString();
            r.Close();
            return s;
        }

        public static void SetSession(int uid, string s) => Db.Exec($"UPDATE user_info SET session='{s}' WHERE uid={uid}");
        public static void SetSession(string QQ, string s) => Db.Exec($"UPDATE user_info SET session='{s}' WHERE QQ='{QQ}'");
        //public static void SetSession(GroupMember member, string s) => SetSession(member.uin.ToString(), s);


        //public static void UpdateMemberList()
        //{
        //    if (isTest)
        //        memberList = IRQQApi.Api_GetGroupMemberList(w, G.test);
        //    else
        //        memberList = IRQQApi.Api_GetGroupMemberList(w, G.major);
        //}
        //public static void UpdateMemberList(bool notForce)
        //{
        //    if (memberList == "")
        //        UpdateMemberList();
        //}

        //public static bool IsMember(string qq) => IRQQApi.Api_GetGroupChatLv(C.w, G.major, qq) != -1;
        //public static bool IsMember(string qq) => Marshal.PtrToStringAnsi(IRQQApi.Api_GetGroupMemberList_B(C.W, G.major)).IndexOf(qq.Trim()) > -1;

        //public static Predicate<GroupMember> master = new Predicate<GroupMember>( //表示判断群主的方法
        //        delegate (GroupMember member)
        //        {
        //            if (member.role == "群主")
        //                return true;
        //            return false;
        //        });

        //public static List<GroupMember> GetMembers(string group)
        //{
        //    string json = Marshal.PtrToStringAnsi(IRQQApi.Api_GetGroupMemberList_B(C.W, group));
        //    JArray ja = JArray.Parse(json);
        //    List<GroupMember> l = ja.ToObject<List<GroupMember>>();
        //    return l;
        //}

        //public static string GetMaster(string group)
        //{
        //    List<GroupMember> l = GetMembers(group);
        //    GroupMember groupMaster = l.Find(C.master);
        //    return groupMaster.uin;
        //}

        public static void DownloadFile(string link,string path)
        {
            WebClient wc = new WebClient();
            wc.DownloadFile(link, path);
        }

        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

    }

    public static class G
    {
        public const string major = "646751705";
        public const string si = "690696283";
        public const string test = "828390342";
    }

    public enum Status
    {
        no,
        infoSetup,
        clubMan,
        showPic,
        cms
    }

    //public struct GroupMember
    //{
    //    public string role;
    //    public string uin;

    //    public void S(string msg) => tianlang.S.P(this, msg);
    //}

    //public struct FileRecv
    //{
    //    //{"filename":"Visual Studio 2017.lnk","filesize":"1.45 KB","filelink":"http://182.254.8.40/ftn_handler/953bc1b9487188f694d56bccdd7833657b202c7aac8c7406c7863d4a6295d4e70203c2d249fa4b66e7367c95e7fa00696e4ed6443bea65ce621defcd5197373a"}
    //    public string filename;
    //    public string filesize;
    //    public string filelink;
    //}

    public static class StringHelper
    {
        /// <summary>
        /// 取文本左边内容
        /// </summary>
        /// <param name="str">文本</param>
        /// <param name="s">标识符</param>
        /// <returns>左边内容</returns>
        public static string GetLeft(this string str, string s)
        {
            string temp = str.Substring(0, str.IndexOf(s));
            return temp;
        }


        /// <summary>
        /// 取文本右边内容
        /// </summary>
        /// <param name="str">文本</param>
        /// <param name="s">标识符</param>
        /// <returns>右边内容</returns>
        public static string GetRight(this string str, string s)
        {
            string temp = str.Substring(str.IndexOf(s) + s.Length);
            return temp;
        }

        /// <summary>
        /// 取文本中间内容
        /// </summary>
        /// <param name="str">原文本</param>
        /// <param name="leftstr">左边文本</param>
        /// <param name="rightstr">右边文本</param>
        /// <returns>返回中间文本内容</returns>
        public static string Between(this string str, string leftstr, string rightstr)
        {
            int i = str.IndexOf(leftstr) + leftstr.Length;
            string temp = str.Substring(i, str.IndexOf(rightstr, i) - i);
            return temp;
        }
    }
}
