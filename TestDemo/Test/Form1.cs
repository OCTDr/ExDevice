using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QcPublic;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DbHelper.SetDbServer("Provider=OraOLEDB.Oracle.1;Persist Security Info=TRUE;Data Source=111.231.190.187/IGCESDB;User Id=igces;Password=igces;Pooling=True;Min Pool Size=50;Max Pool=2000");
           
        }

        private void button1_Click(object sender, EventArgs e)
        {

            QcDevice newdev = (this.advPropertyGrid1.SelectedObject as QcDeviceDescriptor).RowObject as QcDevice;
           if( newdev.Update())
           {
               this.advPropertyGrid1.SelectedObject = new QcDeviceDescriptor(newdev);
           }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            QcDevice newdev = new QcDevice();
            newdev["设备编号"] = "S1231";
            newdev["设备型号"] = "JS01";
            newdev["生产厂商"] = "南方";
            this.advPropertyGrid1.SelectedObject = new QcDeviceDescriptor(newdev);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            QcDevice newdev = QcDevice.GetDevices().Last();
            this.advPropertyGrid1.SelectedObject = new QcDeviceDescriptor(newdev);
        }


    }
}
