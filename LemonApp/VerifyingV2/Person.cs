﻿using System.Data;

namespace Clansty.tianlang
{
    class Person //做成一个实例化的类，表示某个人类
    {
        public DataRow Row { get; }
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

    }
}
