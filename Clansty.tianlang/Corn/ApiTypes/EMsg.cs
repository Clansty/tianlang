using System;
using System.Collections.Generic;
using System.Text;

namespace CornSDK.ApiTypes
{
    class EMsg
    {
        public long Seq { get; set; }
        public long TimeStamp { get; set; }
        public int Type { get; set; }
        public int SubType { get; set; }
        public string Text { get; set; }
    }
}
