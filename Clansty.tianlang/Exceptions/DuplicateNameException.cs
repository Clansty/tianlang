using System;

namespace Clansty.tianlang
{
    /// <summary>
    /// 姓名重名
    /// </summary>
    class DuplicateNameException : Exception
    {
        public DuplicateNameException(): base(message: "姓名重名") { }
        public DuplicateNameException(string name, int enrollment) : 
            base(message: $"姓名重名 姓名: {name}, 年级: {enrollment}") { }
        public DuplicateNameException(string name) : 
            base(message: $"姓名重名 姓名: {name}") { }
    }
}
