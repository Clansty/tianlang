using System;

namespace Clansty.tianlang
{
    class IncorrectDataException : Exception
    {
        internal IncorrectDataException() : base() { }
        internal IncorrectDataException(string msg): base(msg) { }
    }
}
