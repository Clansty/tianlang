using System;
using System.Web.Script.Serialization;

namespace Clansty.tianlang
{
    public static class Test
    {
        public static void Do()
        {
            C.WriteLn(new JavaScriptSerializer().Serialize(new AppInfo()));
            while (true)
                Console.ReadLine();
        }
    }
}
