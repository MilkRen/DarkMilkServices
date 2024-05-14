using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ServerTCP.Сryptographies
{
    public class CryptoRsa
    {
        public CryptoRsa()
        {
     
        }

        /// <summary>
        /// (publicKey, privateKey)
        /// </summary>
        /// <returns></returns>
        public static (string, string) GenerateKey()
        {
            using (var rsa = RSA.Create())
            {
                var publicKey = rsa.ToXmlString(false);
                var privateKey = rsa.ToXmlString(false);
                return (publicKey, privateKey);
            }
        }

        public static byte[] Encrypt(string publiKey, string data)
        {
            using (var rsa = RSA.Create())
            {
                rsa.FromXmlString(publiKey);
                byte[] messageBytes = Encoding.UTF8.GetBytes(data);
                byte[] encryptedBytes = rsa.Encrypt(messageBytes, RSAEncryptionPadding.OaepSHA256);
                return encryptedBytes;
            }
        }

        public static string Decrypt(string privateKey, byte[] encryptedBytes)
        {
            using (var rsa = RSA.Create())
            {
                rsa.FromXmlString(privateKey);
                byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.OaepSHA256);
                var decryptedMessage = Encoding.UTF8.GetString(decryptedBytes);
                return decryptedMessage;
            }
        }
    }
}