using DevExpress.Mvvm.DataAnnotations;
using System;
using System.IO;

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars.Docking2010;
using DataEditor.Model;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Utils.MVVM.Services;
using DataEditor.View;
using DevExpress.XtraBars.Docking2010.Customization;

namespace DataEditor
{
    public partial class TaskPage : TaskDevicePage
    {

   
        public TaskPage(string skinName = "HybridApp", string model = "TASK") : base(skinName,model)
        {
            InitializeComponent();
            iniMytool();
            LoadTask();
        }
       
        private void iniMytool()
        {
            WindowsUISeparator  sp = new WindowsUISeparator();
            sp.IsLeft = false;
            this.Bar_Tool.Buttons.Add(sp);


            WindowsUIButton newtask;
            WindowsUIButtonImageOptions imgaddtask = new WindowsUIButtonImageOptions();
            imgaddtask.Image = Common.getImageByLocal("new_task.png");
            newtask= new WindowsUIButton("新建任务", true, imgaddtask);          
            newtask.IsLeft = false;
            newtask.Click += Newtask_Click;
            this.Bar_Tool.Buttons.Add(newtask);
          
            WindowsUIButtonImageOptions imgdelte = new WindowsUIButtonImageOptions();
            imgdelte.Image = Common.getImageByLocal("delete_task.png");
            WindowsUIButton deleteTask = new WindowsUIButton("删除任务", true, imgdelte);
            deleteTask.Click += DeleteTask_Click;
            deleteTask.IsLeft = false;
            this.Bar_Tool.Buttons.Add(deleteTask);

            WindowsUISeparator sp1 = new WindowsUISeparator();
            sp1.IsLeft = false;
            this.Bar_Tool.Buttons.Add(sp1);

            WindowsUIButton adddevice;
            WindowsUIButtonImageOptions addDevice = new WindowsUIButtonImageOptions();
            addDevice.Image = Common.getImageByLocal("add_device.png");
            adddevice= new WindowsUIButton("添加设备", true, addDevice);
            adddevice.IsLeft = false;
            adddevice.Click += Adddevice_Click;
            this.Bar_Tool.Buttons.Add(adddevice);

            //WindowsUIButtonImageOptions deleteDevice = new WindowsUIButtonImageOptions();
            //deleteDevice.Image = Common.getImageByLocal("delete_device.png");
            //WindowsUIButton deltete = new WindowsUIButton("删除设备",true, deleteDevice);
            //deltete.IsLeft = false;
            //deltete.Click += Deltete_Click;
            //this.Bar_Tool.Buttons.Add(deltete);

            WindowsUISeparator sp2 = new WindowsUISeparator();
            sp2.IsLeft = false;
            this.Bar_Tool.Buttons.Add(sp2);


            WindowsUIButton setting;
            WindowsUIButtonImageOptions imgsetting = new WindowsUIButtonImageOptions();
            imgsetting.Image = Common.getImageByLocal("DbSetting.png");
            setting = new WindowsUIButton("设置空间", true, imgsetting);
            setting.IsLeft = false;
            setting.Click += Setting_Click;
            this.Bar_Tool.Buttons.Add(setting);
            //
            DevExpress.XtraEditors.SimpleButton simpleButton1 = new DevExpress.XtraEditors.SimpleButton();


            simpleButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));

           simpleButton1.Location = new System.Drawing.Point(-50, 20);
           simpleButton1.Name = "simpleButton1";
           simpleButton1.Size = new System.Drawing.Size(75, 35);
           simpleButton1.TabIndex = 0;
           simpleButton1.Text = "开始采集";
            this.Bar_Tool.Controls.Add(simpleButton1);

           
        }

        private void Setting_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            if (f.ShowDialog() == DialogResult.OK)
            {
                Config.AppConfig.TaskRootPath = f.SelectedPath;                    
                Config.AppConfig.ConfigIni.Path.TaskRootPath = f.SelectedPath;
                Config.AppConfig.ConfigIni.SaveToFile();
                LoadTask();
            }

        }

        private void LoadTask(string day= "*.")
        {
            this.IniFilterItem(loadTaskByRoolPath(day));
            GC.Collect(0);
        }
        private void Adddevice_Click(object sender, EventArgs e)
        {
            FlyoutDialog dia = new FlyoutDialog(this.FindForm(), new AddDevice(this.SelectedItem ));
           if( dia.ShowDialog()==DialogResult.OK)
            {
                this.RefreshSelected();
            }
        }

        private void Newtask_Click(object sender, EventArgs e)
        {
            FlyoutDialog dia = new FlyoutDialog(this.FindForm(), new NewTask());
          
          if(  dia.ShowDialog()==DialogResult.OK)
            {
                LoadTask();//刷新加载
            }
        }

        private void DeleteTask_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("选择的任务将被删除，是否继续？", "警告", MessageBoxButtons.YesNo) == DialogResult.No) return;

            IList<FilterItem > sor = this.FilterItems;

            int[] s;
            if (GridCtlFilter.MainView is DevExpress.XtraGrid.Views.Tile.TileView)
            {
          
                DevExpress.XtraGrid.Views.Tile.TileView tile = GridCtlFilter.MainView as DevExpress.XtraGrid.Views.Tile.TileView;
                s = tile.GetSelectedRows();
                for (int index = s.Length - 1; index >= 0; index--)
                {
                    if (Common.DeleteTaskFiles(sor[s[index]].Name))
                    {
                        sor.RemoveAt(s[index]);
                        if (s[index] != sor.Count)
                        {
                            tile.ViewCaption = sor[s[index]].Name;
                            this.SelectedItem = sor[s[index]];
                        }
                    }
                }             
            }
            this.IniFilterItem(sor);
            this.GridCtlFilter.RefreshDataSource();
        }

        private void Deltete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("选择的设备将被删除，是否继续？","警告",MessageBoxButtons.YesNo)==DialogResult.No) return ;

            IList<TaskDevice> sor = this.TaskDevices;
           
            int[] s;
            if (GridCtl.MainView is DevExpress.XtraGrid.Views.Grid.GridView)
            {
                DevExpress.XtraGrid.Views.Grid.GridView grid = GridCtl.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
                s = grid.GetSelectedRows();
            }            
            else
            {
                DevExpress.XtraGrid.Views.Tile.TileView tile = GridCtl.MainView as DevExpress.XtraGrid.Views.Tile.TileView;
                s = tile.GetSelectedRows();
           
            }
            
            for (int index = s.Length-1 ; index >= 0; index--)
            {
                sor.RemoveAt(s[index]);
            }
            this.IniDeviceItem(sor);
            this.GridCtl.RefreshDataSource();
        }
        private IList<FilterItem> loadTaskByRoolPath(string day="*.")
        {
            
            IList<FilterItem> ALltask = new List<FilterItem>();
           DirectoryInfo root  =new DirectoryInfo( Config.AppConfig.TaskRootPath);
            foreach (DirectoryInfo  dir in root .GetDirectories(day+"*").OrderByDescending(t=>t.Name))
            {
                try
                {
                    if (dir.Name == Config.AppConfig.DownloadDirName) continue;
                    FileInfo[] file = dir.GetFiles(dir.Name + ".xml");
                    EditTask task = file.Length > 0 ? Common.LoadFromXml(file.FirstOrDefault().FullName, typeof(EditTask)) as EditTask : null;
                    ALltask.Add(new FilterItem(dir.Name, task != null ? task.Devices.Where(t => t.DeviceType == task.Type).Count() : 0, task != null ? task.Type : "Err"));
                }
                catch
                {
                    continue;
                }
            }
            return ALltask;
        }



    }



}
