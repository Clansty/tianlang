using System.Data;
using System.IO;

namespace Clansty.tianlang
{
    class Person //做成一个实例化的类，表示某个人类
    {
        #region 构造函数 产生器
        private Person(DataRow row)
        {
            Row = row;
        }
        internal static Person[] GetPeople(string name)
        {
            //有可能有重名，返回多个
            //有可能找不到，返回空数组
            var rows = Db.persons.Select($"name = '{name.Replace("'", "''")}'");
            var r = new Person[rows.Length];
            for (int i = 0; i < rows.Length; i++)
            {
                r[i] = new Person(rows[i]);
            }
            return r;
        }
        /// <exception cref="PersonNotFoundException"></exception>
        /// <exception cref="DuplicateNameException"></exception>
        internal static Person Get(string name)
        {
            //找到了，返回
            //找不到，有重名，抛异常
            var ps = GetPeople(name);
            if (ps.Length == 0)
                throw new PersonNotFoundException();
            if (ps.Length > 1)
                throw new DuplicateNameException();
            return ps[0];
        }
        /// <exception cref="PersonNotFoundException"></exception>
        /// <exception cref="DuplicateNameException"></exception>
        internal static Person Get(string name, int enrollment)
        {
            var rows = Db.persons.Select($"name = '{name.Replace("'", "''")}' AND enrollment = {enrollment}");
            if (rows.Length == 0)
                throw new PersonNotFoundException();
            if (rows.Length > 1)
                throw new DuplicateNameException();
            return new Person(rows[0]);
        }
        /// <exception cref="PersonNotFoundException"></exception>
        internal static Person Get(int id)
        {
            var row = Db.persons.Rows.Find(id);
            if (row is null)
                throw new PersonNotFoundException();
            return new Person(row);
        }
        #endregion
        #region 实例成员 这些成员应该是只读的
        internal DataRow Row { get; }
        internal int Id => (int)Row["id"];
        internal string Name => (string)Row["name"];
        internal bool Junior => (bool)Row["junior"];
        internal bool Branch => (bool)Row["branch"];
        internal bool Board => (bool)Row["board"];
        internal Gender Gender => (Gender)Row["gender"];
        internal int Class => (int)Row["class"];
        internal int Enrollment => (int)Row["enrollment"];
        #endregion
        /// <exception cref="InvalidDataException"></exception>
        internal User User
        {
            get
            {
                var rows = Db.users.Select($"bind = {Id}");
                if (rows.Length == 0)
                    return null;
                if (rows.Length > 1)
                    throw new InvalidDataException($"身份 {Id} 绑定到了两个不同用户");
                return new User((long)rows[0]["id"]);
            }
        }
        public override string ToString()
        {
            var u = User;
            if (u is null)
                return $"ID: {Id}\n" +
                       $"姓名: {Name}\n" +
                       $"初中: {Junior}\n" +
                       $"住校生: {Board}\n" +
                       $"性别: {Gender}\n" +
                       $"入学年份: {Enrollment}\n" +
                       $"校区: {Branch}\n" +
                       $"班级: {Class}";
            return u.ToString();
        }
    }
}
