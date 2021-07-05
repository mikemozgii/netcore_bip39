using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TONBRAINS.BIP39.Core
{
    public class Crypto
    {
        public string Checksum(byte[] bytea, WordCount mnemonicType)
        {
            var h = new SHA256CryptoServiceProvider().ComputeHash(bytea);

            int cs;
            switch (mnemonicType)
            {
                case WordCount.Twelve:
                    cs = 4; // (128*8)/32 = 16
                    break;
                case WordCount.Fifteen:
                    cs = 5; // (160/8)/32 = 20
                    break;
                case WordCount.Eighteen:
                    cs = 6; // (192/8)/32 = 24
                    break;
                case WordCount.TwentyOne:
                    cs = 7; // (224/8)/32 = 28
                    break;
                case WordCount.TwentyFour:
                    cs = 8; // (256/8)/32 = 32
                    break;
                default:
                    return null;
            }


            //We then take 1 bit of that hash for every 32 bits of entropy, and add it to the end of our entropy
            string result = h.ToBinaryString();
            return result.Substring(0, cs);
        }
    }
}
