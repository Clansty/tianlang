using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clansty.tianlang
{
    class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base(message: "找不到此用户") { }
    }
}
