using System;

namespace Clansty.tianlang
{
    /// <summary>
    /// 姓名重名
    /// </summary>
    class DuplicateNameException : Exception
    {
        public DuplicateNameException(): base(message: "姓名重名") { }
    }
}
