using System;
using System.Collections.Generic;
using System.Text;

namespace CornSDK.ApiTypes
{
    class GMRecv
    {
        public string Type { get; set; }
        public GFQQ FromQQ { get; set; }
        public long LogonQQ { get; set; }
        public TimeStamp TimeStamp { get; set; }
        public FromGroup FromGroup { get; set; }
        public GMsg Msg { get; set; }
        public File File { get; set; }
    }
}
