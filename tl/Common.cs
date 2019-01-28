using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace tianlang
{
    public static class C
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
         */

        public const int version = 57;


        public const string wp= "1980853671";
        public const string wt = "2125742312";
        public static string w;

        public static bool isTest;

        public const string err = "出现了一些错误";

        private static string memberList = "";

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

        /// <summary>
        /// 设置某个 QQ 的状态
        /// </summary>
        /// <param name="qq"></param>
        /// <param name="status"></param>
        public static void SetStatus(string qq, Status status)
        {
            Db.Exec($"UPDATE user_info SET status={(int)status} WHERE QQ='{qq}'");
            Db.Exec($"UPDATE user_info SET step=0 WHERE QQ='{qq}'");
            Db.Exec($"UPDATE user_info SET substep=0 WHERE QQ='{qq}'");
        }

        /// <summary>
        /// 根据 uid 设置状态
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="status"></param>
        public static void SetStatus(int uid, Status status)
        {
            Db.Exec($"UPDATE user_info SET status={(int)status} WHERE uid={uid}");
            Db.Exec($"UPDATE user_info SET step=0 WHERE uid={uid}");
            Db.Exec($"UPDATE user_info SET substep=0 WHERE uid={uid}");
        }

        public static Status GetStatus(string qq)
        {
            SqlDataReader r = Db.QueryReader($"SELECT status FROM user_info WHERE QQ='{qq}'");
            int s = 0;
            while (r.Read())
                s = Convert.ToInt32(r[0]);
            r.Close();
            return (Status)s;
        }

        public static string GetSession(int uid)
        {
            SqlDataReader r = Db.QueryReader($"SELECT session FROM user_info WHERE uid={uid}");
            string s = "";
            while (r.Read())
                s = (string)r[0];
            r.Close();
            return s;
        }

        public static void SetSession(int uid, string s) => Db.Exec($"UPDATE user_info SET session='{s}' WHERE uid={uid}");
        public static void SetSession(string QQ, string s) => Db.Exec($"UPDATE user_info SET session='{s}' WHERE QQ='{QQ}'");

        public static string GetSession(string QQ)
        {
            SqlDataReader r = Db.QueryReader($"SELECT session FROM user_info WHERE QQ='{QQ}'");
            string s = "";
            while (r.Read())
                s = (string)r[0];
            r.Close();
            return s;
        }

        public static void UpdateMemberList()
        {
            if (isTest)
                memberList = IRQQApi.Api_GetGroupMemberList(w, G.test);
            else
                memberList = IRQQApi.Api_GetGroupMemberList(w, G.major);
        }

        public static bool IsMember(string qq) => memberList.IndexOf(qq) >= 0;
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
        clubMan
    }

    public class GroupMemberList
    {
        //根据 IR 的 JSON
        public string card;
        public int flag;
        public int g;
        public long join_time;
        public long last_speak_time;
        public class lv
        {
            public int level;
            public int point;
        }
        public string nick;
        public int qage;
        public int role;
        public string tags;
        public long uin;
    }

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
