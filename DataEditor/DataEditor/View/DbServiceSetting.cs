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
using QcPublic;

namespace DataEditor
{
    public partial class DbServiceSetting : DevExpress.XtraEditors.XtraUserControl
    {
      
        public DbServiceSetting()
        {
            InitializeComponent();
            textEdit2.Properties.PasswordChar = '*';
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.FindForm().DialogResult = DialogResult.Cancel;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            
       
           if( DbHelper.Check(string.Format(Config.AppConfig.DbCon, textEdit1.Text, textEdit2.Text)))
            {
                Config.AppConfig.ConfigIni.Service.IP = textEdit1.Text;
                Config.AppConfig.ConfigIni.Service.PW = QcEncrypt.Encode(textEdit2.Text);
                Config.AppConfig.ConfigIni.SaveToFile();
                this.FindForm().DialogResult = DialogResult.OK;
            }
            else
            {
                simpleLabelItem1.Text = "服务器连接不成功";
            }           
        }
    }
}
