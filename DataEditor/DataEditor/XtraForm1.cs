using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Navigation;
using DataEditor.Model;

namespace DataEditor
{
    public partial class MainForm :DevExpress.XtraBars.Ribbon.RibbonForm
    {
        Point _HoverTreePosition;
        public MainForm()
        {
            iniQcPublic();
            InitializeComponent();
            inipageDevice();
            inipageTask();
              
        }

        private void iniQcPublic()
        {
            QcPublic.QcLog.LogEvent += QcLog_LogEvent;
            Config.AppConfig.ini();
        }

        private void QcLog_LogEvent(string str)
        {
            string logname = string.Format("{0}{1}{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            System.IO.File.AppendAllText(Application.StartupPath + "\\log\\" + logname + ".txt",str);
        }

        private void inipageTask()
        {
           
            TaskPage     ct = new TaskPage(); 
            ct.Dock = DockStyle.Fill;   
            Page_Task.Controls.Add(ct); 
        }
      
        private void inipageDevice()
        {
            DownloadPage ct = new DownloadPage(); 
            ct.Dock = DockStyle.Fill;
            Page_Device.Controls.Add(ct);
        }
      
        #region this form event
        private void tileNavPane1_MouseDown(object sender, MouseEventArgs e)
        {
            _HoverTreePosition.X = e.X;
            _HoverTreePosition.Y = e.Y;
        }
        
        private void tileNavPane1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point h_myPosittion = MousePosition;
                h_myPosittion.Offset(-_HoverTreePosition.X, -_HoverTreePosition.Y);
                Location = h_myPosittion;
            }
        }
        
        private void Form_BT_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            fun_by_byname((sender as NavButton).Name);                
        }

        private void tileNavPane1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            fun_by_byname("bt_form_max");
        }
        #endregion

        #region this form  function
        private void fun_by_byname(string bt_name)
        {
            switch (bt_name)
            {
                case "bt_form_close":
                    Application.ExitThread();
                    break;
                case "bt_form_max":
                    if(this.WindowState==FormWindowState.Normal)
                    {
                        this.WindowState = FormWindowState.Maximized;
                      //  bt_form_max.Glyph =global::DataEditor.Properties.Resources.formmax_2_16;
                    }
                    else
                    {
                        this.WindowState = FormWindowState.Normal;
                     //   bt_form_max.Glyph = global::DataEditor.Properties.Resources.formmax16;
                    }
                    break;
                case "bt_form_min":
                    this.WindowState = FormWindowState.Minimized;
                    break;
            }
        
        }




        #endregion

        private void Bar_Funtons_SelectedItemChanged(object sender, TileItemEventArgs e)
        {
            Frame_Pages.SelectedPageIndex =  Bar_Group_A.Items.IndexOf(e.Item);
        }

       
    }
}