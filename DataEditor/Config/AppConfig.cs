using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Config
{
    public class AppConfig
    {


        static public string TaskRootPath;
        static public string DownloadDirName ="00000000(00)";
        static public dynamic ConfigIni= new QcPublic.DynamicIniConfig(AppDomain.CurrentDomain.BaseDirectory + "\\config\\setting.ini");
        public  const string DbCon = "Provider=OraOLEDB.Oracle.1;Persist Security Info=TRUE;Data Source={0}/IGCESDB;User Id=igces;Password={1};Pooling=True;Min Pool Size=50;Max Pool=2000";
        static public void ini()
        {          
            TaskRootPath = ConfigIni.Path.TaskRootPath;
            if(!Directory.Exists(TaskRootPath))
            {
                TaskRootPath = AppDomain.CurrentDomain.BaseDirectory + "\\temp";
                try
                {
                    Directory.CreateDirectory(TaskRootPath + "\\" + DownloadDirName);
                }
                catch
                {

                }

            }
         //   ConfigIni.SaveToFile();       

        }
    }
}
