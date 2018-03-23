using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Management;
using QcPublic;
namespace QcPublic
{
    public class QcHardWare
    {
      
       public  string GetUserName()
        {
           return GetManagementInfo("Win32_ComputerSystem","UserName");           
        }
       public XElement GetSystemInfo(XElement el)
       {
           return GetSystemInfoXml(el,"Win32_ComputerSystem");
       }
       private XElement GetSystemInfoXml(XElement el, string partion)
       {
           using (ManagementClass mc = new ManagementClass(partion))
           {
               using (ManagementObjectCollection moc = mc.GetInstances())
               {
                   foreach (ManagementObject mo in moc)
                   {
                           foreach (var p in mo.Properties)
                           {
                               el.Add(new XElement(p.Name, p.Value));
                           }                      
                   }
               }
           }
           return el;
       }
       private string GetManagementInfo(string partion,string id="*")
       {          
           try
           {
               String ret = "";
               List<string> lst = new List<string>();
               using (ManagementClass mc = new ManagementClass(partion))
               {
                   using (ManagementObjectCollection moc = mc.GetInstances())
                   {
                       foreach (ManagementObject mo in moc)
                       {
                           if (id == "*")
                           {
                               foreach (var p in mo.Properties)
                               {
                                   ret += p.Name + ":" + p.Value + "\r\n";
                               }
                           }
                           else
                           {
                               var t = mo.Properties[id].Value;
                               if (t != null)
                               {
                                   string str = ReplaceLowOrderAscIIChar(t.ToString().Trim());
                                   if(!string.IsNullOrEmpty(str))
                                       lst.Add(str.Replace(" ",""));
                               }
                           }
                       }
                   }
               }
               lst.Sort();
               lst.ForEach(t => { if (!string.IsNullOrWhiteSpace(t)) ret += t + ";"; });
               return ret;
           }
           catch
           {
               return "";
           }          
       }
     

       private string ReplaceLowOrderAscIIChar(string temp)
       {
           StringBuilder info = new StringBuilder();
           foreach (char cc in temp)
           {
               int ss = (int)cc;
               if (((ss >= 0) && (ss <= 8)) || ((ss >= 11) && (ss <= 12)) || ((ss >= 14) && (ss <= 31))) { }
               //info.Append(Convert .ToChar(32));
               else
                   info.Append(cc);
           }
           return info.ToString();
       }
        public string GetDiskID()
        {
            try
            {
                return QcDisk.GetHddInfo(0).SerialNumber;//获取第一个磁盘的序号
            }
            catch
            {
                return "";
            }
           //// return GetManagementInfo("Win32_DiskDrive","SerialNumber");

           // ManagementObjectSearcher mos = new ManagementObjectSearcher("Associators of{win32_LogicalDisk='" + systempath + "'} where ResultClass=Win32_DiskPartition");
            //uint diskindex = 0;
            //try
            //{
            //    string systempath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Windows);
            //    systempath = systempath.Substring(0, 2);
            //    ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid='" + systempath + "'");

            //    string str = ReplaceLowOrderAscIIChar(disk.Properties["SerialNumber"].Value.ToString());
            //    if (!string.IsNullOrEmpty(str))
            //        return str.Replace(" ", "");
            //    else
            //        return "";
            //}
            //catch
            //{
            //    return "";
            //}
            //foreach (ManagementObject mgt in mos.Get())
            //{
            //    diskindex = (uint)mgt.Properties["index"].Value;
            //}
            //using (ManagementClass mc = new ManagementClass("Win32_DiskDrive"))
            //{
            //    using (ManagementObjectCollection moc = mc.GetInstances())
            //    {
            //        foreach (ManagementObject mo in moc)
            //        {
            //            if ((uint)mo.Properties["Index"].Value == 0)
            //            {
            //                object t = mo.Properties["SerialNumber"].Value;
            //                if (t != null)
            //                {
            //                    string str = ReplaceLowOrderAscIIChar(t.ToString().Trim());
            //                    if (!string.IsNullOrEmpty(str))
            //                        return str.Replace(" ", "");
            //                }
            //            }

            //        }
            //    }

            //}
            //return "";
        }       
        public string GetCpuID()
        {
            return GetManagementInfo("Win32_Processor", "ProcessorId");              
        }
        public string GetMacAddress()
        {
            return GetManagementInfo("Win32_NetworkAdapterConfiguration","MACAddress");
        }
    }
}
