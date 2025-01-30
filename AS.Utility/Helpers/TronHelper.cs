using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AS.Utility.Helpers
{
    public static class TronHelper
    {
        private const string ALPHABET = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
        private const int BASE = 58;

        public static string ToBase58(this string hex)
        {
            var bytes = HexStrToByteArray(hex);
            var base58 = GetBase58CheckAddress(bytes);
            return base58;
        }
        private static byte[] HexStrToByteArray(string hex)
        {
            var byteArray = new List<byte>();
            int d = 0, j = 0;

            foreach (char c in hex)
            {
                if (IsHexChar(c))
                {
                    d = (d << 4) + HexCharToByte(c);
                    j++;
                    if (j % 2 == 0)
                    {
                        byteArray.Add((byte)d);
                        d = 0;
                    }
                }
            }

            return byteArray.ToArray();
        }

        private static string GetBase58CheckAddress(byte[] addressBytes)
        {
            var hash0 = SHA256(addressBytes);
            var hash1 = SHA256(hash0);
            var checkSum = hash1.Take(4).ToArray();
            var fullBytes = addressBytes.Concat(checkSum).ToArray();
            return Encode58(fullBytes);
        }

        private static byte[] SHA256(byte[] input)
        {
            using (var sha256 = new SHA256Managed())
            {
                return sha256.ComputeHash(input);
            }
        }

        private static string Encode58(byte[] buffer)
        {
            if (buffer.Length == 0) return string.Empty;

            var digits = new List<int> { 0 };
            foreach (byte b in buffer)
            {
                for (int i = 0; i < digits.Count; i++)
                    digits[i] <<= 8;

                digits[0] += b;
                int carry = 0;

                for (int i = 0; i < digits.Count; i++)
                {
                    digits[i] += carry;
                    carry = digits[i] / BASE;
                    digits[i] %= BASE;
                }

                while (carry > 0)
                {
                    digits.Add(carry % BASE);
                    carry /= BASE;
                }
            }

            // Handle leading zeros
            for (int i = 0; i < buffer.Length && buffer[i] == 0; i++)
                digits.Add(0);

            digits.Reverse();
            return string.Join(string.Empty, digits.Select(d => ALPHABET[d]));
        }

        private static bool IsHexChar(char c)
        {
            return (c >= '0' && c <= '9') ||
                   (c >= 'A' && c <= 'F') ||
                   (c >= 'a' && c <= 'f');
        }

        private static int HexCharToByte(char c)
        {
            if (c >= '0' && c <= '9') return c - '0';
            if (c >= 'A' && c <= 'F') return c - 'A' + 10;
            if (c >= 'a' && c <= 'f') return c - 'a' + 10;
            throw new ArgumentException($"Invalid hex character: {c}");
        }
    }
}
