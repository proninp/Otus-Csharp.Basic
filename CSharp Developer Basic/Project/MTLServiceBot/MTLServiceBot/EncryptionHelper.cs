using System.Security.Cryptography;
using System.Text;

namespace MTLServiceBot
{
    public static class EncryptionHelper
    {
        private const int DerivationIterations = 1000;

        public static string Encrypt(string message)
        {
            CheckEncryptionData();
            byte[] clearBytes = Encoding.Unicode.GetBytes(message);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(
                    AppConfig.EncryptionKey,
                    Encoding.ASCII.GetBytes(AppConfig.EncryptionSalt),
                    DerivationIterations,
                    HashAlgorithmName.SHA1);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    message = Convert.ToBase64String(ms.ToArray());
                }
            }
            return message;
        }
        public static string Decrypt(string cipherText)
        {
            CheckEncryptionData();
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(
                    AppConfig.EncryptionKey,
                    Encoding.ASCII.GetBytes(AppConfig.EncryptionSalt),
                    DerivationIterations,
                    HashAlgorithmName.SHA1);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        private static void CheckEncryptionData()
        {
            if (string.IsNullOrEmpty(AppConfig.EncryptionKey))
                throw new ArgumentException("Необходимо указать параметр ключа шифрования!", "encryptionKey");
            if (string.IsNullOrEmpty(AppConfig.EncryptionSalt))
                throw new ArgumentException("Необходимо указать параметр модификатора входа хэш-функции шифрования!", "encryptionSalt");
        }
    }
}
