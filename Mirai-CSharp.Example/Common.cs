using System;
using System.Collections.Generic;
using System.Text;

namespace Mirai_CSharp.Tianlang
{
    static class C
    {
        public const bool debug = true;
        public const string host = "127.0.0.1";
        public const int port = 8080;
        public const string key = "QwQQAQOwO";
        public static long Self
        {
            get
            {
                if (debug)
                    return 2981882373;
                else
                    return 1980853671;
            }
        }
    }
}
