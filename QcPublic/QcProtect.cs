using System;
using System.Collections.Generic;
using System.Security.Cryptography;//加密空间
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Xml;
using System.Security.Cryptography.Xml;
using System.IO;
using QcNet;
namespace QcPublic
{
    public class QcProtect
    {
        bool cksn;
        public QcProtect(bool chksn=true )
        {
            cksn = chksn;
        }
        public string GetSn()
        {
            string s = GetHdw();
            var sn = GetHash32(QcEncrypt.Md5Hash(s) + "igces").Substring(0, 26);
            return sn;
           
        }
        public string GetHdw()
        {
            try
            {
                QcHardWare h = new QcHardWare();
                string cpuid = h.GetCpuID();
                string diskid = h.GetDiskID();
                //string macid=h.GetMacAddress();
                if (cpuid != "" && diskid != "")
                {
                    return cpuid + "igeces" + diskid;
                }
                else if (cpuid != "")
                {
                    return "CPU"+cpuid + "igces" + (System.Environment.MachineName).Trim();
                }
                else if (diskid != "")
                {
                    return "DISK"+diskid + "igces" + (System.Environment.MachineName).Trim();
                }
                else
                {
                    return "igces" + (System.Environment.MachineName).Trim();
                }
            }
            catch
            {
                return "igces" + (System.Environment.MachineName).Trim();
            }
            
        }

       private bool CheckCode(string code)
        {

            return true;
        }
        public void MakeKeyFile(string keyfile,string 单位名称="")
        {
            XDocument doc = new XDocument();
            XElement root = new XElement("IGCESLicense");
            doc.Add(root);
            QcHardWare hd=new QcHardWare();
            string sn = this.GetSn();
            if (sn == null)
                return;
            root.Add(new XElement("序列号",this.GetSn()));
            root.Add(new XElement("单位名称", 单位名称));
            root.Add(new XElement("cpu",hd.GetCpuID()));
            root.Add(new XElement("disk",hd.GetDiskID()));
            root.Add(new XElement("hdw",this.GetHdw()));
            //root.Add(new XElement("mac",hd.GetMacAddress()));
            //hd.GetSystemInfo(root);
             doc.Save(keyfile);
        }
       
        public void Makelicense(string pritivefile,string keyfile,string licensefile)
        {
            
           string pubKey = ReadPublicKey(pritivefile);
            RSACryptoServiceProvider Key = new RSACryptoServiceProvider();
            Key.FromXmlString(pubKey);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(keyfile);
            var pub=xmlDoc.CreateElement("PublicKey");
            pub.InnerXml= Key.ToXmlString(false);    
            xmlDoc.SelectSingleNode("IGCESLicense").AppendChild(pub);
            //xmlDoc.Save(licensefile);
            SignedXml signedXml = new SignedXml(xmlDoc);
            signedXml.SigningKey = Key;
            Reference reference = new Reference();
            reference.Uri = "";
            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);
            signedXml.AddReference(reference);
            signedXml.ComputeSignature();
            XmlElement xmlDigitalSignature = signedXml.GetXml();
            xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));
            xmlDoc.Save(licensefile);
            
        }
        public bool CheckLicenseFile(string licensefile)
        {
            if (System.IO.File.Exists(licensefile) == false ) return false;
            return CheckLicense(System.IO.File.ReadAllText(licensefile, System.Text.Encoding.Default));
        }
        public bool CheckLicense(string licenseXml)
        {            
            SignedXml xml = new SignedXml();           
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(licenseXml);
            string pubKey = xmlDoc.SelectSingleNode("IGCESLicense").SelectSingleNode("PublicKey").InnerXml;
            RSACryptoServiceProvider Key = new RSACryptoServiceProvider();
            Key.FromXmlString(pubKey);

            SignedXml signedXml = new SignedXml(xmlDoc);
            XmlNodeList nodeList = xmlDoc.GetElementsByTagName("Signature");
            signedXml.LoadXml((XmlElement)nodeList[0]);
            bool signature_ok= signedXml.CheckSignature(Key);
            if (signature_ok)
            {
                string sn=xmlDoc.GetElementsByTagName("序列号")[0].InnerText;
                if (cksn)
                    return (GetSn() == sn);
                else
                    return true;
                //string cpu = xmlDoc.GetElementsByTagName("cpu")[0].InnerText ;
                //string mac = xmlDoc.GetElementsByTagName("mac")[0].InnerText ;
                //string disk = xmlDoc.GetElementsByTagName("disk")[0].InnerText ;
                //QcHardWare h=new QcHardWare();
                
                //if (cpu.Contains(h.GetCpuID()) == false) return false;
                //var lstdisk = h.GetDiskID().Split(';');
                //if (lstdisk.Any(t => disk.Contains(t) == false)) return false;
                //var lstmac = h.GetMacAddress().Split(';');
                //if (lstmac.Any(t => mac.Contains(t) == false)) return false;
                //return true;
            }
            return false;            
        }

        public void RSAKey(string PrivateKeyPath, string PublicKeyPath)
        {
            try
            {
                RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
                this.CreatePrivateKeyXML(PrivateKeyPath, provider.ToXmlString(true));
                this.CreatePublicKeyXML(PublicKeyPath, provider.ToXmlString(false));
            }

            catch (Exception exception)
            {
                throw exception;
            }

        }
        const string Base32Map = "ABCDEFGHKLMNPQRSTUVWXYZ123456789";

        private string Base32(byte[] inDataArray)
        {
            var inArray = inDataArray.ToList();
            for (int i = 0; i < inArray.Count() % 5;i++ )
            {
                inArray.Add(0);
            }
            StringBuilder s = new StringBuilder(inArray.Count() * 8 / 5 + 1);
            
            
            for (int i = 0;i < inArray.Count(); i+=5)
            {                
                s.Append( Base32Map[inArray[i + 0] >> 3]);
                s.Append( Base32Map[((inArray[i + 0] & 0x07) << 2) + (inArray[i + 1] >> 6)]);
                s.Append(Base32Map[(inArray[i + 1] & 0x3f) >>1]);
                s.Append(Base32Map[((inArray[i + 1] & 0x01) << 4) + (inArray[i + 2] >> 4)]);
                s.Append(Base32Map[((inArray[i + 2] & 0x0F) << 1) + (inArray[i + 3] >> 7)]);
                s.Append(Base32Map[(inArray[i + 3] & 0x7F) >> 2]);
                s.Append(Base32Map[((inArray[i + 3] & 0x03) << 3) + (inArray[i + 4] >> 5)]);
                s.Append(Base32Map[(inArray[i + 4] & 0x1F)]);
            }
            return s.ToString();
        }
        /// <summary>
        /// 对原始数据进行MD5hash
        /// </summary>
        /// <param name="m_strSource">待hash数据</param>
        /// <returns>返回hash后的字符串编码</returns>
        private string GetHash32(string m_strSource)
        {
           
            HashAlgorithm algorithm = HashAlgorithm.Create("MD5");
            byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(m_strSource);
            byte[] inArray = algorithm.ComputeHash(bytes);
            
            
            //return Convert.ToBase64String(inArray);
            return Base32(inArray);
        }
        private string GetHash(string m_strSource)
        {

            HashAlgorithm algorithm = HashAlgorithm.Create("MD5");
            byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(m_strSource);
            byte[] inArray = algorithm.ComputeHash(bytes);
            return Convert.ToBase64String(inArray);
            //return Base32(inArray);
        }
        /// <summary>

        /// RSA加密

        /// </summary>

        /// <param name="xmlPublicKey">公钥</param>

        /// <param name="m_strEncryptString">MD5加密后的数据</param>

        /// <returns>RSA公钥加密后的数据</returns>

        private string RSAEncrypt(string xmlPublicKey, string m_strEncryptString)
        {
            string str2;
            try
            {
                RSACryptoServiceProvider provider = new RSACryptoServiceProvider();

                provider.FromXmlString(xmlPublicKey);

                byte[] bytes = new UnicodeEncoding().GetBytes(m_strEncryptString);

                str2 = Convert.ToBase64String(provider.Encrypt(bytes, false));

            }

            catch (Exception exception)
            {

                throw exception;

            }

            return str2;

        }

        /// <summary>

        /// RSA解密

        /// </summary>

        /// <param name="xmlPrivateKey">私钥</param>

        /// <param name="m_strDecryptString">待解密的数据</param>

        /// <returns>解密后的结果</returns>

        private string RSADecrypt(string xmlPrivateKey, string m_strDecryptString)
        {

            string str2;

            try
            {

                RSACryptoServiceProvider provider = new RSACryptoServiceProvider();

                provider.FromXmlString(xmlPrivateKey);

                byte[] rgb = Convert.FromBase64String(m_strDecryptString);

                byte[] buffer2 = provider.Decrypt(rgb, false);

                str2 = new UnicodeEncoding().GetString(buffer2);

            }

            catch (Exception exception)
            {

                throw exception;

            }

            return str2;

        }

        /// <summary>

        /// 对MD5加密后的密文进行签名

        /// </summary>

        /// <param name="p_strKeyPrivate">私钥</param>

        /// <param name="m_strHashbyteSignature">MD5加密后的密文</param>

        /// <returns></returns>

        private string SignatureFormatter(string p_strKeyPrivate, string m_strHashbyteSignature)
        {

            byte[] rgbHash = Convert.FromBase64String(m_strHashbyteSignature);
            RSACryptoServiceProvider key = new RSACryptoServiceProvider();
            key.FromXmlString(p_strKeyPrivate);
            RSAPKCS1SignatureFormatter formatter = new RSAPKCS1SignatureFormatter(key);
            formatter.SetHashAlgorithm("MD5");      
            byte[] inArray = formatter.CreateSignature(rgbHash);
            return Convert.ToBase64String(inArray);

        }

        /// <summary>

        /// 签名验证

        /// </summary>

        /// <param name="p_strKeyPublic">公钥</param>

        /// <param name="p_strHashbyteDeformatter">待验证的用户名</param>

        /// <param name="p_strDeformatterData">注册码</param>

        /// <returns></returns>

        private bool SignatureDeformatter(string p_strKeyPublic, string p_strHashbyteDeformatter, string p_strDeformatterData)
        {

            try
            {

                byte[] rgbHash = Convert.FromBase64String(p_strHashbyteDeformatter);

                RSACryptoServiceProvider key = new RSACryptoServiceProvider();

                key.FromXmlString(p_strKeyPublic);

                RSAPKCS1SignatureDeformatter deformatter = new RSAPKCS1SignatureDeformatter(key);

                deformatter.SetHashAlgorithm("MD5");

                byte[] rgbSignature = Convert.FromBase64String(p_strDeformatterData);

                if (deformatter.VerifySignature(rgbHash, rgbSignature))
                {

                    return true;

                }

                return false;

            }

            catch
            {

                return false;

            }

        }
        ///  <summary>

        ///  创建公钥文件

        ///  </summary>

        ///  <param name="path"></param>

        ///  <param name="publickey"></param>

        private void CreatePublicKeyXML(string path, string publickey)
        {

            try
            {

                FileStream publickeyxml = new FileStream(path, FileMode.Create);

                StreamWriter sw = new StreamWriter(publickeyxml);

                sw.WriteLine(publickey);

                sw.Close();

                publickeyxml.Close();

            }

            catch
            {

                throw;

            }

        }

        ///  <summary>

        ///  创建私钥文件

        ///  </summary>

        ///  <param name="path"></param>

        ///  <param name="privatekey"></param>

        private void CreatePrivateKeyXML(string path, string privatekey)
        {

            try
            {

                FileStream privatekeyxml = new FileStream(path, FileMode.Create);

                StreamWriter sw = new StreamWriter(privatekeyxml);

                sw.WriteLine(privatekey);

                sw.Close();

                privatekeyxml.Close();

            }

            catch
            {

                throw;

            }

        }

        ///  <summary>

        ///  读取公钥

        ///  </summary>

        ///  <param name="path"></param>

        ///  <returns></returns>

        private string ReadPublicKey(string path)
        {

            StreamReader reader = new StreamReader(path);

            string publickey = reader.ReadToEnd();

            reader.Close();

            return publickey;

        }

        ///  <summary>

        ///  读取私钥

        ///  </summary>

        ///  <param name="path"></param>

        ///  <returns></returns>

        private string ReadPrivateKey(string path)
        {

            StreamReader reader = new StreamReader(path);

            string privatekey = reader.ReadToEnd();

            reader.Close();
            return privatekey;
        }

    }
}
