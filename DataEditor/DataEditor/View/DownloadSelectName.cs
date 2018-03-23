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
using DataEditor.Model;
using QcPublic;

namespace DataEditor
{
    public partial class DownloadSelectName : DevExpress.XtraEditors.XtraUserControl
    {
        public DownloadSelectName()
        {
            InitializeComponent();
            iniNames();

        }
        List<FilterItem> users = new List<FilterItem>();
        private void iniNames()
        {

            foreach (QcUser user in QcUser.Users)
            {
                FilterItem item = new FilterItem(user.姓名, 0, user.姓名);
                item.IsChecked = false;
                item.Tag = user;
                users.Add(item);
            }
            GridCtlFilter.DataSource = users;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.FindForm().DialogResult = DialogResult.Cancel;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (tryDownload())
            {
                this.FindForm().DialogResult = DialogResult.OK;
            }

        }
        private bool tryDownload()
        {
            string filename = Common.getNextDownloadFilename();
            int[] s = TviewFilter.GetCheckedRows();
            List<TaskDevice> download = new List<TaskDevice>();
            if (s != null)
            {
                EditTask output = new EditTask("download", "全部");
                foreach (int i in s)
                {
                    FilterItem suser = s.Length > 0 ? users[i] : null;
                    List<QcJob> jobs = null;
                    try
                    {
                        jobs = QcJob.GetMyJob(suser.Tag as QcUser);
                        foreach (QcJob job in jobs)
                        {
                            QcTask task = QcTask.GetTaskByid(job["任务编号"]);
                            foreach (QcCheckData data in QcCheckData.GetCheckData(job))
                            {
                                QcDevice de = QcDevice.GetDeviceByUID(data["设备UID"]);
                                TaskDevice tde = new TaskDevice();
                                tde.ID = data.Code;//利用数据guid进行唯一编号
                                tde.IsChecked = false;
                                tde.JobCode = job.Code;
                                tde.Producer = de["生产厂商"];
                                tde.Model = de["设备型号"];
                                tde.Company = task["委托单位"];
                                tde.DeviceNo = de["设备编号"];
                                tde.EliminateDate = job["作业计划完成时间"];
                                tde.DeviceType = de["设备类型"];
                                tde.Checker = suser.Name;
                                download.Add(tde);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("下载数据错误" + ex.Message);
                        return false;
                    }
                }
                try
                {
                    output.Devices = download.ToArray();
                    string outpath = string.Format("{0}\\{1}\\{2}", Config.AppConfig.TaskRootPath, Config.AppConfig.DownloadDirName, filename);
                    Common.SaveToXml(outpath, output);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("输出文件错误" + ex.Message);
                    return false;
                }
                return true;

            }
            else
            {
                MessageBox.Show("请选择一个人员");
                return false;
            }

        }

        private void TviewFilter_ItemClick(object sender, DevExpress.XtraGrid.Views.Tile.TileViewItemClickEventArgs e)
        {
            if (!IsMulti.Checked)
            {
                users.ForEach(t => t.IsChecked = false);
                GridCtlFilter.RefreshDataSource();
            }

            ((FilterItem)TviewFilter.GetRow(e.Item.RowHandle)).IsChecked = !(bool)(TviewFilter.GetRowCellValue(e.Item.RowHandle, tIsChecked));
            TviewFilter.RefreshRow(e.Item.RowHandle);
        }

        private void IsMulti_CheckedChanged(object sender, EventArgs e)
        {
            if (!IsMulti.Checked)
            {
                users.ForEach(t => t.IsChecked = false);
                int[] s = TviewFilter.GetSelectedRows();
                if (s.Length > 0)
                {
                    users[s[0]].IsChecked = true;
                }
                GridCtlFilter.RefreshDataSource();
            }

        }
    }
}
