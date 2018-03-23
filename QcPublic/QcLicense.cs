using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Security.Cryptography.Xml;
using QcNet;
using System.Threading;
using Microsoft.Win32;
using QcPublic;
namespace QcPublic
{
    public enum LicenseRetCode
    {
        成功,
        授权为空,
        未授权,
        超出授权用户数,
        模块未授权,
        产品未授权,
        超出授权时间范围,
        授权服务器连接超时,
        超出允许授权期限,
        授权服务系统时间被修改,
        无效机器码,
        授权未知错误,
        授权未知错误01,
        授权文件不存在

    }

    public interface ILicense
    {
        LicenseRetCode CheckFeature(int Feature);

        LicenseRetCode CheckModule(string module);

        LicenseRetCode CheckProduct(string ProductName);

        LicenseRetCode RegsiterPC(bool repeat = false);

        void ReleasePC();

    }
    public class pcsn
    {
        public DateTime lastlime { get; set; }
        public string sn { get; set; }
        public int logoncount { get; set; }
        public pcsn(string _sn, DateTime _lasttime, int _logoncount)
        {
            lastlime = _lasttime;
            sn = _sn;
            logoncount = _logoncount;
        }
    }
    public static class PublicLicense
    {
        public static bool LocalModel { get; set; }
        public static ILicense License;
        public static void InitialLicense(string config, string lic)
        {
            dynamic ini;
            ini = new DynamicIniConfig(config);
            QcNetLicense newnetlicense;
            QcLocalLicense newlocallicense;
            QcTimeLicense newtimelicense;
            switch (((string)ini.License.IsLocal).ToUpper())
            {
                case "TRUE":
                    LocalModel = true;
                    newlocallicense = new QcLocalLicense();
                    newlocallicense.SetLicense(QcLicense.ReadLicFile(lic));
                    License = newlocallicense;
                    break;
                case "FALSE":
                    LocalModel = false;
                    newnetlicense = new QcNetLicense();
                    newnetlicense.SetServer(ini.MsgServer.Ip, Convert.ToUInt16(ini.MsgServer.Port));
                    License = newnetlicense;
                    break;
                case "ONLY_IGCES_SUPERTIME"://HAHA 超级模式
                    newtimelicense = new QcTimeLicense(DateTime.Parse("9999-01-01"));
                    License = newtimelicense;
                    break;
                case "TRIAL"://试用模式
                    QcLicense timelic = new QcLicense(QcLicense.ReadLicFile(lic), false);
                    XDocument doc = timelic.LicDoc;
                    LocalModel = false;
                    if (doc.Root.Element("单位名称").Value == "TRIAL")
                    {
                        if (DateTime.Now.Subtract(DateTime.Parse(doc.Root.Element("EndDate").Value)).Days <= 0)
                        {
                            newlocallicense = new QcLocalLicense();
                            newlocallicense.SetLicense(QcLicense.ReadLicFile(lic), false);
                            License = newlocallicense;
                        }
                        else
                        {
                            QcLog.LogString("初始授权失败，试用模式下，试用时间已经超出授权时间，请改用其他模式或重新申请授权");
                        }
                    }
                    else
                    {
                        QcLog.LogString("初始授权失败，试用模式下，lic文件不是试用授权文件，请改用其他模式或重新申请授权");
                    }

                    break;

            }
            if (License == null)
            {
                License = new QcNullLicense();
                QcLog.LogString("初始授权失败，请检查授权模式是否正确");
            }
        }
    }

    public class QcLicense
    {
        public static IDictionary<string, pcsn> lstsn = new Dictionary<string, pcsn>();
        //QcLicense license = null;
        XDocument XLicDoc = null;
        public LicenseRetCode Stats = LicenseRetCode.产品未授权;
        public XDocument LicDoc
        {
            get
            {
                return XLicDoc;
            }
        }
        public static string ReadLicFile(string filename)
        {
            try
            {

                QcCoder coder = new QcCoder();
                return coder.GetEncodingString(System.IO.File.ReadAllBytes(filename), 0);
            }
            catch (Exception e)
            {
                QcLog.LogString(e.Message);
                return "";
            }
        }
        QcProtect protect;
        private string _licensetext;
        public bool chksn { get; set; }
        public QcLicense(string licensetext, bool pchksn = true)
        {
            _licensetext = licensetext;
            protect = new QcProtect(pchksn);
            chksn = pchksn;
            checklicensetext();

        }

        public void checklicensetext()
        {
            string licensetext = _licensetext;
            try
            {
                XLicDoc = XDocument.Parse(licensetext);
                if (protect.CheckLicense(licensetext))
                {
                    Stats = LicenseRetCode.成功;
                    if (DateTime.Now.CompareTo(DateTime.Parse(LicDoc.Root.Element("EndDate").Value.ToString())) > 0)
                    {
                        Stats = LicenseRetCode.超出授权时间范围;
                    }
                    else
                    {
                        RegistryKey rsg = null;
                        if (Registry.LocalMachine.OpenSubKey("SOFTWARE\\OfficeServer") == null)
                        {
                            if (chksn)//如果要进行机器码检验那么就需要进行注册时间判断
                            {
                                if (DateTime.Now.Subtract(DateTime.Parse(LicDoc.Root.Element("StartDate").Value.ToString())).Days > 7)
                                {
                                    Stats = LicenseRetCode.超出允许授权期限;
                                }
                            }
                            //不论超不超期限都要进行记录系统时间
                            Registry.LocalMachine.CreateSubKey("SOFTWARE", RegistryKeyPermissionCheck.ReadWriteSubTree).CreateSubKey("OfficeServer");
                            rsg = Registry.LocalMachine.CreateSubKey("SOFTWARE").OpenSubKey("OfficeServer", true);
                            rsg.SetValue("PreDate", DateTime.Now.ToShortDateString());
                            rsg.Close();


                        }
                        else
                        {
                            rsg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\OfficeServer", true);
                            if (DateTime.Now.CompareTo(DateTime.Parse(rsg.GetValue("PreDate").ToString())) < 0)
                            {
                                Stats = LicenseRetCode.授权服务系统时间被修改;
                            }
                            else
                            {
                                rsg.SetValue("PreDate", DateTime.Now.ToShortDateString());
                            }
                            rsg.Close();
                        }
                    }
                    QcLog.LogString("授权文件检查成功，当前机器码为：" + new QcProtect().GetSn() + "授权机器码为" + LicDoc.Root.Element("序列号").Value + "\n当前机器特征为:" + new QcProtect().GetHdw());
                    //if (DateTime.Now > DateTime.Parse(LicDoc.Root.Element("EndDate").Value.ToString())) Stats = LicenseRetCode.超出允许授权期限;
                    //创建系统时间注入文件，如果没有则创建，如果有则判断系统时间是否被篡改，如果没有篡改就注入系统时间，如果超过7天时间仍没有注册不允许
                    //创建系统时间注入文件，
                }
                else
                {
                    Stats = LicenseRetCode.未授权;
                    QcLog.LogString("授权文件检查失败，当前机器码为：" + new QcProtect().GetSn() + "授权机器码为" + LicDoc.Root.Element("序列号").Value + "\n当前机器特征为:" + new QcProtect().GetHdw());
                }
                //授权时间判断

                //注册时间判断在这里判断
                //系统服务时间篡改在这里判断
                lstFeature.AddRange(LicDoc.Root.Element("FeatureID").Value.Split('_'));

                lstProduct.AddRange(LicDoc.Root.Element("Product").Value.Split('_'));
                foreach (var v in LicDoc.Root.Element("Modules").Elements("ModuleName"))
                {
                    lstModule.Add(v.Value);
                }

                QcLog.LogString("授权文件读取成功!");
                QcLog.LogString("FeatureList = " + String.Join(";", lstFeature));
                QcLog.LogString("ProductList = " + String.Join(";", lstProduct));
            }
            catch (Exception e)
            {
                Stats = LicenseRetCode.未授权;
                QcLog.LogString("授权文件不合无法读取" + e.Message);
            }
        }
        private List<string> lstFeature = new List<string>();
        public LicenseRetCode CheckFeature(int FeatureID)
        {
            if (lstFeature.Contains(FeatureID.ToString())) return LicenseRetCode.成功;
            return LicenseRetCode.模块未授权;
        }
        private List<string> lstModule = new List<string>();
        public LicenseRetCode CheckModule(string module)
        {
            if (lstModule.Contains(module)) return LicenseRetCode.成功;
            return LicenseRetCode.模块未授权;
        }
        private List<string> lstProduct = new List<string>();
        public LicenseRetCode CheckProduct(string product)
        {
            if (lstProduct.Contains(product)) return LicenseRetCode.成功;
            return LicenseRetCode.产品未授权;
        }
        public string ProductInfo { get { return string.Join("_", lstProduct); } }
        public string UserInfo { get { return GetUsers().ToString(); } }
        public string TimeInfo { get { return LicDoc.Root.Element("StartDate").Value.ToString() +" ---- "+ LicDoc.Root.Element("EndDate").Value.ToString(); } }

        public LicenseRetCode RegsisterUser(string snp)
        {
            string[] sns = snp.Split('_');
            string sn = sns[0];
            bool repeat = bool.Parse(sns[1]);
            if (this.Stats != LicenseRetCode.成功) return this.Stats;
            if (lstsn.Keys.Contains(sn))
            {
                lstsn[sn].lastlime = DateTime.Now;
                if (!repeat)
                    lstsn[sn].logoncount++;
                return LicenseRetCode.成功;
            }
            else
            {
                if (lstsn.Keys.Count > GetUsers())
                {
                    return LicenseRetCode.超出授权用户数;
                }
                else
                {
                    lstsn.Add(sn, new pcsn(sn, DateTime.Now, 1));
                    return LicenseRetCode.成功;
                }
            }

        }
        public void ReleaseUser(string sn)
        {
            try
            {
                if (lstsn.Keys.Contains(sn))
                {
                    if (lstsn[sn].logoncount == 1)
                    {
                        lstsn.Remove(sn);
                    }
                    else
                    {
                        lstsn[sn].logoncount--;
                    }
                }
            }
            catch { }

        }
        private int GetUsers()
        {
            int users = 0;
            try
            {
                int.TryParse(LicDoc.Root.Element("UserNum").Value, out users);
                return users;
            }
            catch (Exception e)
            {
                return 0;
            }

        }
    }
    public class QcNullLicense : ILicense
    {
        public LicenseRetCode CheckFeature(int Feature)
        {
            return LicenseRetCode.授权为空;
        }

        public LicenseRetCode CheckModule(string module)
        {
            return LicenseRetCode.授权为空;
        }

        public LicenseRetCode CheckProduct(string ProductName)
        {
            return LicenseRetCode.授权为空;
        }

        public LicenseRetCode RegsiterPC(bool repeat = false)
        {
            return LicenseRetCode.授权为空;
        }

        public void ReleasePC()
        {

        }
    }
    public class QcTimeLicense : ILicense
    {

        LicenseRetCode stat;
        public QcTimeLicense(DateTime exp)
        {
            if (exp.Subtract(DateTime.Now).Days <= 1)
            {
                stat = LicenseRetCode.超出允许授权期限;
            }
            else
            {
                stat = LicenseRetCode.成功;
            }
        }
        public LicenseRetCode CheckFeature(int Feature)
        {
            return stat;
        }

        public LicenseRetCode CheckModule(string module)
        {
            return stat;
        }

        public LicenseRetCode CheckProduct(string ProductName)
        {
            return stat;
        }

        public LicenseRetCode RegsiterPC(bool repeat = false)
        {
            return stat;
        }

        public void ReleasePC()
        {

        }
    }
    public class QcLocalLicense : ILicense
    {
        private QcLicense license;
        public void SetLicense(string lic, bool chksn = true)
        {

            license = new QcLicense(lic, chksn);
        }
        public LicenseRetCode CheckFeature(int Feature)
        {
            return license.CheckFeature(Feature);
        }
        public LicenseRetCode CheckModule(string module)
        {
            return license.CheckModule(module);
        }
        public LicenseRetCode CheckProduct(string ProductName)
        {
            return license.CheckProduct(ProductName);
        }
        public LicenseRetCode RegsiterPC(bool repeat = false)
        {
            return license.Stats;
        }
        public void ReleasePC()
        {
        }

    }
    public class QcNetLicense : ILicense
    {
        static QcProtect sn = new QcProtect();
        public void SetServer(string ip, ushort port)
        {
            QcMessagner.IP = ip;
            QcMessagner.Port = port;


        }


        LicenseRetCode PostLicenseMessage(string type, string arg)
        {
            int count = 3;
            QcMsgClient client = new QcMsgClient(null, null);
            AutoResetEvent are = new AutoResetEvent(false);
            QcCmd retcmd = null;
            string cmd = QcCmd.MakeCmd(QcProtocol.QcCommand.QcCheckLicense, type, arg);
            try
            {

                //client =;
                client.ConnectedServer += (o, e) =>
                {
                    client.Send(cmd);
                };
                client.ReceiveCmd += (o, e) =>
                    {

                        retcmd = e.Cmd;
                        are.Set();
                        //接收消息
                    };
            CONTINUE:
                client.Connect(QcMessagner.IP, QcMessagner.Port);
                if (are.WaitOne(5000))//3秒钟超时连接
                {

                    LicenseRetCode ret = LicenseRetCode.授权未知错误01;
                    //if (retcmd == null)
                    //{
                    //    Thread.Sleep(500);//有可能消息接收 事件还没有给retcmd赋值，因此延迟500毫秒 
                    //}
                    if (retcmd != null)
                    {
                        string t_Test = retcmd.tokens(1);
                        ret = (LicenseRetCode)Enum.Parse(typeof(LicenseRetCode), t_Test);

                    }
                    return ret;
                }
                else
                {

                    if (count > 0)
                    {
                        count--;
                        goto CONTINUE;
                    }
                    else
                    {
                        return LicenseRetCode.授权服务器连接超时;
                    }
                }
            }
            catch (Exception e)
            {
                QcLog.LogString(e.Message + e.StackTrace);
                //System.Windows.Forms.MessageBox.Show(e.Message + e.StackTrace);
                return LicenseRetCode.授权未知错误;
            }
            finally
            {
                //这里该怎样断开服务器上的连接呢？
                if (client != null) client.Close();
                are.Set();
            }
        }
        public LicenseRetCode CheckFeature(int Feature)
        {
            return PostLicenseMessage("Feature", Feature.ToString());
        }
        public LicenseRetCode CheckModule(string module)
        {
            return PostLicenseMessage("Module", module.ToString());
        }
        public LicenseRetCode CheckProduct(string ProductName)
        {
            return PostLicenseMessage("Product", ProductName);
        }
        string snp = sn.GetSn();
        public LicenseRetCode RegsiterPC(bool repeat = false)
        {
            if (snp == null)
            {
                return LicenseRetCode.无效机器码;
            }

            string newsnp;
            if (repeat)
                newsnp = snp + "\\" + System.Environment.MachineName.Trim().Replace("_", "-") + "_true";
            else
                newsnp = snp + "\\" + System.Environment.MachineName.Trim().Replace("_", "-") + "_false";
            return PostLicenseMessage("Regsiter", newsnp);  //向服务提供一次注册请求  ，并返回状态 ，       
        }
        public void ReleasePC()
        {
            PostLicenseMessage("Release", snp + "\\" + System.Environment.MachineName.Trim().Replace("_", "-"));//每次退出 就 向服务提供一次 释放信号
        }
    }
}
