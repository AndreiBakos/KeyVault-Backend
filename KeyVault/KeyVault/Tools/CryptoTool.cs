using System;
using System.Security.Cryptography;
using System.Text;

namespace KeyVault.Tools
{
    public class CryptoTool
    {
        public string Encrypt(string message, Guid key, Guid iv)
        {
            using (var aes = Aes.Create())
            {
                // Set the key and initialization vector
                aes.Key = key.ToByteArray();
                aes.IV = iv.ToByteArray();

                // Create a encryptor
                var encryptor = aes.CreateEncryptor();

                // Encrypt the message
                byte[] plainBytes = Encoding.UTF8.GetBytes(message);
                byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                return Convert.ToBase64String(cipherBytes);
            }
        }

        public string Decrypt(string hiddenMessage, Guid key, Guid iv)
        {
            using (Aes aes = Aes.Create())
            {
                // Set the key and initialization vector
                aes.Key = key.ToByteArray();
                aes.IV = iv.ToByteArray();

                // Create a decryptor
                var decryptor = aes.CreateDecryptor();

                // Decrypt the message
                byte[] plainBytes = decryptor.TransformFinalBlock(Convert.FromBase64String(hiddenMessage), 0, Convert.FromBase64String(hiddenMessage).Length);
                string originalMessage = Encoding.UTF8.GetString(plainBytes);

                return originalMessage;
            }
        }
    }
}