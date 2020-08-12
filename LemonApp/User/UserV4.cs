using System.Data;

namespace Clansty.tianlang
{
    class User : INamedUser
    {
        public string Uin { get; }
        public DataRow Row { get; }

        public User(string uin, bool createWhenNotFound = true)
        {
            Row = Sql.users.Rows.Find(uin);
            if (Row is null)
            {
                if (createWhenNotFound)
                {
                    //数据结构修改时这里要改
                    Row = Sql.users.Rows.Add(uin, "", "", 0, 0, "", 0, 0, 0);
                }
                else
                {
                    throw new UserNotFoundException();
                }
            }
        }

        public string Name
        {
            get => (string)Row["name"];
            set => Row["name"] = value;
        }

        public string Nick
        {
            get
            {
                //为了防止频繁请求还是把默认昵称信息落数据库
                //老写法还取两次
                var ret = (string)Row["nick"];
                if (string.IsNullOrEmpty(ret))
                {
                    ret = Robot.GetNick(Uin);
                    Nick = ret;
                }
                return ret;
            }

            set => Row["nick"] = value;
        }

        public bool Branch
        {
            get
            {
                //TODO
                var chk = RealName.Check(Name);
                if (chk.Status == RealNameStatus.e2019jc || chk.Status == RealNameStatus.e2018jc)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// 标识实名认证成功验证，此时不应该能自己修改姓名
        /// </summary>
        public bool Verified => VerifyMsg == RealNameVerifingResult.succeed ||
                                VerifyMsg == RealNameVerifingResult.unsupported && Name != "";

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

                return RealNameVerifingResult.wtf; //不可能运行到这里
            }
        }

        public bool Junior
        {
            get => (bool)Row["junior"];
            set => Row["junior"] = value;
        }

        public string Class => "";

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

                    return (int)Row["enrollment"];
                }
                catch
                {
                    Enrollment = 0;
                    return 0;
                }
            }
            set => Row["enrollment"] = value;
        }

        public int Step
        {
            get
            {
                try
                {
                    return (int)Row["step"];
                }
                catch
                {
                    Step = 0;
                    return 0;
                }
            }
            set => Row["step"] = value;
        }

        public string Namecard
        {
            get => Robot.GetGroupMemberCard(G.major, Uin);

            set => Robot.SetGroupMemberCard(G.major, Uin, value);
        }

        public Status Status
        {
            get
            {
                try
                {
                    return (Status)Row["status"];
                }
                catch
                {
                    Status = Status.no;
                    return Status.no;
                }
            }
            set => Row["status"] = (int)value;
        }

        public UserType Role
        {
            get
            {
                try
                {
                    return (UserType)Row["role"];
                }
                catch
                {
                    Role = UserType.user;
                    return UserType.user;
                }
            }
            set => Row["role"] = (int)value;
        }

        public string Grade
        {
            get
            {
                string r;
                var graduated = false;
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
                        var prefix = (string)Row["prefix"];
                        if (prefix == null)
                            prefix = "";
                        return prefix;
                    default:
                        if (Enrollment < 2000)
                            return "未知";
                        r = Enrollment + 3 + "届";
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
                var r = Role == UserType.powerUser ? "A管理员 " : "";
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

        public string ToXml(string title = "用户信息")
        {
            var ret = $"[{title}]\n" +
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
            return ret;
        }
    }
}