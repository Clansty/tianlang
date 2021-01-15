using System;
using System.Data;
using Mirai_CSharp.Models;

namespace Clansty.tianlang
{
    class User
    {
        internal long Uin => (long) Row["id"];
        internal DataRow Row { get; }

        internal User(long uin, bool createWhenNotFound = true)
        {
            Row = Db.users.Rows.Find(uin);
            if (Row is null)
            {
                if (createWhenNotFound)
                {
                    //数据结构修改时这里要改
                    Row = Db.users.Rows.Add(uin, "", "", 0, 0, "", 0, 0, 0, 0, 0);
                }
                else
                {
                    throw new UserNotFoundException();
                }
            }

            _ = VerifyMsg;
        }

        internal User(DataRow row)
        {
            Row = row;
        }

        internal string Name
        {
            get
            {
                var p = Person;
                if (p is null)
                {
                    var r = Row["name"];
                    if (r == DBNull.Value)
                        return "";
                    return (string) Row["name"];
                }

                return p.Name;
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
                    ret = (string) r;
                }

                if (string.IsNullOrWhiteSpace(ret))
                {
                    Nick = "No Name";
                }

                return ret;
            }

            set => Row["nick"] = value;
        }

        internal bool Branch
        {
            get
            {
                var p = Person;
                if (p is null)
                    return false;
                return p.Branch;
            }
        }

        /// <summary>
        /// 标识实名认证成功验证，此时不应该能自己修改姓名
        /// </summary>
        internal bool Verified => VerifyMsg == VerifingResult.succeed || VerifyMsg == VerifingResult.unsupported;

        internal bool Junior
        {
            get
            {
                if (Enrollment == 2019 || Enrollment == 2016)
                    return (bool) Row["junior"];
                var p = Person;
                if (p is null)
                {
                    return false;
                } //2020

                return p.Junior;
            }

            set => Row["junior"] = value;
        }

        internal long Enrollment
        {
            get
            {
                var p = Person;
                if (p is null)
                    return (long) Row["enrollment"];
                return p.Enrollment;
            }
            set => Row["enrollment"] = value;
        }

        internal long Step
        {
            get
            {
                try
                {
                    return (long) Row["step"];
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
                var t = C.QQ.NthsBot.GetGroupMemberInfoAsync(G.major, Uin);
                t.Wait();
                return t.Result.Name;
#endif
            }

            set
            {
#if DEBUG
                C.WriteLn($"{Uin} 群名片设置为 {value}");
#else
                C.QQ.NthsBot.ChangeGroupMemberInfoAsync(G.major, Uin,
                    new GroupMemberCardInfo
                    {
                        Name = value
                    }
                );
#endif
            }
        }

        internal Status Status
        {
            get
            {
                try
                {
                    return (Status) Row["status"];
                }
                catch
                {
                    Status = Status.no;
                    return Status.no;
                }
            }
            set => Row["status"] = (long) value;
        }

        internal UserType Role
        {
            get
            {
                try
                {
                    return (UserType) Row["role"];
                }
                catch
                {
                    Role = UserType.user;
                    return UserType.user;
                }
            }
            set => Row["role"] = (int) value;
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
                            prefix = (string) p;
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
                long b = 0;
                if (rb != DBNull.Value)
                    b = (long) rb;
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
                long b = 0;
                if (rb != DBNull.Value)
                    b = (long) rb;
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

                    return !SupportedEnrollment.Contains(Enrollment)
                        ? VerifingResult.unsupported
                        : VerifingResult.occupied;
                }
                catch (PersonNotFoundException)
                {
                    return !SupportedEnrollment.Contains(Enrollment)
                        ? VerifingResult.unsupported
                        : VerifingResult.notFound;
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

                        return !SupportedEnrollment.Contains(Enrollment)
                            ? VerifingResult.unsupported
                            : VerifingResult.occupied;
                    }
                    catch (PersonNotFoundException)
                    {
                        return !SupportedEnrollment.Contains(Enrollment)
                            ? VerifingResult.unsupported
                            : VerifingResult.notFound;
                    }
                    catch (Exception ex)
                    {
                        C.WriteLn(ex);
                        return VerifingResult.error;
                    }
                }
                catch (Exception ex)
                {
                    C.WriteLn(ex);
                    return VerifingResult.error;
                }
            }
        }

        internal long TgUid
        {
            get
            {
                try
                {
                    return (long) Row["tg"];
                }
                catch
                {
                    TgUid = 0;
                    return 0;
                }
            }
            set => Row["tg"] = value;
        }

        public override string ToString()
        {
            return ToString("用户信息");
        }

        internal string ToString(string title)
        {
            _ = VerifyMsg;
            var ret = $"[{title}]\n" +
                      $"QQ: {Uin}\n" +
                      $"昵称: {Nick}\n" +
                      $"姓名: {Name}\n" +
                      $"入学年份: {Enrollment}\n" +
                      $"年级: {Grade}\n" +
                      $"Telegram Uid: {TgUid}\n" +
                      $"实名状态: {VerifyMsg}\n" +
                      $"是大群成员: {IsMember}";
            if (IsMember)
                ret += "\n" +
                       $"群名片: {Namecard}\n" +
                       $"理想群名片: {ProperNamecard}\n" +
                       $"身份: {Role}";
            var p = Person;
            if (p != null)
                ret += "\n" +
                       $"实名身份 ID: {p.Id}\n" +
                       $"班级: {p.Class}\n" +
                       (p.FormerClass is null ? "" : $"曾经班级: {p.FormerClass}\n") +
                       (p.Sex == Sex.unknown ? "" : $"性别: {p.Sex}\n") +
                       $"校区: {(p.Branch ? "金阊" : "本部")}\n" +
                       $"住校生: {p.Board}";
            return ret;
        }
    }
}