using System;

namespace Clansty.tianlang
{
    public class GroupCommand
    {
        public string Description { get; set; }
        public string Usage { get; set; }
        public UserType Permission { get; set; }
        public bool IsParamsNeeded { get; set; }
        public Func<string, string> Func { get; set; }
    }
    class PrivateCommand
    {
        public string Description { get; set; }
        public UserType Permission { get; set; }
        public Func<string, string, string> Func { get; set; }
    }
}
