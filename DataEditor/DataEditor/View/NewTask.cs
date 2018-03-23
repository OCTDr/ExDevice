using System.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Utils.MVVM.Services;
using DevExpress.Mvvm.POCO;
using DevExpress.XtraBars.Docking2010.Customization;
using DataEditor.Model;

namespace DataEditor.View
{
    public partial class NewTask : UserControl
    {
        public NewTask()
        {
            InitializeComponent();
            IniDevice();
        }

        List<FilterItem> devicetype = new List<FilterItem>();
        private void IniDevice()
        {
          
            foreach (string type in Enum.GetNames(typeof(Customparamtype.DeviceType)))
            {
                devicetype.Add(new Model.FilterItem(type, 0, type));
            }
            GridCtlFilter.DataSource = devicetype;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.FindForm().DialogResult = DialogResult.Cancel;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
           
            int[] s = TviewFilter.GetSelectedRows();

            FilterItem newtask = s.Length > 0 ? devicetype[s[0]] : null;
            newtask.Name = Common.GetNewTaskName();
            if (newtask != null)
            {
              FlyoutDialog dia = new FlyoutDialog(this.FindForm(), new AddDevice(newtask));
              if(  dia.ShowDialog()==DialogResult.OK)
                {
                    this.FindForm().DialogResult = DialogResult.OK;
                }
              else
                {
                    this.FindForm().DialogResult = DialogResult.Cancel;
                }
            }
            else
            {
                MessageBox.Show("请先选择一种类型");
            }
           
        }

      
    }

   
}
