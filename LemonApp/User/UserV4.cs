using System;
using System.Data;

namespace Clansty.tianlang
{
    class User
    {
        internal long Uin => (int)Row["id"];
        internal DataRow Row { get; }

        internal User(long uin, bool createWhenNotFound = true)
        {
            Row = Db.users.Rows.Find(uin);
            if (Row is null)
            {
                if (createWhenNotFound)
                {
                    //数据结构修改时这里要改
                    Row = Db.users.Rows.Add(uin, "", "", 0, 0, "", 0, 0, 0, 0);
                }
                else
                {
                    throw new UserNotFoundException();
                }
            }
        }
        internal string Name
        {
            get
            {
                var r = Row["name"];
                if (r == DBNull.Value)
                    return "";
                return (string)Row["name"];
            }

            set => Row["name"] = value;
        }

        internal string Nick
        {
            get
            {
                //为了防止频繁请求还是把默认昵称信息落数据库
                //老写法还取两次
                var r = Row["nick"];
                string ret = null;
                if (r != DBNull.Value)
                {
                    ret = (string)r;
                }
                if (string.IsNullOrWhiteSpace(ret))
                {
                    ret = Robot.GetNick(Uin);
                    Nick = ret;
                }
                return ret;
            }

            set => Row["nick"] = value;
        }

        internal bool Branch
        {
            get
            {
                //TODO
                return false;
            }
        }

        /// <summary>
        /// 标识实名认证成功验证，此时不应该能自己修改姓名
        /// </summary>
        internal bool Verified => VerifyMsg == VerifingResult.succeed || VerifyMsg == VerifingResult.unsupported;

        internal bool Junior
        {
            get => (bool)Row["junior"];
            set => Row["junior"] = value;
        }

        internal int Class => 0;

        internal int Enrollment
        {
            get
            {
                //TODO
                return (int)Row["enrollment"];
            }
            set => Row["enrollment"] = value;
        }

        internal int Step
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

        internal string Namecard
        {
            get
            {
#if DEBUG
                return ProperNamecard;
#else
                return Robot.GetGroupMemberCard(G.major, Uin);
#endif
            }

            set
            {
#if DEBUG
                C.WriteLn($"{Uin} 群名片设置为 {value}");
#else
                Robot.SetGroupMemberCard(G.major, Uin, value);
#endif
            }
        }

        internal Status Status
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

        internal UserType Role
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

        internal string Grade
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
                        var p = Row["prefix"];
                        var prefix = "未定义";
                        if (p != DBNull.Value)
                            prefix = (string)p;
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

        internal string ProperNamecard
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
        internal bool IsMember => MemberList.major.Contains(Uin);
        internal Person Person
        {
            get
            {
                var rb = Row["bind"];
                var b = 0;
                if (rb != DBNull.Value)
                    b = (int)rb;
                if (b == 0)
                    return null;
                return Person.Get(b);
            }
        }
        internal VerifingResult VerifyMsg
        {
            get
            {
                var rb = Row["bind"];
                var b = 0;
                if (rb != DBNull.Value)
                    b = (int)rb;
                if (b != 0)
                    return VerifingResult.succeed;
                if (string.IsNullOrWhiteSpace(Name))
                    return VerifingResult.nameEmpty; //NameEmpty 视为未实名
                //尝试绑定
                //XXX:绑定过程是否独立出一个函数
                try
                {
                    var p = Person.Get(Name);
                    if (p.User is null)
                    {
                        Row["bind"] = p.Id;
                        return VerifingResult.succeed;
                    }
                    if (!SupportedEnrollment.Contains(Enrollment))
                        return VerifingResult.unsupported;//已占用但是年级不一样且不支持，其实是重名了
                    return VerifingResult.occupied;
                }
                catch (PersonNotFoundException)
                {
                    if (!SupportedEnrollment.Contains(Enrollment))
                        return VerifingResult.unsupported;
                    return VerifingResult.notFound;
                }
                catch (DuplicateNameException)
                {
                    try
                    {
                        var p = Person.Get(Name, Enrollment);
                        if (p.User is null)
                        {
                            Row["bind"] = p.Id;
                            return VerifingResult.succeed;
                        }
                        if (!SupportedEnrollment.Contains(Enrollment))
                            return VerifingResult.unsupported;//已占用但是年级不一样且不支持，其实是重名了
                        return VerifingResult.occupied;
                    }
                    catch (PersonNotFoundException)
                    {
                        if (!SupportedEnrollment.Contains(Enrollment))
                            return VerifingResult.unsupported;
                        return VerifingResult.notFound;
                    }
                    catch
                    {
                        throw;
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        public override string ToString()
        {
            return ToString("用户信息");
        }
        internal string ToString(string title)
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