using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TONBRAINS.BIP39.Core
{
    public class Mnemonic
    {

        public string _mnemonicRaw { get; set; }
        public string _entropy { get; set; }
        public string _key { get; set; }
        public string _password { get; set; }
        public Languages _language { get; set; }
        public string[] _mnemonicWords { get; set; }
        public Dictionary<string, int> _mnemonicWordIndices { get; set; }
        public string[] _wordDictionary { get; set; }
        public bool _isValid { get; set; }
        public WordCount _wordCount { get; set; }
        public Mnemonic(string password = null, WordCount mnemonicType = WordCount.Twelve, Languages language = Languages.English)
        {
            var mnemonic =  GenerateNew(mnemonicType, language);
            _password = password;
            Init(mnemonic, language);
        }

        public Mnemonic(string mnemonic, string password = null, Languages lang = Languages.English)
        {
            _password = password;
            Init(mnemonic, lang);
        }

        public void Init(string mnemonic, Languages lang = Languages.English)
        {


            if (!string.IsNullOrWhiteSpace(_password))
                _password = _password.Normalize(NormalizationForm.FormKD);
            if (string.IsNullOrWhiteSpace(mnemonic)) throw new ArgumentException("Error: Invalid Mnemonic Phrase", "mnemonic");
            _mnemonicRaw = mnemonic.Trim().ToLower().Normalize(NormalizationForm.FormKD);
            _language = lang;
            _mnemonicWords = Regex.Split(_mnemonicRaw, @"\s+").Where(s => s != string.Empty).ToArray();

            if (_mnemonicWords.Count() % 3 != 0)
            {
                throw new ArgumentException("Error: Invalid Mnemonic Phrase", "mnemonic");
            }

            switch (_mnemonicWords.Count())
            {
                case 12:
                    _wordCount = WordCount.Twelve;
                    break;
                case 15:
                    _wordCount = WordCount.Fifteen;
                    break;
                case 18:
                    _wordCount = WordCount.Eighteen;
                    break;
                case 21:
                    _wordCount = WordCount.TwentyOne;
                    break;
                case 24:
                    _wordCount = WordCount.TwentyFour;
                    break;
                default:
                    throw new ArgumentException("Error: Invalid Word Count", "mnemonic");
            }

            _wordDictionary = WordDicitionaries.GetWordDictionary(lang).ToArray();

            if (!_mnemonicWords.All(q => _wordDictionary.Contains(q)))
            {
                throw new ArgumentException("Error: Invalid Words", "mnemonic");
            }

            ToIndices();

            _entropy = GetEntropy();

            _isValid = true;

            GetKey();
        }

        public string GetEntropy()
        {
            var finalBits = string.Join("", _mnemonicWords.Select(q =>Convert.ToString(Array.IndexOf(_wordDictionary, q), 2).PadLeft(11, '0')));
            var deapth = (int)Math.Floor((double)finalBits.Length / 33) * 32;
            var entropy = finalBits.Substring(0, deapth);
            var checksum = finalBits.Substring(deapth);

            var entropyBytesMatch = Regex.Matches(entropy, "(.{1,8})").OfType<Match>().Select(m => m.Groups[0].Value).ToArray();

            var entropyBytes = entropyBytesMatch.Select(bytes => Convert.ToByte(bytes, 2)).ToArray();

            if (new Crypto().Checksum(entropyBytes, _wordCount) != checksum)
                throw new Exception("Error: Invalid Entropy");

            return entropyBytes.ToHex();
        }

        public string GenerateNew(WordCount mnemonicType = WordCount.Twelve, Languages lang = Languages.English)
        {
            var entropy = new Entropy().GenerateEntropy(mnemonicType);
            var checksum_binary_string = new Crypto().Checksum(entropy, mnemonicType);
            var binary_string = entropy.ToBinaryString();

            var finalBits = $"{binary_string}{checksum_binary_string}";
            var wordDictionary = WordDicitionaries.GetWordDictionary(lang).ToArray();
            var split = Regex.Matches(finalBits, "(.{1,11})").OfType<Match>().Select(m => m.Groups[0].Value).ToArray();
            var finalWords = split.Select(binary =>
            {
                return wordDictionary[Convert.ToInt32(binary, 2)];
            });
            return string.Join(lang == Languages.Japanese ? "\u3000" : " ", finalWords);
        }

        private void GetKey()
        {
            var mnemonicBytes = _mnemonicRaw.GetBytes();
            var saltBytes = $"mnemonic{_password}".GetBytes();
            _key = new Rfc2898DeriveBytes(mnemonicBytes, saltBytes, 2048, HashAlgorithmName.SHA512).GetBytes(64).ToHex();
        }

        public void ToIndices()
        {
            _mnemonicWordIndices = new Dictionary<string, int>();
            foreach (var mnemonicWord in _mnemonicWords)
            {
                var wordIndex = GetWordIndex(mnemonicWord);
                if (wordIndex == -1)
                {
                    throw new Exception($"{mnemonicWord} is not in the Dictionary");
                }
                _mnemonicWordIndices.Add(mnemonicWord, wordIndex);
            }
        }

        public int GetWordIndex(string word)
        {
            var index = -1;
            if (_mnemonicWords.Contains(word))
            {
                return Array.IndexOf(_mnemonicWords, word);
            }
            return index;
        }

    }
}
