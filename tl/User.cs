using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Text.RegularExpressions;

namespace tianlang
{
    class User
    {
        // 构造函数
        /// <summary>
        /// 创建 Student 并用 UID 填充
        /// </summary>
        /// <param name="Uid"></param>
        public User(int Uid)
        {
            this.Uid = Uid;
            DataSet data = Db.Query($"SELECT * FROM user_info WHERE uid={this.Uid}");
            QQ = data.Tables[0].Rows[0]["QQ"].ToString();
            Name = data.Tables[0].Rows[0]["name"] == DBNull.Value ? "" : data.Tables[0].Rows[0]["name"].ToString();
            Class = data.Tables[0].Rows[0]["class"] == DBNull.Value ? 0 : (int)data.Tables[0].Rows[0]["class"];
            Branch = data.Tables[0].Rows[0]["branch"] == DBNull.Value ? false : (bool)data.Tables[0].Rows[0]["branch"];
            Nick = data.Tables[0].Rows[0]["nick"] == DBNull.Value ? "" : data.Tables[0].Rows[0]["nick"].ToString();
            Junior = data.Tables[0].Rows[0]["junior"] == DBNull.Value ? false : (bool)data.Tables[0].Rows[0]["junior"];
            Enrollment = data.Tables[0].Rows[0]["enrollment"] == DBNull.Value ? 0 : (int)data.Tables[0].Rows[0]["enrollment"];
            NameCard = IRQQApi.Api_GetGroupCard(C.w, G.major, QQ);
        }
        /// <summary>
        /// 创建 Student 并用 QQ 填充
        /// </summary>
        /// <param name="QQ"></param>
        public User(string QQ)
        {
            C.GetUid(QQ);
            this.QQ = QQ;
            DataSet data = Db.Query($"SELECT * FROM user_info WHERE QQ='{this.QQ}'");
            Uid = (int)data.Tables[0].Rows[0]["uid"];
            Name = data.Tables[0].Rows[0]["name"] == DBNull.Value ? "" : data.Tables[0].Rows[0]["name"].ToString();
            Class = data.Tables[0].Rows[0]["class"] == DBNull.Value ? 0 : (int)data.Tables[0].Rows[0]["class"];
            Branch = data.Tables[0].Rows[0]["branch"] == DBNull.Value ? false : (bool)data.Tables[0].Rows[0]["branch"];
            Nick = data.Tables[0].Rows[0]["nick"] == DBNull.Value ? "" : data.Tables[0].Rows[0]["nick"].ToString();
            Junior = data.Tables[0].Rows[0]["junior"] == DBNull.Value ? false : (bool)data.Tables[0].Rows[0]["junior"];
            Enrollment = data.Tables[0].Rows[0]["enrollment"] == DBNull.Value ? 0 : (int)data.Tables[0].Rows[0]["enrollment"];
            NameCard = IRQQApi.Api_GetGroupCard(C.w, G.major, this.QQ);

        }

        public User()
        {
        }

        // 成员
        public int Uid = 0;
        public string QQ = "";
        public string Name = "";
        public int Class = 0;
        public bool Branch = false;
        public string Nick = "";
        public readonly string NameCard = "";
        public bool Junior = false;
        // 系统维护的字段
        private string grade = "";
        private int enrollment = 0;
        // 封装字段
        public string Grade {
            get => grade;
            set
            {
                switch (value)
                {
                    case "高一":
                    case "高1":
                        enrollment = 2018;
                        grade = "高一";
                        break;
                    case "高二":
                    case "高2":
                        enrollment = 2017;
                        grade = "高二";
                        break;
                    case "高三":
                    case "高3":
                        enrollment = 2016;
                        grade = "高三";
                        break;
                    case "初一":
                    case "初1":
                        enrollment = 2018;
                        grade = "初一";
                        break;
                    case "初二":
                    case "初2":
                        enrollment = 2017;
                        grade = "初二";
                        break;
                    case "初三":
                    case "初3":
                        enrollment = 2016;
                        grade = "初三";
                        break;

                }
            }
        }
        public int Enrollment
        {
            get => enrollment;
            set
            {
                if (value == 0)
                    return;
                if (10 < value && value < 20)
                value += 2000;

                if (Junior)
                    switch (value)
                    {
                        case 2018:
                            grade = "初一";
                            break;
                        case 2017:
                            grade = "初二";
                            break;
                        case 2016:
                            grade = "初三";
                            break;
                    }
                else
                    switch (value)
                    {
                        case 2018:
                            grade = "高一";
                            break;
                        case 2017:
                            grade = "高二";
                            break;
                        case 2016:
                            grade = "高三";
                            break;
                    }
                if (value < 2016)
                    grade = (value + 3).ToString() + "届" + (Junior ? "初中" : "");
                enrollment = value;
            }
        }
        /// <summary>
        /// 解析，填充
        /// </summary>
        /// <param name="origin">校区年级班级</param>
        public void Fill(string origin)
        {
            if (origin.IndexOf("金阊") != -1)
            {
                Branch = true;
                origin = origin.GetRight("金阊").Trim();
            }
            if (origin.IndexOf("初") != -1)
                Junior = true;
            try
            {
                string tmp = "";
                if (Junior)
                    tmp = origin.Substring(origin.IndexOf("初"), 2);
                else
                    tmp = origin.Substring(origin.IndexOf("高"), 2);
                if (tmp != "")
                {
                    Grade = tmp;
                    origin = origin.GetRight(tmp).Trim();
                }
            }
            catch
            {
                int tmp = 0;
                if (origin.IndexOf("届") != -1)
                    tmp = Convert.ToInt32(Regex.Replace(origin.GetLeft("届").Trim(), @"[^0-9]+", "").Trim());
                if (tmp > 0)
                {
                    Enrollment = tmp - 3;
                    origin = origin.GetRight("届").Trim();
                }
            }

            if (origin.IndexOf("班") != -1)
                origin = origin.GetLeft("班").Trim();
            try
            {
                int tmp = 0;
                tmp = Convert.ToInt32(Regex.Replace(origin.Trim(), @"[^0-9]+", "").Trim());
                if (tmp > 0)
                    Class = tmp;
                else
                    Convert.ToInt32("boom"); // goto catch
            }
            catch
            {
                if (origin.IndexOf("一") != -1)
                    Class = 1;
                else if (origin.IndexOf("二") != -1)
                    Class = 2;
                else if (origin.IndexOf("三") != -1)
                    Class = 3;
                else if (origin.IndexOf("四") != -1)
                    Class = 4;
                else if (origin.IndexOf("五") != -1)
                    Class = 5;
                else if (origin.IndexOf("六") != -1)
                    Class = 6;
                else if (origin.IndexOf("七") != -1)
                    Class = 7;
                else if (origin.IndexOf("八") != -1)
                    Class = 8;
                else if (origin.IndexOf("九") != -1)
                    Class = 9;
                else if (origin.IndexOf("十") != -1)
                    Class = 10;

            }
        }

        public override string ToString()
        {
            return $"uid: {Uid}\n" +
                $"qq: {QQ}\n" +
                $"name: {Name}\n" +
                $"class: {Class}\n" +
                $"branch: {(Branch ? "金阊" : "本部")}\n" +
                $"nick: {Nick}\n" +
                $"junior: {Junior}\n" +
                $"enrollment: {Enrollment}\n" +
                $"grade: {Grade}\n" +
                $"namecard: {NameCard}";
        }
    }
}
