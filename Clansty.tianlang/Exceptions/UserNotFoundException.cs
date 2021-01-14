using System;

namespace Clansty.tianlang
{
    /// <summary>
    /// 找不到此用户
    /// </summary>
    class UserNotFoundException : Exception
    {
        internal UserNotFoundException() : base(message: "找不到此用户") { }
    }
}
