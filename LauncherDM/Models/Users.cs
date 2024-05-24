using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using ServerTCP.Сryptographies;

namespace LauncherDM.Models
{
    [Serializable]
    public class Users
    {
        public string Login { get; set; }

        public byte[] PasswordArray { get; set; }

        public string ImagePath { get; set; }

        private string Key = "fdjiaobzfddarkmilk";
        public Users() { }

        public Users(string login, string imagePath, byte[] passwordArray = null)
        {
            Login = login;
            PasswordArray = passwordArray;
            ImagePath = imagePath;
        }

        public byte[] CryptPassword(string password)
        {
            //var hash = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            //var convertHash = Convert.ToHexString(hash);
            using (var myAes = Aes.Create())
            {
                byte[] aesKey = SHA256Managed.Create().ComputeHash(Encoding.UTF8.GetBytes(Key));
                byte[] aesIV = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(Key));
                myAes.Key = aesKey;
                myAes.IV = aesIV;
                PasswordArray = CryptoAes.EncryptStringToBytes_Aes(password, myAes.Key, myAes.IV);
                //Password = Convert.ToBase64String(PasswordArray);
            }

            return PasswordArray;
        }

        public string DecryptPassword()
        {
            using (var myAes = Aes.Create())
            {
                byte[] aesKey = SHA256Managed.Create().ComputeHash(Encoding.UTF8.GetBytes(Key));
                byte[] aesIV = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(Key));
                myAes.Key = aesKey;
                myAes.IV = aesIV;
                return CryptoAes.DecryptStringFromBytes_Aes(PasswordArray, myAes.Key, myAes.IV);
            }
        }
    }

    [Serializable]
    public class UsersForXml
    {
        public List<Users> UserList { get; set; } = new List<Users>();
    }
}
