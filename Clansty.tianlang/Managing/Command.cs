using System;

namespace Clansty.tianlang
{
    class GroupCommand
    {
        internal string Description { get; set; }
        internal string Usage { get; set; }
        internal UserType Permission { get; set; }
        internal bool IsParamsNeeded { get; set; }
        internal Func<string, string> Func { get; set; }
    }
    class PrivateCommand
    {
        internal string Description { get; set; }
        internal UserType Permission { get; set; }
        internal Func<string, string, string> Func { get; set; }
    }
}
