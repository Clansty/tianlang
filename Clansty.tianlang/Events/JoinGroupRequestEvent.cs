using System;
using Native.Sdk.Cqp.Enum;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;

namespace Clansty.tianlang.Events
{
    public class JoinGroupRequestEvent : IGroupAddRequest
    {
        public void GroupAddRequest(object sender, CQGroupAddRequestEventArgs e)
        {
            if (e.SubType == CQGroupAddRequestType.ApplyAddGroup)
            {
                if (e.FromGroup == G.major)
                {
                    var msg = e.AppendMessage.Contains("答案：")
                        ? e.AppendMessage.GetRight("答案：").Trim()
                        : e.AppendMessage;
                    var u = new User(e.FromQQ);
                    C.Write("有人 ", ConsoleColor.DarkCyan);
                    C.Write(e.FromQQ, ConsoleColor.Cyan);
                    C.Write(" 加群 ", ConsoleColor.DarkCyan);
                    C.Write(msg, ConsoleColor.Cyan);
                    C.WriteLn("");
                    if (u.Role == UserType.blackListed)
                    {
                        e.Request.SetGroupAddRequest(CQGroupAddRequestType.ApplyAddGroup, CQResponseType.FAIL,
                            "blacklisted");
                        S.Si(u.ToXml(Strs.Get("addRejected", Strs.Get("blacklisted"))) + $"\n申请信息: {msg}", e.CQApi);
                        return;
                    }

                    if (Rds.SContains("knownUsers", e.FromQQ))
                    {
                        e.Request.SetGroupAddRequest(CQGroupAddRequestType.ApplyAddGroup, CQResponseType.PASS);
                        S.Si(u.ToXml("加群申请已同意: 白名单用户") + $"\n申请信息: {msg}", e.CQApi);
                        return;
                    }

                    if (msg.IndexOf(" ", StringComparison.Ordinal) < 0)
                    {
                        //TODO 只填了姓名，但是姓名找得到可以同意
                        if (u.VerifyMsg == RealNameVerifingResult.succeed && u.Name == msg)
                        {
                            e.Request.SetGroupAddRequest(CQGroupAddRequestType.ApplyAddGroup, CQResponseType.PASS);
                            S.Si(u.ToXml("加群申请已同意: 已实名用户") + $"\n申请信息: {msg}", e.CQApi);
                            return;
                        }

                        var chk2 = RealName.Check(msg);
                        if (chk2.OccupiedQQ != null && chk2.OccupiedQQ != e.FromQQ &&
                            chk2.Status != RealNameStatus.notFound)
                        {
                            e.Request.SetGroupAddRequest(CQGroupAddRequestType.ApplyAddGroup, CQResponseType.FAIL,
                                "此身份已有一个账户加入，如有疑问请联系管理员");
                            S.Si(u.ToXml("加群申请已拒绝: 此人已存在") + $"\n申请信息: {msg}", e.CQApi);
                            return;
                        }

                        if (chk2.OccupiedQQ == null && chk2.Status != RealNameStatus.notFound)
                        {
                            //尝试进行验证
                            u.Name = msg;
                            if (u.Verified)
                            {
                                e.Request.SetGroupAddRequest(CQGroupAddRequestType.ApplyAddGroup, CQResponseType.PASS);
                                S.Si(u.ToXml("加群申请已同意: 实名认证成功") + $"\n申请信息: {msg}", e.CQApi);
                                return;
                            }

                            var err = u.VerifyMsg;
                            e.Request.SetGroupAddRequest(CQGroupAddRequestType.ApplyAddGroup, CQResponseType.FAIL,
                                "玄学错误，请联系管理员");
                            S.Si(u.ToXml("加群申请已拒绝: 玄学错误，此错误不应该由本段代码处理") + $"\n申请信息: {msg}\n未预期的错误: {err}", e.CQApi);
                            return;
                        }

                        e.Request.SetGroupAddRequest(CQGroupAddRequestType.ApplyAddGroup, CQResponseType.FAIL,
                            Strs.Get("formatIncorrect"));
                        S.Si(u.ToXml(Strs.Get("addRejected", Strs.Get("formatErr"))) + $"\n申请信息: {msg}", e.CQApi);
                        return;
                    }

                    var enr = UserInfo.ParseEnrollment(msg.GetLeft(" "));
                    var name = msg.GetRight(" ").Trim();

                    if (enr < 1970)
                    {
                        e.Request.SetGroupAddRequest(CQGroupAddRequestType.ApplyAddGroup, CQResponseType.FAIL,
                            Strs.Get("EnrFormatErr"));
                        S.Si(u.ToXml(Strs.Get("addRejected", Strs.Get("EnrFormatErr"))) + $"\n申请信息: {msg}", e.CQApi);
                        return;
                    }

                    u.Enrollment = enr;

                    if (enr != 2017 && enr != 2018 && enr != 2019)
                    {
                        u.Name = name;
                        u.Junior = UserInfo.ParseJunior(msg.GetLeft(" "));
                        S.Si(u.ToXml("此年级不支持自动审核") + $"\n申请信息: {msg}", e.CQApi);
                        return;
                    }

                    if (u.Verified)
                    {
                        if (u.Name != name)
                        {
                            e.Request.SetGroupAddRequest(CQGroupAddRequestType.ApplyAddGroup, CQResponseType.FAIL,
                                "已实名账户请用登记姓名加入，如有问题请联系管理员");
                            S.Si(u.ToXml("加群申请已拒绝: 已实名账户尝试 override") + $"\n申请信息: {msg}", e.CQApi);
                            return;
                        }

                        e.Request.SetGroupAddRequest(CQGroupAddRequestType.ApplyAddGroup, CQResponseType.PASS);
                        S.Si(u.ToXml("加群申请已同意: 已实名用户") + $"\n申请信息: {msg}", e.CQApi);
                        return;
                    }

                    var chk = RealName.Check(name);

                    if (chk.Status == RealNameStatus.notFound)
                    {
                        e.Request.SetGroupAddRequest(CQGroupAddRequestType.ApplyAddGroup, CQResponseType.FAIL, "查无此人");
                        S.Si(u.ToXml("加群申请已拒绝: 查无此人") + $"\n申请信息: {msg}", e.CQApi);
                        return;
                    }

                    if (chk.OccupiedQQ != null && chk.OccupiedQQ != e.FromQQ)
                    {
                        e.Request.SetGroupAddRequest(CQGroupAddRequestType.ApplyAddGroup, CQResponseType.FAIL,
                            "此身份已有一个账户加入，如有疑问请联系管理员");
                        S.Si(u.ToXml("加群申请已拒绝: 此人已存在") + $"\n申请信息: {msg}", e.CQApi);
                        return;
                    }

                    //if (chk.Status == RealNameStatus.e2017 && enr != 2017)
                    //{
                    //    e.Reject("姓名与年级不匹配，请检查");
                    //    S.Si(u.ToXml("加群申请已拒绝: 姓名与年级不匹配") + $"\n申请信息: {msg}");
                    //    return;
                    //}
                    //if (chk.Status == RealNameStatus.e2018 && enr != 2018)
                    //{
                    //    e.Reject("姓名与年级不匹配，请检查");
                    //    S.Si(u.ToXml("加群申请已拒绝: 姓名与年级不匹配") + $"\n申请信息: {msg}");
                    //    return;
                    //}
                    //if (chk.Status == RealNameStatus.e2018jc && enr != 2018)
                    //{
                    //    e.Reject("姓名与年级不匹配，请检查");
                    //    S.Si(u.ToXml("加群申请已拒绝: 姓名与年级不匹配") + $"\n申请信息: {msg}");
                    //    return;
                    //}
                    //if (chk.Status == RealNameStatus.e2019 && enr != 2019)
                    //{
                    //    e.Reject("姓名与年级不匹配，请检查");
                    //    S.Si(u.ToXml("加群申请已拒绝: 姓名与年级不匹配") + $"\n申请信息: {msg}");
                    //    return;
                    //}
                    //if (chk.Status == RealNameStatus.e2019jc && enr != 2019)
                    //{
                    //    e.Reject("姓名与年级不匹配，请检查");
                    //    S.Si(u.ToXml("加群申请已拒绝: 姓名与年级不匹配") + $"\n申请信息: {msg}");
                    //    return;
                    //}


                    if (chk.OccupiedQQ == null)
                    {
                        //尝试进行验证
                        u.Name = name;
                        u.Junior = UserInfo.ParseJunior(msg.GetLeft(" "));
                        if (u.Verified)
                        {
                            e.Request.SetGroupAddRequest(CQGroupAddRequestType.ApplyAddGroup, CQResponseType.PASS);
                            S.Si(u.ToXml("加群申请已同意: 实名认证成功") + $"\n申请信息: {msg}", e.CQApi);
                            return;
                        }

                        var err = u.VerifyMsg;
                        e.Request.SetGroupAddRequest(CQGroupAddRequestType.ApplyAddGroup, CQResponseType.FAIL,
                            "玄学错误，请联系管理员");
                        S.Si(u.ToXml("加群申请已拒绝: 玄学错误，此错误不应该由本段代码处理\n" +
                                     $"申请信息: {msg}\n未预期的错误: {err}"), e.CQApi);
                        return;
                    }

                    S.Si(u.ToXml("申请用户信息"), e.CQApi);
                }
            }

            if (e.SubType == CQGroupAddRequestType.RobotBeInviteAddGroup)
                e.Request.SetGroupAddRequest(CQGroupAddRequestType.RobotBeInviteAddGroup, CQResponseType.PASS);
        }
    }
}