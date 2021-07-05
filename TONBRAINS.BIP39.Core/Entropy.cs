using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TONBRAINS.BIP39.Core
{
    public class Entropy
    {
       // public static int _bits = 8;
        public byte[] GenerateEntropy(WordCount mnemonicType)
        {

            int bytesCount;
            switch (mnemonicType)
            {
                case WordCount.Twelve:
                    bytesCount = 16; // 128/8 = 16
                    break;
                case WordCount.Fifteen:
                    bytesCount = 20; // 160/8 = 20
                    break;
                case WordCount.Eighteen:
                    bytesCount = 24; // 192/8 = 24
                    break;
                case WordCount.TwentyOne:
                    bytesCount = 28; // 224/8 = 28
                    break;
                case WordCount.TwentyFour:
                    bytesCount = 32; // 256/8 = 32
                    break;
                default:
                    return Array.Empty<byte>();
            }

            var bytea = new byte[bytesCount];
            new RNGCryptoServiceProvider().GetBytes(bytea);
            return bytea;
        }
    }
}
