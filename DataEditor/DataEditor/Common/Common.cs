using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;
using DataEditor.Model;

namespace DataEditor
{
    public class Common
    {


        internal static Image getImageByLocal(string imgname, string size = "32X32", string Exception = "Unknown")
        {
            try
            {
                string imagepath = Environment.CurrentDirectory + "\\img\\" + size + "\\" + imgname;
                return Image.FromFile(imagepath);
            }
            catch
            {
                try
                {
                    return Image.FromFile(Environment.CurrentDirectory + "\\img\\64X64\\" + Exception + ".png");
                }
                catch
                {
                    return null;
                }
            }
        }
        public static object LoadFromXml(string filePath, Type type)
        {
            object result = null;

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(type);
                    result = xmlSerializer.Deserialize(reader);
                }
            }

            return result;
        }
        public static void SaveToXml(string filePath, object sourceObj)
        {
            if (!string.IsNullOrWhiteSpace(filePath) && sourceObj != null)
            {
                Type type = sourceObj.GetType();

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(type);
                    xmlSerializer.Serialize(writer, sourceObj);
                }
            }
        }
        public static object getDeviecSorByTask(string taskname, string type = null)
        {
            string fileneme = string.Format("{0}\\{1}\\{1}.xml", Config.AppConfig.TaskRootPath, taskname);// + "\\" + _SelectedItem.Name +"\\"+_SelectedItem ""
            if (File.Exists(fileneme))
            {
                EditTask task = null;
                try
                {
                    task = Common.LoadFromXml(fileneme, typeof(EditTask)) as EditTask;

                    if (string.IsNullOrEmpty(type) || task.Type == type)
                    {
                        if( task.Devices.Length==0)
                        {
                            List<TaskDevice> nodevice = new List<TaskDevice>();
                            TaskDevice d = new TaskDevice();
                            d.Photo = Common.getImageByLocal("Err.png");
                            d.Company = "仪器列表为空";
                            d.DeviceType = "错误";
                            d.IsChecked = false;
                            nodevice.Add(d);
                            return nodevice;
                        }
                        else 

                            {
                            return task.Devices;
                        }
                    }
                       
                    else
                    {
                        List<TaskDevice> nodevice = new List<TaskDevice>();
                        TaskDevice d = new TaskDevice();
                        d.Photo = Common.getImageByLocal("Err.png");
                        d.Company = "仪器类型不匹配";
                        d.DeviceType = "错误";
                        d.IsChecked = false;
                        nodevice.Add(d);
                        return nodevice;
                    }
                }
                catch
                {
                    List<TaskDevice> nodevice = new List<TaskDevice>();
                    TaskDevice d = new TaskDevice();
                    d.Photo = Common.getImageByLocal("Err.png");
                    d.Company = "任务文件读取失败";
                    d.DeviceType = "错误";
                    d.IsChecked = false;
                    nodevice.Add(d);
                    return nodevice;
                }
            }
            else
            {
                List<TaskDevice> nodevice = new List<TaskDevice>();
                TaskDevice d = new TaskDevice();
                d.Photo = Common.getImageByLocal("Err.png");
                d.Company = "任务文件不存在";
                d.DeviceType = "错误";
                d.IsChecked = false;
                nodevice.Add(d);
                return nodevice;

            }
        }


        public static object getDeviecSorByDevice(string type="全部")
        {
            string fileneme = getDownloadFilename();
            if (File.Exists(fileneme))
            {
                EditTask task = null;
                try
                {
                    task = Common.LoadFromXml(fileneme, typeof(EditTask)) as EditTask;
                    IEnumerable<TaskDevice> filter;
                    if (type == "全部" || type.ToUpper() == "ALL")
                    {
                        filter= task.Devices;
                    }
                    else
                    {
                        filter = task.Devices.Where(t => t.DeviceType == type);
                    }
                                      
                    if (filter.Count() == 0)
                    {
                        List<TaskDevice> nodevice = new List<TaskDevice>();
                        TaskDevice d = new TaskDevice();
                        d.Photo = Common.getImageByLocal("Err.png");
                        d.Company = "仪器列表为空";
                        d.DeviceType = "错误";
                        d.IsChecked = false;
                        nodevice.Add(d);
                        return nodevice;
                    }
                    else
                    {
                        return filter;
                    }

                }
                catch
                {
                    List<TaskDevice> nodevice = new List<TaskDevice>();
                    TaskDevice d = new TaskDevice();
                    d.Photo = Common.getImageByLocal("Err.png");
                    d.Company = "任务文件读取失败";
                    d.DeviceType = "错误";
                    d.IsChecked = false;
                    nodevice.Add(d);
                    return nodevice;
                }
            }
            else
            {
                List<TaskDevice> nodevice = new List<TaskDevice>();
                TaskDevice d = new TaskDevice();
                d.Photo = Common.getImageByLocal("Err.png");
                d.Company = "设备清单文件不存在";
                d.DeviceType = "错误";
                d.IsChecked = false;
                nodevice.Add(d);
                return nodevice;

            }
        }

        public static string GetNewTaskName()
        {
            DateTime d = DateTime.Now;//当前日期
            string cur = string.Format("{0}{1}{2}", d.Year.ToString("00"), d.Month.ToString("00"), d.Day.ToString("00"));
            List<string> exists = new List<string>();
            foreach (DirectoryInfo dir in new DirectoryInfo(Config.AppConfig.TaskRootPath).GetDirectories(cur + "*").OrderByDescending(t => t.Name))
            {
                exists.Add(dir.Name);
            }
            int newindex = exists.Count() > 0 ? Convert.ToInt32(exists.First().Substring(9, 2)) + 1 : 1;
            return string.Format("{0}({1})", cur, newindex.ToString("00"));
        }
        public static string GetLastTaskName(string taskname)
        {
            if (string.IsNullOrEmpty(taskname)) return "null";
            if (taskname == "null") return "null";
            string cur = taskname.Substring(0, 8);
            List<string> exists = new List<string>();
            bool tag = false;

            foreach (DirectoryInfo dir in new DirectoryInfo(Config.AppConfig.TaskRootPath).GetDirectories(cur + "*").OrderByDescending(t => t.Name))
            {
                if (tag) return dir.Name;
                if (taskname == dir.Name) tag = true;
                exists.Add(dir.Name);
            }
            return tag ? "null" : exists.Count > 0 ? exists.First() : "null";
        }
        /// <summary>
        /// 包含扩展名
        /// </summary>
        /// <param name="interval"> 间隔周期 秒为单位</param>
        /// <returns></returns>
        public static string getNextDownloadFilename(long interval_Seconds = 3600)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            long timespan = (long)(DateTime.Now - startTime).TotalSeconds;
            long lasttimespan = 0;
            foreach (FileInfo file in new DirectoryInfo(Config.AppConfig.TaskRootPath + "\\" + Config.AppConfig.DownloadDirName).GetFiles("*.xml").OrderByDescending(t => t.Name))
            {
                lasttimespan = Convert.ToInt32(Path.GetFileNameWithoutExtension(file.Name));
                break;
            }
            long newtime = timespan - lasttimespan > interval_Seconds ? timespan : lasttimespan;
            return newtime.ToString() + ".xml";
        }
        public static string getDownloadFilename()
            {
            try
            {
                foreach (FileInfo file in new DirectoryInfo(Config.AppConfig.TaskRootPath + "\\" + Config.AppConfig.DownloadDirName).GetFiles("*.xml").OrderByDescending(t => t.Name))
                {
                    return file.FullName;

                }
            }
            catch
            {
                return Config.AppConfig.TaskRootPath + "\\" + Config.AppConfig.DownloadDirName + "\\" + Config.AppConfig.DownloadDirName + ".xml";
            }
            return Config.AppConfig.TaskRootPath + "\\" + Config.AppConfig.DownloadDirName + "\\" + Config.AppConfig.DownloadDirName + ".xml";
        }

        public static bool   DeleteTaskFiles(string taskname)
        {
             try
            {
                Directory.Delete(Config.AppConfig.TaskRootPath + "\\" + taskname,true);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
