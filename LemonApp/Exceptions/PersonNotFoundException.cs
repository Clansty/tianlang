using System;

namespace Clansty.tianlang
{
    /// <summary>
    /// 查无此人
    /// </summary>
    class PersonNotFoundException : Exception
    {
        internal PersonNotFoundException() : base(message: "查无此人") { }
    }
}
