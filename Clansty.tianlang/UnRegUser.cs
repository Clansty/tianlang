using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clansty.tianlang
{
    class UnRegUser : INamedUser
    {
        public UnRegUser(string name)
        {
            Name = name;
            var chk = RealName.Check(name);
            if (chk.Status == RealNameStatus.notFound)
                throw new Exception("玄学错误");
            if (chk.Status == RealNameStatus.e2019 || chk.Status == RealNameStatus.e2019jc)
                Enrollment = 2019;
            if (chk.Status == RealNameStatus.e2018)
                Enrollment = 2018;
            if (chk.Status == RealNameStatus.e2017)
                Enrollment = 2017;
            Branch = false;
            if (chk.Status == RealNameStatus.e2019jc)
                Branch = true;
        }
        public string Name { get; }
        public string Class => Rds.HGet("classes", Name);
        public int Enrollment { get; }
        public bool Branch { get; }
        public string Grade
        {
            get
            {
                string r;
                switch (Enrollment)
                {
                    case 2017:
                        r = "初/高三";
                        break;
                    case 2018:
                        r = "高二";
                        break;
                    case 2019:
                        r = "高一";
                        break;
                    default:
                        if (Enrollment < 2000)
                            return "未知";
                        r = (Enrollment + 3).ToString() + "届";
                        break;
                }
                return r;
            }
        }
        public string ToXml(string title = "用户信息")
        {
            return $"[{title}]\n" +
                   $"年级: {Grade}\n" +
                   $"姓名: {Name}\n" +
                   $"校区: {(Branch ? "金阊" : "本部")}\n" +
                   $"入学年份: {Enrollment}\n" +
                   $"班级: {Class}\n" +
                    "Ps. 无此人 QQ 号记载";

        }
    }
}
