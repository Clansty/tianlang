using System;
using System.Collections.Generic;
using System.Text;

namespace CornSDK
{
    public interface IGroupMsgHandler
    {
        public void OnGroupMsg(GroupMsgArgs e);
    }
}
