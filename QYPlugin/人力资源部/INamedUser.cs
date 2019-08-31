using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clansty.tianlang
{
    interface INamedUser
    {
        string Name { get; }
        string Class { get; }
        int Enrollment { get; }
        string Grade { get; }
        bool Branch { get; }
        string ToXml(string title);
    }
}
