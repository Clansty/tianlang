using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clansty.tianlang
{
    public class Unpack : IDisposable // 根据易语言 SDK 的 Unpack 类来写的
    {
        public Unpack(byte[] bin)
        {
            ms = new MemoryStream(bin);
            br = new BinaryReader(ms);
        }
        public Unpack(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            ms = new MemoryStream(bytes);
            br = new BinaryReader(ms);
        }

        private readonly BinaryReader br;
        private readonly MemoryStream ms;

        public void Dispose()
        {
            br.Dispose();
            ms.Dispose();
        }

        public int NextInt
        {
            get
            {
                byte[] buffer = br.ReadBytes(4);
                Array.Reverse(buffer);
                return BitConverter.ToInt32(buffer, 0);
            }
        }
        public long NextLong
        {
            get
            {
                byte[] buffer = br.ReadBytes(8);
                Array.Reverse(buffer);
                return BitConverter.ToInt64(buffer, 0);
            }
        }
        public short NextShort
        {
            get
            {
                byte[] buffer = br.ReadBytes(2);
                Array.Reverse(buffer);
                return BitConverter.ToInt16(buffer, 0);
            }
        }
        public string NextStr
        {
            get
            {
                short l = NextShort;
                byte[] buffer = br.ReadBytes(l);
                return Encoding.Default.GetString(buffer);
            }
        }
        public byte[] NextToken => br.ReadBytes(NextShort);

        public void Skip(short l) => br.ReadBytes(l);
    }
}
