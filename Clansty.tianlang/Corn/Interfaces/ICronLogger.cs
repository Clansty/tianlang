using System;
using System.Collections.Generic;
using System.Text;

namespace CornSDK
{
    /// <summary>
    /// 通过这个接口可以定制自己的日志记录器
    /// </summary>
    public interface ICronLogger
    {
        public void Log(string text, CornLogLevel level = CornLogLevel.Info);
    }
}
