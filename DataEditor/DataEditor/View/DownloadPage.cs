using DataEditor.Model;
using DevExpress.XtraBars.Docking2010;
using DevExpress.XtraBars.Docking2010.Customization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataEditor
{
    public partial class DownloadPage : DataEditor.TaskDevicePage
    {
        public DownloadPage(string skinName = "HybridApp", string model = "DEVICE") : base(skinName, model)
        {
            InitializeComponent();           
            iniMytool();
            IniDbConn();
            LoadDevice();                
        }
        private void IniDbConn()
        {
            try
            {
                string ip = Config.AppConfig.ConfigIni.Service.IP;
                string pw = QcPublic.QcEncrypt.Decode(Config.AppConfig.ConfigIni.Service.PW);
                QcPublic.DbHelper.SetDbServer(string.Format(Config.AppConfig.DbCon, Config.AppConfig.ConfigIni, ip, pw));
            }
            catch
            {

            }
        }
        private void LoadDevice()
        {
            this.IniFilterItem(loadDeviceType());
        }
        private void iniMytool()
        {
            WindowsUISeparator sp = new WindowsUISeparator();
            sp.IsLeft = false;
            this.Bar_Tool.Buttons.Add(sp);


            WindowsUIButton download;
            WindowsUIButtonImageOptions imgdownload = new WindowsUIButtonImageOptions();
            imgdownload.Image = Common.getImageByLocal("download.png");
            download = new WindowsUIButton("下载设备", true, imgdownload);
            download.IsLeft = false;
            download.Click += Download_Click;
            this.Bar_Tool.Buttons.Add(download);

            WindowsUIButton setting;
            WindowsUIButtonImageOptions imgsetting = new WindowsUIButtonImageOptions();
            imgsetting.Image = Common.getImageByLocal("DbSetting.png");
            setting = new WindowsUIButton("设置服务", true, imgsetting);
            setting.IsLeft = false;
            setting.Click += Setting_Click;

            this.Bar_Tool.Buttons.Add(setting);           


        }

        private void Setting_Click(object sender, EventArgs e)
        {
            FlyoutDialog dia = new FlyoutDialog(this.FindForm(), new DbServiceSetting());
            if (dia.ShowDialog() == DialogResult.OK)
            {
                FlyoutDialog dia2 = new FlyoutDialog(this.FindForm(), new DownloadSelectName());
                if (dia2.ShowDialog() == DialogResult.OK)
                {
                    LoadDevice();
                }
            }
        }

        private void Download_Click(object sender, EventArgs e)
        {
            if (QcPublic.DbHelper.Check(QcPublic.DbHelper.ConnectionString))
            {
                FlyoutDialog dia = new FlyoutDialog(this.FindForm(), new DownloadSelectName());
                if (dia.ShowDialog() == DialogResult.OK)
                {
                    LoadDevice();
                }
            }
            else
           {
                FlyoutDialog dia = new FlyoutDialog(this.FindForm(), new DbServiceSetting());
                if (dia.ShowDialog() == DialogResult.OK)
                {
                    FlyoutDialog dia2 = new FlyoutDialog(this.FindForm(), new DownloadSelectName());
                    if (dia2.ShowDialog() == DialogResult.OK)
                    {
                        LoadDevice();
                    }
                }
            }
        }

        private IList<FilterItem> loadDeviceType()
        {
            EditTask task = null;

            string fileneme = Common.getDownloadFilename();
            try
            {
                task = Common.LoadFromXml(fileneme, typeof(EditTask)) as EditTask;
            }
            catch
            {

            }


            IList<FilterItem> type = new List<FilterItem>();           
            FilterItem all = new FilterItem("全部", task!=null?task.Devices.Length:0, "全部");
            FilterItem A = new FilterItem("全站仪", task != null ? task.Devices.Where(t=>t.DeviceType == "全站仪").Count() : 0, "全站仪");
            FilterItem B = new FilterItem("经纬仪", task != null ? task.Devices.Where(t => t.DeviceType == "经纬仪").Count() : 0, "经纬仪");
            FilterItem C = new FilterItem("水准仪", task != null ? task.Devices.Where(t => t.DeviceType == "水准仪").Count() : 0, "水准仪");
            FilterItem D = new FilterItem("GNSS", task != null ? task.Devices.Where(t => t.DeviceType == "GNSS").Count() : 0, "GNSS");
            FilterItem E = new FilterItem("水准标尺",  task!=null?task.Devices.Where(t => t.DeviceType == "水准标尺").Count() : 0, "水准标尺");
            type.Add(all); type.Add(A); type.Add(B); type.Add(C); type.Add(D); type.Add(E);
            return type;
        }
    }
}
