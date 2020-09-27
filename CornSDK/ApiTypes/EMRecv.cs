using System;
using System.Collections.Generic;
using System.Text;

namespace CornSDK.ApiTypes
{
    class EMRecv
    {
        public string Type { get; set; }
        public QQ FromQQ { get; set; }
        public QQ OperateQQ { get; set; }
        public long LogonQQ { get; set; }
        public FromGroup FromGroup { get; set; }
        public EMsg Msg { get; set; }
    }
}
