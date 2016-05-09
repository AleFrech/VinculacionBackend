using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using VinculacionBackend.Data.Interfaces;

namespace VinculacionBackend.Data
{
    public  class Encryption:IEncryption
    {
        public string Encrypt(string stringValue)
        {
            byte[] key = { };
            byte[] IV = { 0x32, 0x41, 0x54, 0x67, 0x73, 0x21, 0x47, 0x19 };
            MemoryStream ms = null;

            try
            {
                string encryptionKey = "bd5ygNc8";
                key = Encoding.UTF8.GetBytes(encryptionKey);
                byte[] bytes = Encoding.UTF8.GetBytes(stringValue);
                DESCryptoServiceProvider dcp = new DESCryptoServiceProvider();
                ICryptoTransform ict = dcp.CreateEncryptor(key, IV);
                ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, ict, CryptoStreamMode.Write);
                cs.Write(bytes, 0, bytes.Length);
                cs.FlushFinalBlock();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        public string Decrypt(string stringValue)
        {
            byte[] key = { };
            byte[] IV = { 0x32, 0x41, 0x54, 0x67, 0x73, 0x21, 0x47, 0x19 };
            MemoryStream ms = null;

            try
            {
                string encryptionKey = "bd5ygNc8";
                key = Encoding.UTF8.GetBytes(encryptionKey);
                byte[] bytes = new byte[stringValue.Length];
                bytes = Convert.FromBase64String(stringValue);
                DESCryptoServiceProvider dcp = new DESCryptoServiceProvider();
                ICryptoTransform ict = dcp.CreateDecryptor(key, IV);
                ms = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(ms, ict, CryptoStreamMode.Write);
                cryptoStream.Write(bytes, 0, bytes.Length);
                cryptoStream.FlushFinalBlock();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Encoding en = Encoding.UTF8;
            return en.GetString(ms.ToArray());
        }
    }
}