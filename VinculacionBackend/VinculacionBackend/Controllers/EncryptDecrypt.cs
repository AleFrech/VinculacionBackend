using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace VinculacionBackend.Controllers
{
    public static class EncryptDecrypt
    {
        public static string Encrypt(string StringValue)
        {
            byte[] key = { };
            byte[] IV = { 0x32, 0x41, 0x54, 0x67, 0x73, 0x21, 0x47, 0x19 };
            MemoryStream ms = null;

            try
            {
                string encryptionKey = "bd5ygNc8";
                key = Encoding.UTF8.GetBytes(encryptionKey);
                byte[] bytes = Encoding.UTF8.GetBytes(StringValue);
                DESCryptoServiceProvider dcp = new DESCryptoServiceProvider();
                ICryptoTransform ict = dcp.CreateEncryptor(key, IV);
                ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, ict, CryptoStreamMode.Write);
                cs.Write(bytes, 0, bytes.Length);
                cs.FlushFinalBlock();
            }
            catch (Exception ex)
            {
                
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Decrypt(string StringValue)
        {
            byte[] key = { };
            byte[] IV = { 0x32, 0x41, 0x54, 0x67, 0x73, 0x21, 0x47, 0x19 };
            MemoryStream ms = null;

            try
            {
                string encryptionKey = "bd5ygNc8";
                key = Encoding.UTF8.GetBytes(encryptionKey);
                byte[] bytes = new byte[StringValue.Length];
                bytes = Convert.FromBase64String(StringValue);
                DESCryptoServiceProvider dcp = new DESCryptoServiceProvider();
                ICryptoTransform ict = dcp.CreateDecryptor(key, IV);
                ms = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(ms, ict, CryptoStreamMode.Write);
                cryptoStream.Write(bytes, 0, bytes.Length);
                cryptoStream.FlushFinalBlock();
            }
            catch (Exception ex)
            {
                
            }
            Encoding en = Encoding.UTF8;
            return en.GetString(ms.ToArray());
        }
    }
}