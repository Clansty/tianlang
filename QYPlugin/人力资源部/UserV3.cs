using ServiceStack.Redis;

namespace Clansty.tianlang
{
    class User : INamedUser
    {
        public string Uin { get; }
        public User(string uin)
        {
            Uin = uin;
            if (!Rds.SContains("users", uin))
            {
                Rds.SAdd("users", uin);
                IRedisClient client = Rds.GetClient();
                client.SetEntryInHashIfNotExists("u" + Uin, "name", "");
                client.SetEntryInHashIfNotExists("u" + Uin, "nick", "");
                client.SetEntryInHashIfNotExists("u" + Uin, "junior", "0");
                client.SetEntryInHashIfNotExists("u" + Uin, "enrollment", "-1");
                client.SetEntryInHashIfNotExists("u" + Uin, "step", "-1");
                client.SetEntryInHashIfNotExists("u" + Uin, "status", "0");
                client.SetEntryInHashIfNotExists("u" + Uin, "role", "0");
                client.Dispose();
            }
        }

        public string Name
        {
            get => Get("name");
            set => Set("name", value);
        }
        public string Nick
        {
            get => Get("nick").Trim() == "" ? Robot.GetNick(Uin) : Get("nick").Trim();
            set => Set("nick", value);
        }
        public bool Branch
        {
            get
            {
                var chk = RealName.Check(Name);
                if (chk.Status == RealNameStatus.e2019jc || chk.Status == RealNameStatus.e2018jc)
                    return true;
                return false;
            }
        }
        /// <summary>
        /// 标识实名认证成功验证，此时不应该能自己修改姓名
        /// </summary>
        public bool Verified => VerifyMsg == RealNameVerifingResult.succeed || VerifyMsg == RealNameVerifingResult.unsupported;
        public RealNameVerifingResult VerifyMsg
        {
            get
            {
                if (Enrollment != 2017 && Enrollment != 2018 && Enrollment != 2019)
                    return RealNameVerifingResult.unsupported;

                var bind = RealName.Bind(Uin, Name);
                if (bind == RealNameBindingResult.noNeed)
                    return RealNameVerifingResult.succeed;
                if (bind == RealNameBindingResult.succeed)
                    return RealNameVerifingResult.succeed;
                if (bind == RealNameBindingResult.notFound)
                    return RealNameVerifingResult.notFound;
                if (bind == RealNameBindingResult.occupied)
                    return RealNameVerifingResult.occupied;

                return RealNameVerifingResult.wtf;//不可能运行到这里
            }
        }

        public bool Junior
        {
            get => Get("junior") == "1";
            set => Set("junior", value ? "1" : "0");
        }
        public string Class => Rds.HGet("classes", Name);
        public int Enrollment
        {
            get
            {
                try
                {
                    var chk = RealName.Check(Name);
                    if (chk.Status == RealNameStatus.e2017)
                        return 2017;
                    if (chk.Status == RealNameStatus.e2018 || chk.Status == RealNameStatus.e2018jc)
                        return 2018;
                    if (chk.Status == RealNameStatus.e2019 || chk.Status == RealNameStatus.e2019jc)
                        return 2019;

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
                    case 2018:
                        r = "三";
                        break;
                    case 2019:
                        r = "二";
                        break;
                    case 2020:
                        r = "一";
                        break;
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
                    return "高一/2020届初中";
                if (r == "初二")
                    return "高二/2019届初中";
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
                r += Nick; //如无自定义昵称则用 QQ 昵称
                if (!Verified)
                    r = "未实名" + r;
                return r;
            }
        }
        internal bool IsFresh => Name == ""; //紧急 fix
        public bool IsMember => MemberList.major.Contains(Uin);

        public string Get(string key) => Rds.HGet("u" + Uin, key);
        public void Set(string key, string value) => Rds.HSet("u" + Uin, key, value);

        public string ToXml(string title = "用户信息")
        {
            var ret =  $"[{title}]\n" +
                       $"QQ: {Uin}\n" +
                       $"年级: {Grade}\n" +
                       $"昵称: {Nick}\n" +
                       $"姓名: {Name}\n" +
                       $"校区: {(Branch ? "金阊" : "本部")}\n" +
                       $"入学年份: {Enrollment}\n" +
                       $"初中: {Junior}\n" +
                       $"班级: {Class}\n" +
                       $"群名片: {Namecard}\n" +
                       $"理想群名片: {ProperNamecard}\n" +
                       $"IsMember: {IsMember}\n" +
                       $"身份: {Role}\n" +
                       $"实名状态: {VerifyMsg}";
            //test for new struct for real name related information 191230
            using (var client = Rds.GetClient())
            {
                var ht = client.GetAllEntriesFromHash($"name{Name}");
                if (ht.Count > 0)
                {
                    ret += "\n--新版用户信息 Beta--";
                    foreach (var kvp in ht)
                    {
                        ret += $"\n{kvp.Key}: {kvp.Value}";
                    }
                }
            }
            //TODO: i will transfer real-name check info to the new struct soon
            return ret;
        }
    }
}
