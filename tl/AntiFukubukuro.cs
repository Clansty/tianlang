using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tianlang
{
    public class AntiFukubukuro
    {
        Random rnd = new Random();

        public AntiFukubukuro(string QQ)
        {
            int r = rnd.Next(1, 10); //1-9
            string rmsg = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><msg serviceID=\"1\" brief=\"甜狼 | 福袋随机禁言\"><item></item><item>";
            if (r == 9)
            {
                int a = ushort.MaxValue;
                IRQQApi.Api_ShutUP(C.W, G.major, QQ, a);
                rmsg = rmsg + $"<title>恭喜你，中了超级禁言套餐 ushort.MaxValue = {a} 秒</title>";
            }
            else if (r <= 2)
            {
                int a = rnd.Next(1, 6);
                IRQQApi.Api_ShutUP(C.W, G.major, QQ, a * 60);
                rmsg = rmsg + "<title>恭喜你，中了小型禁言套餐 {a} 分钟</title>";
            }
            else if (r <= 6)
            {
                int a = rnd.Next(5, 16);
                IRQQApi.Api_ShutUP(C.W, G.major, QQ, a * 60);
                rmsg = rmsg + $"<title>恭喜你，中了中型禁言套餐 {a} 分钟</title><summary>继续加油！</summary>";
            }
            else
            {
                int a = rnd.Next(16, 25);
                IRQQApi.Api_ShutUP(C.W, G.major, QQ, a * 60);
                rmsg = rmsg + $"<title>恭喜你，中了大型禁言套餐 {a} 分钟</title>";
            }
            rmsg = rmsg + "</item><item layout=\"3\"><button size=\"30\" action=\"web\" url=\"https://docs.lwqwq.com/functions/antifukubukuro\">为什么会这样</button></item></msg>";
            S.Major(rmsg);
        }
    }
}
