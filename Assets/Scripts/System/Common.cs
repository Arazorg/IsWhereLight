using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class Common
{
    /// <summary>
    /// Инструкция от Ары для себя самого
    /// AES (Advanced Encryption Standard) 128 бит ключ(дефолт)
    /// 128 бит AES, NIST, но не 256-bit AES
    /// 256 бит AES медленнее на 40% чем 128-bit AES
    /// Защита даты, но не защита опертивки(взломают, плачу)
    /// </summary>
    public static class AES
    {
        public static int KeyLength = 128;
        private const string SaltKey = "ShMG8hLyZ7k~Ge5@";
        private const string VIKey = "~6YUi0Sv5@|{aOZO";

        public static string Encrypt(byte[] value, string password)
        {
            var keyBytes = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(SaltKey)).GetBytes(KeyLength / 8);
            var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.UTF8.GetBytes(VIKey));

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(value, 0, value.Length);
                    cryptoStream.FlushFinalBlock();
                    cryptoStream.Close();
                    memoryStream.Close();

                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }

        public static string Encrypt(string value, string password)
        {
            return Encrypt(Encoding.UTF8.GetBytes(value), password);
        }

        public static string Decrypt(string value, string password)
        {
            var cipherTextBytes = Convert.FromBase64String(value);
            var keyBytes = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(SaltKey)).GetBytes(KeyLength / 8);
            var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC, Padding = PaddingMode.None };
            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.UTF8.GetBytes(VIKey));

            using (var memoryStream = new MemoryStream(cipherTextBytes))
            {
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    var plainTextBytes = new byte[cipherTextBytes.Length];
                    var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                    memoryStream.Close();
                    cryptoStream.Close();

                    return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
                }
            }
        }
    }

    public class B64X
    {
        public static byte[] Key = Guid.NewGuid().ToByteArray();

        public static string Encode(string value)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        }

        public static string Decode(string value)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(value));
        }

        public static string Encrypt(string value, string key)
        {
            return Convert.ToBase64String(Encode(Encoding.UTF8.GetBytes(value), Encoding.UTF8.GetBytes(key)));
        }

        public static string Decrypt(string value, string key)
        {
            return Encoding.UTF8.GetString(Encode(Convert.FromBase64String(value), Encoding.UTF8.GetBytes(key)));
        }

        private static byte[] Encode(byte[] bytes, byte[] key)
        {
            var j = 0;

            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] ^= key[j];

                if (++j == key.Length)
                {
                    j = 0;
                }
            }

            return bytes;
        }
    }

}
