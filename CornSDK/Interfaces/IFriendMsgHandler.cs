using System;
using System.Collections.Generic;
using System.Text;

namespace CornSDK
{
    public interface IFriendMsgHandler
    {
        public void OnFriendMsg(FriendMsgArgs e);
    }
}
