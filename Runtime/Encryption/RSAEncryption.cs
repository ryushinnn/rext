using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace RExt.Encryption {
    public static class RSAEncryption {
        public static KeyValuePair<string, string> GenrateKeyPair(int keySize) {
            var rsa = new RSACryptoServiceProvider(keySize);
            string publicKey = rsa.ToXmlString(false);
            string privateKey = rsa.ToXmlString(true);
            return new KeyValuePair<string, string>(publicKey, privateKey);
        }

        public static string Encrypt(string plane, string publicKey) {
            byte[] encrypted = Encrypt(Encoding.UTF8.GetBytes(plane), publicKey);
            return Convert.ToBase64String(encrypted);
        }

        public static byte[] Encrypt(byte[] src, string publicKey) {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider()) {
                rsa.FromXmlString(publicKey);
                byte[] encrypted = rsa.Encrypt(src, false);
                return encrypted;
            }
        }

        public static string Decrypt(string encrtpted, string privateKey) {
            byte[] decripted = Decrypt(Convert.FromBase64String(encrtpted), privateKey);
            return Encoding.UTF8.GetString(decripted);
        }

        public static byte[] Decrypt(byte[] src, string privateKey) {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider()) {
                rsa.FromXmlString(privateKey);
                byte[] decrypted = rsa.Decrypt(src, false);
                return decrypted;
            }
        }
    }
}