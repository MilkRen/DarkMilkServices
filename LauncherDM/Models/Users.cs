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

        public string Password { get; set; }

        public string ImagePath { get; set; }

        private string Key = "fdjiaobzfddarkmilk";
        public Users() { }

        public Users(string login, string password, string imagePath)
        {
            Login = login;
            Password = password;
            ImagePath = imagePath;
        }

        public void cryptPassword()
        {
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(Password));
            var convertHash = Convert.ToHexString(hash);
            using (var myAes = Aes.Create())
            {
                byte[] aesKey = SHA256Managed.Create().ComputeHash(Encoding.UTF8.GetBytes(Key));
                byte[] aesIV = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(Key));
                myAes.Key = aesKey;
                myAes.IV = aesIV;
                var tokenResult = Convert.ToBase64String(CryptoAes.EncryptStringToBytes_Aes(convertHash, myAes.Key, myAes.IV));
                Password = tokenResult;
            }
        }
    }

    [Serializable]
    public class UsersList
    {
        public List<Users> UserList { get; set; } = new List<Users>();
    }
}
