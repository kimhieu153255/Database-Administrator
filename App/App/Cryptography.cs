using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;



namespace App
{
    internal class RSA
    {
        private RSACryptoServiceProvider rsa;
        public const bool PRIVATE = true;
        public const bool PUBLIC = false;

        public RSA(int keySize)
        {
            rsa = new RSACryptoServiceProvider(keySize);
        }

        public string GetPublicKey()
        {
            RSAParameters publicKey = rsa.ExportParameters(PUBLIC);
            string publicKeyXml = rsa.ToXmlString(PUBLIC);
            return publicKeyXml;
        }

        public void ExportPrivateKeyToFile(string fileName)
        {
            RSAParameters privateKey = rsa.ExportParameters(PRIVATE);
            string privateKeyXml = rsa.ToXmlString(PRIVATE);
            File.WriteAllText(fileName, privateKeyXml);
        }

        public int ImportPrivateKeyFromFile(string fileName)
        {
            if(File.Exists(fileName)){
                string privateKeyXml = File.ReadAllText(fileName);
                rsa.FromXmlString(privateKeyXml);
                return 1;
            }
            return 0;
        }

        public byte[] Encrypt(string plaintext)
        {
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
            RSAParameters publicKey = rsa.ExportParameters(PUBLIC);
            rsa.ImportParameters(publicKey);
            byte[] ciphertext = rsa.Encrypt(plaintextBytes, true);
            return ciphertext;
        }

        public string Decrypt(byte[] ciphertext)
        {
            byte[] decrypted;
            try
            {
                //RSAParameters privateKey = rsa.ExportParameters(PRIVATE);
                //rsa.ImportParameters(privateKey);
                decrypted = rsa.Decrypt(ciphertext, true);
            }
            catch (CryptographicException)
            {
                throw new Exception("Unable to decrypt message. Invalid private key.");
            }
            return Encoding.UTF8.GetString(decrypted);
        }

    }
}
