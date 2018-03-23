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
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DataEditor.Model;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using ZXing.Common;
using System.IO;
using System.Xml.Serialization;

namespace DataEditor.View
{
    public partial class AddDevice : DevExpress.XtraEditors.XtraUserControl
    {
        const string GROUP_BY_Added= "1.已添加的";
        const string GROUP_BY_Remain = "2.剩余的";
        const string GROUP_BY_PreTask = "3.任务";
        const string GROUP_BY_Earlier = "4.任务";
        protected FilterItem CurTask;

        #region  测试数据
        private List<TaskDevice> getDevice()
        {
            IEnumerable<TaskDevice>  filter = (IEnumerable<TaskDevice>)Common.getDeviecSorByDevice(CurTask.Type);
            IEnumerable<TaskDevice> added = (IEnumerable<TaskDevice>)Common.getDeviecSorByTask (CurTask.Name);
            string lastname = Common.GetLastTaskName(CurTask.Name);
            string last2name = Common.GetLastTaskName(lastname);
            IEnumerable<TaskDevice> lastadded= (IEnumerable<TaskDevice>)Common.getDeviecSorByTask(lastname,CurTask.Type);
            IEnumerable<TaskDevice> last2added = (IEnumerable<TaskDevice>)Common.getDeviecSorByTask(last2name,CurTask.Type);
            List<TaskDevice> temp = new List<TaskDevice>();
            if (filter.Count() !=0)
            {
                temp = filter.ToList();
                var  remain = temp.RemoveAll(t => added.Any(a => a.ID == t.ID) ||lastadded.Any(a => a.ID == t.ID) ||last2added.Any(a => a.ID == t.ID));
                foreach (TaskDevice item in temp)
                {
                    item.GroupBy = GROUP_BY_Remain;
                }
                        
            }
          
            added = added.SkipWhile(t => string.IsNullOrEmpty(t.ID));
            lastadded = lastadded.SkipWhile(t => string.IsNullOrEmpty(t.ID));
            last2added = last2added.SkipWhile(t => string.IsNullOrEmpty(t.ID));
            foreach (TaskDevice item in added)
            {
                item.GroupBy = GROUP_BY_Added;
            }
            temp.AddRange(added);
            foreach (TaskDevice item in lastadded)
            {
                item.IsChecked = false;
                item.GroupBy = GROUP_BY_PreTask+"  " +lastname ;
            }
            temp.AddRange(lastadded);
            foreach (TaskDevice item in last2added)
            {
                item.IsChecked = false;
                item.GroupBy = GROUP_BY_Earlier + "  " + last2name;
            }
            temp.AddRange(last2added);

            if (temp.Count() == 0)
            {
                TaskDevice d = new TaskDevice();
                d.Photo = Common.getImageByLocal("Err.png");
                d.Company = "清单中没有符合条件的设备";
                d.DeviceType = "错误";
                d.IsChecked = false;
                temp.Add(d);
                
            }
            return temp;
        }
        #endregion
        public AddDevice(FilterItem task)
        {
            InitializeComponent();
            bt_change_cam.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            bt_change_cam.LookAndFeel.UseDefaultLookAndFeel = false;
            bt_change_cam.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            //  bt_selected_tab.LookAndFeel.UseDefaultLookAndFeel = false;
            //   bt_selected_tab.Tag = 0;
            CurTask = task;
            IniTaskDevice();

        }

        private List<TaskDevice> DeviceSource;

        private void IniTaskDevice()
        {
            DeviceSource = getDevice();
            TabDeviceSelect.DataSource = DeviceSource;
        }

        private void TView_ItemClick(object sender, DevExpress.XtraGrid.Views.Tile.TileViewItemClickEventArgs e)
        {
            ((TaskDevice)TView.GetRow(e.Item.RowHandle)).IsChecked = !(bool)(TView.GetRowCellValue(e.Item.RowHandle, tIsChecked));
            TView.RefreshRow(e.Item.RowHandle);
        }


        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.FindForm().DialogResult = DialogResult.Cancel;
            stopscan();
        }

        private void tabbedControlGroup1_SelectedPageChanged(object sender, DevExpress.XtraLayout.LayoutTabPageChangedEventArgs e)
        {
            //  ChangeBtStyle(bt_selected_tab);
            if ((string)e.Page.Tag == "scan")
            {
                getCamList();
                startscan();
            }
            else
            {
                stopscan();
            }
        }

        #region  扫描获取设备信息相关

        private void stopscan()
        {

            CloseVideoSource();

        }
        private void startscan()
        {

            if (DeviceExist)
            {
                //视频捕获设备
                videoSource = new VideoCaptureDevice(videoDevices[camerindex].MonikerString);
                //捕获到新画面时触发
                videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
                //先关一下，下面再打开。避免重复打开的错误
                CloseVideoSource();
                //设置画面大小
                videoSource.DesiredFrameSize = new Size(405, 305);
                //启动视频组件
                videoSource.Start();
                //启动定时解析二维码
                Timer_DecodeImg.Enabled = true;
                //启动绘制视频中的扫描线
                Timer_ViewImg.Enabled = true;
                Timer_DecodeImg.Start();
                Timer_ViewImg.Start();

            }
        }

        FilterInfoCollection videoDevices;

        VideoCaptureDevice videoSource;
        bool DeviceExist = false;
        /// <summary>
        /// 全局变量，记录扫描线距顶端的距离，实现动画效果
        /// </summary>
        int top = 0;
        /// <summary>
        /// 全局变量，保证每一次捕获的图像
        /// </summary>
        Bitmap img = null;

        int camerindex = 0;

        /// <summary>
        /// 利用第3方组件，获取摄像设备
        /// </summary>
        private void getCamList()
        {
            try
            {
                //AForge.Video.DirectShow.FilterInfoCollection 设备枚举类
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                //清空列表框
                if (videoDevices.Count == 0)
                    throw new ApplicationException();
                //全局变量，标示设备摄像头设备是否存在
                DeviceExist = true;
            }
            catch (ApplicationException)
            {
                DeviceExist = false;
                MessageBox.Show("未找到可用的相机设备");

            }

        }

        /// <summary>
        /// 利用计时器，实时解析二维码/条形码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>     
        private void DecodeImg_Tick(object sender, EventArgs e)
        {
            //  simpleLabelItem2.Text = result;
            string imgCode = DeCodeImg(img);
            if (!string.IsNullOrEmpty(imgCode))
            {
                SetResult(imgCode);
            }
        }

        private string DeCodeImg(Bitmap img)
        {
            if (img == null)
                return "";
            #region 将图片转化成 byte数组
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] bt = ms.GetBuffer();
            ms.Close();
            #endregion
            LuminanceSource source = new RGBLuminanceSource(bt, img.Width, img.Height);
            BinaryBitmap bitmap = new BinaryBitmap(new ZXing.Common.HybridBinarizer(source));
            //【1】设置读取条形码规格
            DecodingOptions decodeOption = new DecodingOptions();
            decodeOption.PossibleFormats = new List<BarcodeFormat>() {
               BarcodeFormat.EAN_13,
            };
            //【2】进行读取操作
            BarcodeReader br = new BarcodeReader();
            br.Options = decodeOption;
            Result result2, result1;
            try
            {
                //开始解码
                result2 = new MultiFormatReader().decode(bitmap);
                result1 = br.Decode(img);
            }
            catch
            {
                return "";
            }
            if (result1 != null)
            {

                return result1.Text;
            }
            if (result2 != null)
            {
                return result2.Text;
            }
            return "";
        }

        /// <summary>
        /// 得到的扫描结果 通过设备资源获取仪器信息并更新设备资源的IsChecked属性
        /// </summary>
        /// <param name="result"> 设备的id</param>
        private void SetResult(string result)
        {
            List<TaskDevice> selected = new List<TaskDevice>();//= DeviceSource.Where(t => t.ID == result).ToList();

            DeviceSource.ForEach(t => { if (t.ID == result) { t.IsChecked = true; selected.Add(t); } });
            if (selected.Count == 0)
            {
                TaskDevice d = new TaskDevice();
                d.Photo = Common.getImageByLocal("unkown.png");
                d.Company = "请下载最新的设备清单";
                d.DeviceType = "未知设备";
                d.IsChecked = false;
                selected.Add(d);
            }
            GridCtl2.DataSource = selected;
            GridCtl2.RefreshDataSource();
            TabDeviceSelect.RefreshDataSource();
        }
        /// <summary>
        ///利用计时器 实时显示图像 画面中绘制扫描线，实现动态效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewImg_Tick(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }

            Bitmap img2 = (Bitmap)img.Clone();
            Pen p = new Pen(Color.Red);
            Graphics g = Graphics.FromImage(img2);
            Point p1 = new Point(0, top);
            Point p2 = new Point(pictureEdit1.Width, top);
            g.DrawLine(p, p1, p2);
            g.Dispose();
            top += 2;
            top = top % pictureEdit1.Height;
            pictureEdit1.Image = img2;
        }
        /// <summary>
        /// 第3方组件 实时调用摄像头获取的每一帧图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            img = (Bitmap)eventArgs.Frame.Clone();
        }
        /// <summary>
        /// 关闭摄像头组件
        /// </summary>
        private void CloseVideoSource()
        {
            Timer_DecodeImg.Stop();
            Timer_ViewImg.Stop();
            if (!(videoSource == null))
                if (videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    videoSource = null;
                }
        }
        #endregion
        /// <summary>
        /// 切换前后相机 ，只支持两个相机的切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            stopscan();
            if (videoDevices != null && videoDevices.Count >= 2)
            {
                camerindex = camerindex == 0 ? 1 : 0;
            }
            startscan();
        }


        private bool TrySave()
        {
            EditTask sor = new EditTask(CurTask.Name, CurTask.Type);
            sor.Devices = DeviceSource.Where(t => t.IsChecked && !string.IsNullOrEmpty(t.ID)).ToArray();
            string saveDir = Config.AppConfig.TaskRootPath + "\\" + CurTask.Name;
            if (!Directory.Exists(saveDir))
            {
                try
                {
                    Directory.CreateDirectory(saveDir);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("无法创建任务目录：" + ex.Message);
                    return false;
                }
            }
            string saveFile = saveDir + "\\" + CurTask.Name + ".xml";
            try
            {
               Common. SaveToXml(saveFile, sor);
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("无法保存文件：" + ex.Message);
                return false;
            }

        }
        
  





        private void simpleButton1_Click(object sender, EventArgs e)
        {
            stopscan();
            if (TrySave())
            {
                this.FindForm().DialogResult = DialogResult.OK;
            }
        }
        /// <summary>
        /// 扫一扫 与 选择之间进行切换  无效
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click_1(object sender, EventArgs e)
        {
            SimpleButton bt = ChangeBtStyle(sender);
            TabGroup.SelectedTabPageIndex = (int)bt.Tag;

        }
        /// <summary>
        /// 改变扫一扫 选择 按钮的样式
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        private static SimpleButton ChangeBtStyle(object sender)
        {
            SimpleButton bt = sender as SimpleButton;
            bt.Tag = (int)bt.Tag == 0 ? 1 : 0;
            // bt.Text = (int)bt.Tag == 0 ? "扫一扫" : "返回";
            bt.ImageOptions.Image = (int)bt.Tag == 0 ? Common.getImageByLocal("scan.png") : Common.getImageByLocal("return.png");
            return bt;
        }
    }

    //[POCOViewModel()]
    //public class AddDeviceViewModel : IDocumentContent, ISupportParameter
    //{
    //    object title = "选择你要添加的设备";
    //    public object Title { get { return title; } }
    //    protected IDocumentOwner DocumentOwner { get; private set; }
    //    IDocumentOwner IDocumentContent.DocumentOwner
    //    {
    //        get { return DocumentOwner; }
    //        set { DocumentOwner = value; }
    //    }
    //    public void Close()
    //    {
    //        DocumentOwner.Close(this);
    //    }
    //    object IDocumentContent.Title
    //    {
    //        get
    //        {
    //            return Title;
    //        }
    //    }

    //    protected object Parameter { get; set; }

    //    object ISupportParameter.Parameter
    //    {
    //        get
    //        {
    //            return Parameter;
    //        }

    //        set
    //        {
    //            Parameter = value;
    //        }
    //    }

    //    void IDocumentContent.OnClose(CancelEventArgs e)
    //    {
    //        e.Cancel = false;
    //        //  Messenger.Default.Send(new DestroyOrphanedDocumentsMessage());
    //    }

    //    void IDocumentContent.OnDestroy()
    //    {
    //        // OnDestroy();
    //    }
    //}
}
