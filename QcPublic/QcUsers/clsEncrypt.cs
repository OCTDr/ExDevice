using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace QcPublic
{
    public class QcEncrypt
    {

        public static string Md5Hash(string strPwd)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.Default.GetBytes(strPwd);
            byte[] md5data = md5.ComputeHash(data);
            md5.Clear();
            string str = "";
            for (int i = 0; i < md5data.Length; i++)
            {
                str += md5data[i].ToString("x").PadLeft(2, '0');
            }
            return str;
        }
            //默认密钥向量
            private static byte[] Keys = { 0x18, 0x36, 0x26, 0x79, 0x3A, 0xA2, 0xC3, 0x0F };          
            /// <summary>
            /// DES加密字符串
            /// </summary>
            /// <param name="encryptString">待加密的字符串</param>
            /// <param name="encryptKey">加密密钥,要求为8位</param>
            /// <returns>加密成功返回加密后的字符串,失败返回源串</returns>
            public static string Encode(string encryptString,bool isstatic=false)
            {
                Random r=new Random();
                encryptString += (r.Next() % 1000 + 1000).ToString();
                string encryptKey = GetMachineCode();
                encryptKey = encryptKey.PadRight(8, ' ');
                byte[] rgbKey;
                if (isstatic) rgbKey = Keys;
                else rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());

            }
            private static string GetMachineCode()
            {
                return Md5Hash(Environment.MachineName+Environment.OSVersion+Environment.UserName).Substring(12,8);
            }
            /// <summary>
            /// DES解密字符串
            /// </summary>
            /// <param name="decryptString">待解密的字符串</param>
            /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
            /// <returns>解密成功返回解密后的字符串,失败返源串</returns>
            public static string Decode(string decryptString, bool isstatic=false)
            {
                try
                {
                    string decryptKey=GetMachineCode();

                    decryptKey = decryptKey.PadRight(8, ' ');
                    byte[] rgbKey;
                    if (isstatic) rgbKey = Keys;                 
                    else   rgbKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
                  
                    byte[] rgbIV = Keys;
                    byte[] inputByteArray = Convert.FromBase64String(decryptString);
                    DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();

                    MemoryStream mStream = new MemoryStream();
                    CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                    cStream.Write(inputByteArray, 0, inputByteArray.Length);
                    cStream.FlushFinalBlock();
                    string str= Encoding.UTF8.GetString(mStream.ToArray());
                    return str.Substring(0, str.Length - 4);
                }
                catch
                {
                    return "";
                }
            }           

        
    }
}
