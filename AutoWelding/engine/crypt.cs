using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.InteropServices;

namespace AutoWelding.engine
{
    class Crypt
    {
        private static byte[] Keys = { 0xfc, 0xcc, 0x16, 0x24, 0x33, 0x56, 0x9D, 0x0f };

        /*******************************************************************
        * function:构造函数
        * input value:
        * output value:
        ********************************************************************/

        public Crypt()
        {
        }

        /*******************************************************************
        * function:字符串加密
        * input value:
        * output value:
        ********************************************************************/
        public string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey);//encryptKey.Substring(0, 8)
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch (Exception ee)
            {
                string err = ee.Message;
                return "";
            }
        }

        /*******************************************************************
            * function:字符串解密
            * input value:
            * output value:
            ********************************************************************/
        public string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);//decryptKey.Substring(0, 8)
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch (Exception ee)
            {
                string err = ee.Message;
                return "";
            }
        }        
    }
}
