using System;
using System.Collections.Generic;
using System.Text;

namespace CornSDK.ApiTypes
{
    class GMsg
    {
        public int Req { get; set; }
        public long Random { get; set; }
        public int SubType { get; set; }
        public int AppID { get; set; }
        public string Text { get; set; }
        public string Text_Reply { get; set; }
        public int BubbleID { get; set; }
    }
}
