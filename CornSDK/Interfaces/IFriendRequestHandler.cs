using System;
using System.Collections.Generic;
using System.Text;

namespace CornSDK
{
    public interface IFriendRequestHandler
    {
        public void OnFriendRequest(FriendRequestArgs e);
    }
}
