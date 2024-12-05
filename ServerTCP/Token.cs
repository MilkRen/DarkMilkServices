using System.Security.Cryptography;
using System.Text;
using ServerTCP.Сryptographies;

namespace ServerTCP
{
    public class Token
    {
        private static string Key = "sdkjoidarkmilk=";

        public static Dictionary<string, string> Tokens = new Dictionary<string, string>();

        public static string GenerateToken(string login, string password)
        {
            //var tokenData = string.Concat(login, ",", password);
            var regToken = string.Concat(login, ",", DateTime.Now);
            using (var myAes = Aes.Create())
            {
                byte[] aesKey = SHA256Managed.Create().ComputeHash(Encoding.UTF8.GetBytes(Key));
                byte[] aesIV = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(Key));
                myAes.Key = aesKey;
                myAes.IV = aesIV;
                var tokenResult = Convert.ToBase64String(CryptoAes.EncryptStringToBytes_Aes(regToken, myAes.Key, myAes.IV));

                if(!Tokens.ContainsKey(tokenResult))
                    Tokens.Add(tokenResult, regToken);
                return tokenResult;
            }
        }
    }
}
