using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServerTCP.Сryptographies
{
    internal class CryptoRsa
    {
        private RSACryptoServiceProvider m_Rsa;
        private RSAParameters m_ExternKey;
        private RSAParameters m_InternKey;

        public CryptoRsa()
        {
            m_Rsa = new RSACryptoServiceProvider(512);
            m_InternKey = m_Rsa.ExportParameters(true);

        }

    }
}