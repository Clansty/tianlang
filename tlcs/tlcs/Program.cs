using System;
using cqhttp.Cyan;
using cqhttp.Cyan.Clients;
using cqhttp.Cyan.Events.CQResponses;

namespace tlcs
{
    class Program
    {
        static void Main(string[] args)
        {
            CQApiClient client = new CQHTTPClient(
               access_url: "http://127.0.0.1:8080",
               access_token: "",
               listen_port: 5700,
               secret: ""
           );
            Console.WriteLine(
                $"QQ:{client.self_id},昵称:{client.self_nick}"
            );
            //client构造后会发送一条get_login_info请求，则可以通过
            //判断是否成功获取登陆的账号的QQ与昵称判断API是否可访问
            client.OnEvent += (client, e) => {
                
                return new EmptyResponse();
            };
        }
    }
}
