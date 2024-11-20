using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace RExt.Encryption {
    public static class RijndaelEncryption {
        static int bufferKeySize = 32;
        static int blockSize = 256;
        static int keySize = 256;

        public static void UpdateEncryptionKeySize(int bufferKeySize = 32, int blockSize = 256, int keySize = 256) {
            RijndaelEncryption.bufferKeySize = bufferKeySize;
            RijndaelEncryption.blockSize = blockSize;
            RijndaelEncryption.keySize = keySize;
        }

        public static string Encrypt(string plane, string password) {
            var encrypted = Encrypt(Encoding.UTF8.GetBytes(plane), password);
            return Convert.ToBase64String(encrypted);
        }

        static byte[] Encrypt(byte[] src, string password) {
            var rij = SetupRijndaelManaged;

            // A pseudorandom number is newly generated based on the inputted password
            var deriveBytes = new Rfc2898DeriveBytes(password, bufferKeySize);
            // The missing parts are specified in advance to fill in 0 length
            var salt = new byte[bufferKeySize];
            // Rfc2898DeriveBytes gets an internally generated salt
            salt = deriveBytes.Salt;
            // The 32-byte data extracted from the generated pseudorandom number is used as a password
            var bufferKey = deriveBytes.GetBytes(bufferKeySize);

            rij.Key = bufferKey;
            rij.GenerateIV();

            using var encrypt = rij.CreateEncryptor(rij.Key, rij.IV);
            var dest = encrypt.TransformFinalBlock(src, 0, src.Length);
            // first 32 bytes of salt and second 32 bytes of IV for the first 64 bytes
            var compile = new List<byte>(salt);
            compile.AddRange(rij.IV);
            compile.AddRange(dest);
            return compile.ToArray();
        }

        public static string Decrypt(string encrypted, string password) {
            var decripted = Decrypt(Convert.FromBase64String(encrypted), password);
            return Encoding.UTF8.GetString(decripted);
        }

        static byte[] Decrypt(byte[] src, string password) {
            var rij = SetupRijndaelManaged;

            var compile = new List<byte>(src);

            // First 32 bytes are salt.
            var salt = compile.GetRange(0, bufferKeySize);
            // Second 32 bytes are IV.
            var iv = compile.GetRange(bufferKeySize, bufferKeySize);
            rij.IV = iv.ToArray();

            var deriveBytes = new Rfc2898DeriveBytes(password, salt.ToArray());
            var bufferKey = deriveBytes.GetBytes(bufferKeySize); // Convert 32 bytes of salt to password
            rij.Key = bufferKey;

            var plain = compile.GetRange(bufferKeySize * 2, compile.Count - (bufferKeySize * 2)).ToArray();

            using var decrypt = rij.CreateDecryptor(rij.Key, rij.IV);
            var dest = decrypt.TransformFinalBlock(plain, 0, plain.Length);
            return dest;
        }

        static RijndaelManaged SetupRijndaelManaged {
            get {
                var rij = new RijndaelManaged();
                rij.BlockSize = blockSize;
                rij.KeySize = keySize;
                rij.Mode = CipherMode.CBC;
                rij.Padding = PaddingMode.PKCS7;
                return rij;
            }
        }
    }
}