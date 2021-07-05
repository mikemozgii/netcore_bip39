using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TONBRAINS.BIP39.Core
{
    public static class ByteaExt
    {
        public static string ToHex(this byte[] bytea)
        {
            return BitConverter.ToString(bytea).Replace("-", "");
        }

        public static string ToBinaryString(this byte[] bytea)
        {
            return string.Join("", bytea.Select(x => Convert.ToString(x, 2).PadLeft(8, '0')));
        }

        public static byte[]  GetBytes(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }


        
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

    }
}
