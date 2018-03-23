using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace QcPublic
{
  public   class QcRecentlyJob
    {
        private static  IList<string> lst_recentlyfile = new List<string>();
        private static string c_recentlyfile;
        public  delegate void nearestjobchange(IList<string> newrecentlyfile);
        public static  event nearestjobchange NearestFileChanged;
            

        public static void InitJob()
        {
            lst_recentlyfile.Clear();
            QcJob.GetMyJob(QcUser.User).Where(t=>!string.IsNullOrEmpty(t["方案ID"])).OrderBy(t => t["修改日期"]).ToList().ForEach(j => lst_recentlyfile.Add(j.Code));
            if (NearestFileChanged != null)
            {
                NearestFileChanged(lst_recentlyfile);
            }
        }
        public static void InitJob(string localfile)
        {
            c_recentlyfile = localfile;
            oct_loadrecentlyfile();
            if (NearestFileChanged != null)
            {
                NearestFileChanged(lst_recentlyfile);
            }
        }

        public static void ResetSetNetworkRecentlyFile()
        {
            lst_recentlyfile.Clear();
            QcJob.GetMyJob(QcUser.User).Where(t => !string.IsNullOrEmpty(t["方案ID"])).OrderBy(t => t["修改日期"]).ToList().ForEach(j => lst_recentlyfile.Add(j.Code) );
            if (NearestFileChanged != null)
            {
                NearestFileChanged(lst_recentlyfile);
            }
        }
        public static  string NearestJob
        {
            set
            { 
                setnearestfile(value);               
            } 
        }
        public static IList<string> RecentlyFile
        {
            get { return lst_recentlyfile; }
        }
        public static void RemoveFile(string filename)
        {
            if (lst_recentlyfile.Contains(filename))
            {
                lst_recentlyfile.Remove(filename);//删除原来位置的文件 
            }
            oct_writerecentlyfiletofile();

        }

        private static void setnearestfile(string filename)
        {
            if (lst_recentlyfile.Contains(filename))
            {
                lst_recentlyfile.Remove(filename);//删除原来位置的文件                      
            }
            lst_recentlyfile.Add(filename);
            oct_writerecentlyfiletofile();
        }


        private static  void oct_writerecentlyfiletofile()
        {
             if (NearestFileChanged != null)
                {
                    NearestFileChanged(lst_recentlyfile);
                }
            using (StreamWriter writer = new StreamWriter(c_recentlyfile, false))
            {
                foreach (string filepath in lst_recentlyfile)
                {
                    writer.WriteLine(filepath);
                }
                writer.Close();
            }
        }
        private static void oct_loadrecentlyfile()
        {
            lst_recentlyfile.Clear();
            if (!File.Exists(c_recentlyfile)) { File.CreateText(c_recentlyfile).Close(); }
            using (StreamReader reader = new StreamReader(c_recentlyfile))
            {
                string str_file = reader.ReadLine();
                while (str_file != null)
                {
                    lst_recentlyfile.Add(str_file);
                    str_file = reader.ReadLine();
                }
                reader.Close();
            }
           
        }
    }
}
