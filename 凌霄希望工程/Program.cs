//using GodSharp.Sockets;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 凌霄希望工程
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    static class C
    {
        public static Socket s = new Socket(SocketType.Stream, ProtocolType.Tcp);
        //public static TcpClient client = new TcpClient(new TcpClientOptions(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 23333), new IPEndPoint(IPAddress.Parse("119.3.78.168"), 2333)) { ConnectTimeout = -1 })
        //{
        //    OnConnected = (c) =>
        //    {
        //        Console.WriteLine($"{c.RemoteEndPoint} connected.");
        //    },
        //    OnReceived = (c) =>
        //    {
        //        Console.WriteLine($"Received from {c.RemoteEndPoint}:");
        //        Console.WriteLine(string.Join(" ", c.Buffers.Select(x => x.ToString("X2")).ToArray()));

        //        c.NetConnection.Send(c.Buffers);
        //    },
        //    OnDisconnected = (c) =>
        //    {
        //        Console.WriteLine($"{c.RemoteEndPoint} disconnected.");
        //    },
        //    OnStarted = (c) =>
        //    {
        //        Console.WriteLine($"{c.RemoteEndPoint} started.");
        //    },
        //    OnStopped = (c) =>
        //    {
        //        Console.WriteLine($"{c.RemoteEndPoint} stopped.");
        //    },
        //    OnException = (c) =>
        //    {
        //        Console.WriteLine($"{c.RemoteEndPoint} exception:Message:{c.Exception.Message},StackTrace:{c.Exception.StackTrace.ToString()}.");
        //    }
        //};
    }
}
