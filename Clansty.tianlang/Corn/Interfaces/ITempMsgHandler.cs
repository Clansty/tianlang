using System;
using System.Collections.Generic;
using System.Text;

namespace CornSDK
{
    public interface ITempMsgHandler
    {
        public void OnTempMsg(TempMsgArgs e);
    }
}
