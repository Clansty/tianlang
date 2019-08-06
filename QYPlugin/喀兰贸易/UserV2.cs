using ServiceStack.Redis;

namespace Clansty.tianlang
{
    class User
    {
        public string Uin { get; }
        public User(string uin)
        {
            Uin = uin;
            Rds.SAdd("users", uin);
            IRedisClient client = Rds.GetClient();
            client.SetEntryInHashIfNotExists("u" + Uin, "name", "");
            client.SetEntryInHashIfNotExists("u" + Uin, "nick", "");
            client.SetEntryInHashIfNotExists("u" + Uin, "branch", "0");
            client.SetEntryInHashIfNotExists("u" + Uin, "junior", "0");
            client.SetEntryInHashIfNotExists("u" + Uin, "class", "-1");
            client.SetEntryInHashIfNotExists("u" + Uin, "enrollment", "-1");
            client.SetEntryInHashIfNotExists("u" + Uin, "step", "-1");
            client.SetEntryInHashIfNotExists("u" + Uin, "status", "0");
            client.SetEntryInHashIfNotExists("u" + Uin, "role", "0");
            client.Dispose();
        }

        public string Name
        {
            get => Get("name");
            set => Set("name", value);
        }
        public string Nick
        {
            get => Get("nick").Trim();
            set => Set("nick", value);
        }
        public bool Branch
        {
            get => Get("branch") == "1";
            set => Set("branch", value ? "1" : "0");
        }
        public bool Junior
        {
            get => Get("junior") == "1";
            set => Set("junior", value ? "1" : "0");
        }
        public int Class
        {
            get
            {
                try
                {
                    return int.Parse(Get("class"));
                }
                catch
                {
                    Class = -1;
                    return -1;
                }
            }
            set => Set("nick", value.ToString());
        }
        public int Enrollment
        {
            get
            {
                try
                {
                    return int.Parse(Get("enrollment"));
                }
                catch
                {
                    Enrollment = -1;
                    return -1;
                }
            }
            set => Set("enrollment", value.ToString());
        }
        public int Step
        {
            get
            {
                try
                {
                    return int.Parse(Get("step"));
                }
                catch
                {
                    Step = -1;
                    return -1;
                }
            }
            set => Set("step", value.ToString());
        }
        public string Namecard
        {
            get => Robot.Group.GetCard(G.major, Uin);
            set => Robot.Group.SetCard(G.major, Uin, value);
        }
        public Status Status
        {
            get
            {
                try
                {
                    return (Status)int.Parse(Get("status"));
                }
                catch
                {
                    Status = Status.no;
                    return Status.no;
                }
            }
            set => Set("status", ((int)value).ToString());
        }
        public UserType Role
        {
            get
            {
                try
                {
                    return (UserType)int.Parse(Get("role"));
                }
                catch
                {
                    Role = UserType.user;
                    return UserType.user;
                }
            }
            set => Set("role", ((int)value).ToString());
        }
        public string Grade
        {
            get
            {
                string r;
                bool graduated = false;
                switch (Enrollment)
                {
                    case 2017:
                        r = "三";
                        break;
                    case 2018:
                        r = "二";
                        break;
                    case 2019:
                        r = "一";
                        break;
                    case 10001:
                        r = "美高班";
                        return r;
                    case 10086: //这种情况通过数据库自定义前缀
                        string prefix = Get("prefix");
                        if (prefix == null)
                            prefix = "";
                        return prefix;
                    default:
                        if (Enrollment < 2000)
                            return "未知";
                        r = (Enrollment + 3).ToString() + "届";
                        graduated = true;
                        break;
                }
                if (!graduated)
                    if (Junior)
                        r = "初" + r;
                    else
                        r = "高" + r;
                else if (Junior)
                    r += "初中";
                if (r == "初一")
                    return "高一/2019届初中";
                return r;
            }
        }
        public string ProperNamecard
        {
            get
            {
                string r = Role == UserType.powerUser ? "A管理员 " : "";
                if (Branch)
                    r += "金阊";
                r += Grade;
                r += " | ";
                r += Nick == "" ? Robot.GetNick(Uin) : Nick; //如无自定义昵称则用 QQ 昵称
                return r;
            }
        }
        internal bool IsFresh => Enrollment < 1970 || Nick == "";

        public string Get(string key) => Rds.HGet("u" + Uin, key);
        public void Set(string key, string value) => Rds.HSet("u" + Uin, key, value);

        public string ToXml(string title = "用户信息")
        {
            //return "[LQ:richmsg,type=1,template_1=" +
            //       "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
            //      $"<msg serviceID=\"1\" brief=\"{title}\">" +
            //         "<item></item>" +
            //         "<item layout=\"6\">" +
            //          $"<title>{title}</title>" +
            //           "<summary>" +
            //          $"QQ: {Uin}\n" +
            //          $"年级: {Grade}\n" +
            //          $"昵称: {Nick}\n" +
            //          $"姓名: {Name}\n" +
            //          $"校区: {(Branch ? "金阊" : "本部")}\n" +
            //          $"入学年份: {Enrollment}\n" +
            //          $"初中: {Junior}\n" +
            //          $"班级: {Class}\n" +
            //          $"群名片: {Namecard}\n" +
            //          $"ProperNamecard: {ProperNamecard}" +
            //           "</summary>" +
            //           "<hr />" +
            //          $"<summary>甜狼 {C.Version}</summary>" +
            //         "</item>" +
            //       "</msg>" +
            //       ",service_id=0,msg_resid=,rand=0,seq=0,flags=0]";
            return $"[{title}]\n" +
                   $"QQ: {Uin}\n" +
                   $"年级: {Grade}\n" +
                   $"昵称: {Nick}\n" +
                   $"姓名: {Name}\n" +
                   $"校区: {(Branch ? "金阊" : "本部")}\n" +
                   $"入学年份: {Enrollment}\n" +
                   $"初中: {Junior}\n" +
                   $"班级: {Class}\n" +
                   $"群名片: {Namecard}\n" +
                   $"ProperNamecard: {ProperNamecard}";

        }
    }
}
