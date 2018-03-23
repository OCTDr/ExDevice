using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;

namespace saoma11
{
    public partial class Form1 : Form
    {
        FilterInfoCollection videoDevices;
        VideoCaptureDevice videoSource;
        bool DeviceExist = false;
        /// <summary>
        /// 全局变量，记录扫描线距顶端的距离
        /// </summary>
        int top = 0;
        /// <summary>
        /// 全局变量，保证每一次捕获的图像
        /// </summary>
        Bitmap img = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            if (btn_Start.Text == "开始扫码")
            {
                if (DeviceExist)
                {
                    //视频捕获设备
                    videoSource = new VideoCaptureDevice(videoDevices[comboBox1.SelectedIndex].MonikerString);
                    //捕获到新画面时触发
                    videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
                    //先关一下，下面再打开。避免重复打开的错误
                    CloseVideoSource();
                    //设置画面大小
                    videoSource.DesiredFrameSize = new Size(405, 305);
                    //启动视频组件
                    videoSource.Start();
                    btn_Start.Text = "停止扫码";
                    //启动定时解析二维码
                    Timer_DecodeImg.Enabled = true;
                    //启动绘制视频中的扫描线
                    Timer_ViewImg.Enabled = true;
                    Timer_DecodeImg.Start();
                    Timer_ViewImg.Start(); 
                        

                }
            }
            else
            {
                CloseVideoSource();
            }
        }
        /// <summary>
        /// 解析二维码/条形码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (img == null)
                return;
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
            catch (ReaderException re)
            {
                return;
            }
            if (result1 != null)
                textBox1.Text = result1.Text;
            if (result2 != null)
                textBox1.Text = result2.Text;
        }
        /// <summary>
        /// 画面中绘制扫描线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (img == null)
                return;
            Bitmap img2 = (Bitmap)img.Clone();
            Pen p = new Pen(Color.Red);
            Graphics g = Graphics.FromImage(img2);
            Point p1 = new Point(0, top);
            Point p2 = new Point(pictureBox1.Width, top);
            g.DrawLine(p, p1, p2);
            g.Dispose();
            top += 2;
            top = top % pictureBox1.Height;
            pictureBox1.Image = img2;
        }
        /// <summary>
        /// 获取到设备上的摄像头
        /// </summary>
        private void getCamList()
        {
            try
            {
                //AForge.Video.DirectShow.FilterInfoCollection 设备枚举类
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                //清空列表框
                comboBox1.Items.Clear();
                if (videoDevices.Count == 0)
                    throw new ApplicationException();
                //全局变量，标示设备摄像头设备是否存在
                DeviceExist = true;
                //加入设备
                foreach (FilterInfo device in videoDevices)
                {
                    comboBox1.Items.Add(device.Name);
                }
                //默认选择第一项
                comboBox1.SelectedIndex = 0;
            }
            catch (ApplicationException)
            {
                DeviceExist = false;
                comboBox1.Items.Add("未找到可用设备");
                
            }
        }
        private void CloseVideoSource()
        {
            if (!(videoSource == null))
                if (videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    videoSource = null;
                }
        }
        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            img = (Bitmap)eventArgs.Frame.Clone();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            getCamList();
        }
    }
}
