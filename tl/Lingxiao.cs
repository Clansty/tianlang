using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;


namespace tianlang
{
    public static class Lingxiao
    {
        public static Dictionary<string, string> codePairs = new Dictionary<string, string>();
        public static AppServer srv = new AppServer();
        private static Random r = new Random();
        public static void StartSrv()
        {
            bool isOK1 = srv.Setup(2333);
            bool isOK2 = srv.Start();
            if (!isOK1)
                Console.WriteLine("服务器无法初始化");
            if (!isOK2)
                Console.WriteLine("服务器无法启动");
            if (isOK1 && isOK2)
                Console.WriteLine("服务器启动成功");
            srv.NewSessionConnected += new SessionHandler<AppSession>(NewSessionConnected);
            srv.SessionClosed += NewSessionClosed;

            //2.
            srv.NewRequestReceived += new RequestHandler<AppSession, StringRequestInfo>(NewRequestReceived);

        }
        static void NewSessionConnected(AppSession session)
        {
            Console.WriteLine("服务端得到来自客户端的连接成功");

            session.Send("Welcome to SuperSocket Telnet Server");
        }
        static void NewSessionClosed(AppSession session, CloseReason aaa)
        {
            Console.WriteLine($"服务端 失去 来自客户端的连接" + session.SessionID + aaa.ToString());
        }

        //2.
        static void NewRequestReceived(AppSession session, StringRequestInfo requestInfo)
        {
            Console.WriteLine(requestInfo.Key);

            Console.WriteLine(requestInfo.Body);

            switch (requestInfo.Key.ToLower())
            {
                case "code":
                    string code = r.Next(10000, 100000).ToString();
                    codePairs[requestInfo.Body] = code;
                    S.P(requestInfo.Body, $"你的验证码是: {code}");
                    break;
            }
        }
    }
}
