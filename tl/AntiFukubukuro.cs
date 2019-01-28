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
            if (r <= 3)
                S.Major($"[IR:at={QQ}] 你很幸运哦，完美的避开了禁言套餐");
            else if (r <= 7)
            {
                int a = rnd.Next(1, 6);
                IRQQApi.Api_ShutUP(C.w, G.major, QQ, a * 60);
                S.Major($"[IR:at={QQ}] 恭喜你，中了小型禁言套餐 {a} 分钟。继续加油！");
            }
            else
            {
                int a = rnd.Next(6, 11);
                IRQQApi.Api_ShutUP(C.w, G.major, QQ, a * 60);
                S.Major($"[IR:at={QQ}] 恭喜你，中了大型禁言套餐 {a} 分钟");
            }
        }
    }
}
