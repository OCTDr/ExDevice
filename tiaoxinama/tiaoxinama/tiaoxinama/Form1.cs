using BarcodeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace tiaoxinama
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1.设置QR二维码的规格
            QrCodeEncodingOptions qrEncodeOption = new QrCodeEncodingOptions();
            qrEncodeOption.CharacterSet = "UTF-8"; // 设置编码格式，否则读取'中文'乱码
            qrEncodeOption.Height = 200;
            qrEncodeOption.Width = 200;
            qrEncodeOption.Margin = 1; // 设置周围空白边距

            // 2.生成条形码图片
            BarcodeWriter wr = new BarcodeWriter();
            wr.Format = BarcodeFormat.QR_CODE; // 二维码
            wr.Options = qrEncodeOption;
            if (this.txtStr.Text == "")
            {
                MessageBox.Show("字符串不能为空");
                return;
            }
            Bitmap img = wr.Write(this.txtStr.Text);

            // 3.在二维码的Bitmap对象上绘制logo图片
            Bitmap logoImg = Bitmap.FromFile(Application.StartupPath+@"\logo.jpg") as Bitmap;
            Graphics g = Graphics.FromImage(img);
            Rectangle logoRec = new Rectangle(); // 设置logo图片的大小和绘制位置
            logoRec.Width = img.Width / 6;
            logoRec.Height = img.Height / 6;
            logoRec.X = img.Width / 2 - logoRec.Width / 2; // 中心点
            logoRec.Y = img.Height / 2 - logoRec.Height / 2;
            g.DrawImage(logoImg, logoRec);

            // 4.保存绘制后的图片
            string filePath = @"C:\Users\dell\Desktop\图片\" + this.txtStr.Text + ".jpg";
            img.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);

            //5.读取保存的图片
            this.txt_PicPath.Text = filePath;
            this.pictureBox1.Image = img;
            MessageBox.Show("保存成功:" + filePath);
        }

        private void btn_BuildOne_Click(object sender, EventArgs e)
        {
            //设置条形码规格
            EncodingOptions encodeOption = new EncodingOptions();
            encodeOption.Height = 130;//高
            encodeOption.Width = 240;//宽

            //生成二维码图片并保存
            BarcodeWriter wr = new BarcodeWriter();
            wr.Options = encodeOption;
            wr.Format = BarcodeFormat.EAN_13;
            if (this.txtStr.Text == "")
            {
                MessageBox.Show("字符串不能为空");
                return;
            }
            Bitmap img = wr.Write(this.txtStr.Text.Trim());
            string filePath = @"C:\Users\dell\Desktop\图片\" + this.txtStr.Text.Trim()+".jpg";
            img.Save(filePath, ImageFormat.Jpeg);

            //读取保存的图片
            this.txt_PicPath.Text = filePath;
            this.pictureBox1.Image = img;
            MessageBox.Show("保存成功:"+filePath);
        }

        private void btn_BuildTwo_Click(object sender, EventArgs e)
        {
            //设置二维码的规格
            QrCodeEncodingOptions qrEncodeOption = new QrCodeEncodingOptions();
            qrEncodeOption.CharacterSet = "UTF-8";
            qrEncodeOption.Width = 200;
            qrEncodeOption.Height = 200;
            qrEncodeOption.Margin = 1;//设置周围空白边距

            //生成二维码图片并保存
            BarcodeWriter wr = new BarcodeWriter();
            wr.Format = BarcodeFormat.QR_CODE;//二维码格式
            wr.Options = qrEncodeOption;
            if (this.txtStr.Text == "")
            {
                MessageBox.Show("字符串不能为空");
                return;
            }
            Bitmap img = wr.Write(this.txtStr.Text);
            string filePath = @"C:\Users\dell\Desktop\图片\" + this.txtStr.Text.Trim() + ".jpg";
            img.Save(filePath, ImageFormat.Jpeg);

            //读取保存的图片
            this.txt_PicPath.Text = filePath;
            this.pictureBox1.Image = img;
            MessageBox.Show("保存成功:" + filePath);
        }

        private void btn_ReadOne_Click(object sender, EventArgs e)
        {
            //【1】设置读取条形码规格
            DecodingOptions decodeOption = new DecodingOptions();
            decodeOption.PossibleFormats = new List<BarcodeFormat>() {
               BarcodeFormat.EAN_13,
            };

            //【2】进行读取操作
            BarcodeReader br = new BarcodeReader();
            br.Options = decodeOption;
            Result rs = br.Decode(this.pictureBox1.Image as Bitmap);
            if (rs == null)
                MessageBox.Show("读取失败");
            else
                MessageBox.Show("读取成功，内容为："+rs.Text);
        }

        private void btn_ReadTwo_Click(object sender, EventArgs e)
        {
            // 1.设置读取二维码规格
            DecodingOptions decodeOption = new DecodingOptions();
            decodeOption.PossibleFormats = new List<BarcodeFormat>() {
               BarcodeFormat.QR_CODE,
           };
            //2.进行读取操作
            BarcodeReader br = new BarcodeReader();
            br.Options = decodeOption;
            Result rs = br.Decode(this.pictureBox1.Image as Bitmap);
            if (rs == null)
                MessageBox.Show("读取失败");
            else
                MessageBox.Show("读取成功，内容为：" + rs.Text);
        }

        private void btnOpenImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            fileDialog.Filter = "图形文件(*.jpg)|*.jpg";
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = fileDialog.FileName;
                this.pictureBox1.Image = Bitmap.FromFile(filePath);
            }
        }
    }
}
