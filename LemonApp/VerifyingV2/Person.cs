using System.Data;

namespace Clansty.tianlang
{
    class Person //做成一个实例化的类，表示某个人类
    {
        #region 构造函数 产生器
        private Person(DataRow row)
        {
            Row = row;
        }
        public static Person[] GetPeople(string name)
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
        public static Person Get(string name)
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
        public static Person Get(string name, int enrollment)
        {
            var rows = Db.persons.Select($"name = '{name.Replace("'", "''")}' AND enrollment = {enrollment}");
            if (rows.Length == 0)
                throw new PersonNotFoundException();
            if (rows.Length > 1)
                throw new DuplicateNameException();
            return new Person(rows[0]);
        }
        public static Person Get(string name, int enrollment, int _class)
        {
            var rows = Db.persons.Select($"name = '{name.Replace("'", "''")}' AND " +
                $"enrollment = {enrollment} AND " +
                $"class = {_class}");
            if (rows.Length == 0)
                throw new PersonNotFoundException();
            if (rows.Length > 1)
                throw new DuplicateNameException();
            return new Person(rows[0]);
        }
        public static Person Get(int id)
        {
            var row = Db.persons.Rows.Find(id);
            if (row is null)
                throw new PersonNotFoundException();
            return new Person(row);
        }
        #endregion
        #region 实例成员 这些成员应该是只读的
        public DataRow Row { get; }
        public int Id => (int)Row["id"];
        public string Name => (string)Row["name"];
        public bool Junior => (bool)Row["junior"];
        public bool Branch => (bool)Row["branch"];
        public bool Board => (bool)Row["board"];
        public Gender Gender => (Gender)Row["gender"];
        public int Class => (int)Row["class"];
        public int Enrollment => (int)Row["enrollment"];
        #endregion
        public User User
        {
            get
            {
                var rows = Db.users.Select($"bind = {Id}");
                if (rows.Length == 0)
                    return null;
                if (rows.Length > 1)
                    throw new IncorrectDataException($"身份 {Id} 绑定到了两个不同用户");
                return new User((long)rows[0]["id"]);
            }
        }
    }
}
