//using GodSharp.Sockets;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;
////using SuperSocket.SocketBase;
////using SuperSocket.SocketBase.Config;
////using SuperSocket.SocketBase.Protocol;


//namespace tianlang
//{
//    public static class Lingxiao
//    {
//        public static Dictionary<IPEndPoint, string> codePairs = new Dictionary<IPEndPoint, string>();
//        //public static AppServer srv;
//        private static Random r = new Random();

//        //https://github.com/godsharp/GodSharp.Socket/blob/master/samples/GodSharp.Socket.TcpServerSample/Program.cs
//        public static ITcpServer server = new TcpServer(2333)
//        {
//            OnConnected = (c) =>
//            {
//                Console.WriteLine($"{c.RemoteEndPoint} connected.");
//                //c.NetConnection.Send($"tl<{C.version}>lx<0.0.0.1>");
//            },
//            OnReceived = (c) =>
//            {
//                Console.WriteLine($"Received from {c.RemoteEndPoint}:");
//                string s = Encoding.Default.GetString(c.Buffers);
//                Console.WriteLine(s);
//                if (s.StartsWith("CODE"))
//                {
//                    s = s.GetRight("CODE").Trim();
//                    string code = r.Next(10000, 100000).ToString();
//                    codePairs[c.RemoteEndPoint] = code;
//                    S.P(s, $"你的验证码是: {code}");
//                }
//                else if (s.StartsWith("LOGIN"))
//                {
//                    s = s.GetRight("LOGIN").Trim();
//                    if (codePairs[c.RemoteEndPoint] == s)
//                        c.NetConnection.Send("SUCCESS");
//                    else
//                        c.NetConnection.Send("FAIL");
//                }
//                //c.NetConnection.Send(c.Buffers);
//            },
//            OnDisconnected = (c) =>
//            {
//                Console.WriteLine($"{c.RemoteEndPoint} disconnected.");
//            },
//            OnStarted = (c) =>
//            {
//                Console.WriteLine($"{c.LocalEndPoint} started.");
//            },
//            OnStopped = (c) =>
//            {
//                Console.WriteLine($"{c.LocalEndPoint} stopped.");
//            },
//            OnException = (c) =>
//            {
//                Console.WriteLine($"{c.RemoteEndPoint} exception:{c.Exception.StackTrace.ToString()}.");
//            },
//            OnServerException = (c) =>
//            {
//                Console.WriteLine($"{c.LocalEndPoint} exception:{c.Exception.StackTrace.ToString()}.");
//            }
//        };


//        public static void StartSrv()
//        {
//            //srv = new AppServer();
//            //Console.Write(1);
//            //bool isOK1 = srv.Setup(new ServerConfig()
//            //{
//            //    Ip = "Any",
//            //    Port = 2333,
//            //    Mode = SocketMode.Tcp
//            //});
//            //Console.Write(2);
//            //bool isOK2 = srv.Start();
//            //if (!isOK1)
//            //    Console.WriteLine("服务器无法初始化");
//            //if (!isOK2)
//            //    Console.WriteLine("服务器无法启动");
//            //if (isOK1 && isOK2)
//            //    Console.WriteLine("服务器启动成功");
//            //srv.NewSessionConnected += new SessionHandler<AppSession>(NewSessionConnected);

//            ////2.
//            //srv.NewRequestReceived += new RequestHandler<AppSession, StringRequestInfo>(NewRequestReceived);
//            server.Start();
//        }
//        //static void NewSessionConnected(AppSession session)
//        //{
//        //    Console.WriteLine("服务端得到来自客户端的连接成功");

//        //    session.Send("Welcome to SuperSocket Telnet Server");
//        //}
//        ////2.
//        //static void NewRequestReceived(AppSession session, StringRequestInfo requestInfo)
//        //{
//        //    Console.WriteLine(requestInfo.Key);

//        //    Console.WriteLine(requestInfo.Body);

//        //    switch (requestInfo.Key.ToLower())
//        //    {
//        //        case "code":
//        //            string code = r.Next(10000, 100000).ToString();
//        //            codePairs[requestInfo.Body] = code;
//        //            S.P(requestInfo.Body, $"你的验证码是: {code}");
//        //            break;
//        //    }
//        //}
//    }
//}
