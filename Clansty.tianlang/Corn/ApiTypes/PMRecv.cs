using System;
using System.Collections.Generic;
using System.Text;

namespace CornSDK.ApiTypes
{
    class PMRecv
    {
        public string Type { get; set; }
        public QQ FromQQ { get; set; }
        public long LogonQQ { get; set; }
        public TimeStamp TimeStamp { get; set; }
        public FromGroup FromGroup { get; set; }
        public PMsg Msg { get; set; }
        public Hb Hb { get; set; }
        public File File { get; set; }
    }
}
