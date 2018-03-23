////
// 指令加密压缩传送
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Compression;//压缩空间
using System.Security.Cryptography;//加密空间
using System.IO;
namespace QcNet
{
   public class QcCoder:Coder
    {
        private static byte[] DESKey = new byte[] { 18, 10, 04, 28, 10, 22, 19, 86 };

        //获取或设置对称算法的初始化向量
        private static byte[] DESIV = new byte[] { 20, 10, 35, 255, 13, 24, 21, 22 };

        /// <summary>
        ///  des加密
        /// </summary>
        /// <param name="EncryptByteArray"></param>
        /// <returns></returns>
        private static byte[] EncryptDES(byte[] EncryptByteArray)
        {
            try
            {
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(DESKey, DESIV), CryptoStreamMode.Write);
                cStream.Write(EncryptByteArray, 0, EncryptByteArray.Length);
                cStream.FlushFinalBlock();
                return mStream.ToArray();
            }
            catch
            {
                return EncryptByteArray;
            }
        }

        /// <summary>
        ///  des解密
        /// </summary>
        /// <param name="EncryptByteArray"></param>
        /// <returns></returns>
        private static byte[] DecryptDES(byte[] EncryptByteArray)
        {
            try
            {
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateDecryptor(DESKey, DESIV), CryptoStreamMode.Write);
                cStream.Write(EncryptByteArray, 0, EncryptByteArray.Length);
                cStream.FlushFinalBlock();
                return mStream.ToArray();
            }
            catch
            {
                return EncryptByteArray;
            }
        }
        /// <summary>
        ///  压缩解压
        /// </summary>
        /// <param name="data">待处理数据</param>
        /// <param name="mode">压缩和解压</param>
        /// <returns></returns>
        private  byte[] Compression(byte[] data, CompressionMode mode)
        {
            DeflateStream zip = null;
            try
            {
                if (mode == CompressionMode.Compress)
                {
                    MemoryStream ms = new MemoryStream();
                    zip = new DeflateStream(ms, mode, true);
                    zip.Write(data, 0, data.Length);
                    zip.Close();
                    return ms.ToArray();
                }
                else
                {
                    MemoryStream ms = new MemoryStream();
                    ms.Write(data, 0, data.Length);
                    ms.Flush();
                    ms.Position = 0;
                    zip = new DeflateStream(ms, mode, true);
                    MemoryStream os = new MemoryStream();
                    int SIZE = 1024;
                    byte[] buf = new byte[SIZE];
                    int l = 0;
                    do
                    {
                        l = zip.Read(buf, 0, SIZE);
                        if (l == 0) l = zip.Read(buf, 0, SIZE);
                        os.Write(buf, 0, l);
                    }
                    while (l != 0);
                    zip.Close();
                    return os.ToArray();
                }
            }
            catch
            {
                if (zip != null) zip.Close();
                return null;
            }
            finally
            {
                if (zip != null) zip.Close();
            }
        }
        //需要发送的指令加密为二进制串
        public override byte[] GetEncodingBytes(string datagram)
        {

            return EncryptDES(Compression(base.GetEncodingBytes(datagram), CompressionMode.Compress));
        }
        //接收到的数据还原字符串指令
        public override string GetEncodingString(byte[] dataBytes, int size)
        {
            try
            {
                if (size == 0) size = dataBytes.Length;                    
                byte[] data = new byte[size];
                System.Array.Copy(dataBytes, data, size);
                data = Compression(DecryptDES(data), CompressionMode.Decompress);
                return base.GetEncodingString(data, data.Length);
            }
            catch
            {
                return null;
            }
        }
    }
}
