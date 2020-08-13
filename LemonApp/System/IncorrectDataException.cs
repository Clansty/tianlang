using System;

namespace Clansty.tianlang
{
    class IncorrectDataException : Exception
    {
        public IncorrectDataException() : base() { }
        public IncorrectDataException(string msg): base(msg) { }
    }
}
