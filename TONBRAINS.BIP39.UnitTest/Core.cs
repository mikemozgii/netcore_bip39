using NUnit.Framework;
using TONBRAINS.BIP39.Core;

namespace TONBRAINS.BIP39.UnitTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {

            //var phrase = new Mnemonic().GenerateRandom();
            //var entropy = new Mnemonic().ToEntropy("legal winner thank year wave sausage worth useful legal winner thank yellow");

            //string mnemonic = "letter advice cage absurd amount doctor acoustic avoid letter advice cage above";
            //string seedHex = "d71de856f81a8acc65e6fc851a38d4d7ec216fd0796d0a6827a3ad6ed5511a30fa280f12eb2e47ed2ac03b5c462a0358d18d69fe4f985ec81778c1b370b652a8";
            //var seedResult = new Mnemonic().MnemonicToSeedHex(mnemonic, "TREZOR");
          var r =  new Mnemonic("abandon math mimic master filter design carbon crystal rookie group knife young", null);
            // Assert.AreEqual(seedHex, seedResult);
            Assert.Pass();
          
        }



        //[Test]
        //public void Test2()
        //{
        //    var entropy = "01000100010010011100010101001010100001101000100101101111000010101100100101100001110011000001100100100110100000110010000000010001";
        //    var r = entropy.ToHex();
        //    var hex = "4449C54A86896F0AC961CC1926832011";
        //    Assert.AreEqual(hex);
        //}
    }
}