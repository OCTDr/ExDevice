using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;

namespace DataEditor
{
    public partial class RootPathSetting : DevExpress.XtraEditors.XtraUserControl
    {
        public RootPathSetting()
        {
            InitializeComponent();
            IniDreivers();
        }  

        private void IniDreivers()
        {
            foreach (DriveInfo d in DriveInfo.GetDrives())
            {
                comboBoxEdit1.Properties.Items.Add(d);
            }
        }
    }
}
