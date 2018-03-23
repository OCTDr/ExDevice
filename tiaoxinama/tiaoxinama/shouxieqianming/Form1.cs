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

namespace shouxieqianming
{
    public partial class Form1 : Form
    {
        //记录直线或曲线的对象
        private System.Drawing.Drawing2D.GraphicsPath mousePath = new System.Drawing.Drawing2D.GraphicsPath();
        //画笔的透明度
        private int myAlpha = 100;
        //画笔颜色对象
        private Color myColor = new Color();
        //画笔宽度
        private int myPenWidth = 3;
        //签名的图片对象
        public Bitmap saveBitMap;
        private string filePath;
        public Form1()
        {
            InitializeComponent();
            filePath = Application.StartupPath + @"\image";
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    mousePath.AddLine(e.X,e.Y,e.X,e.Y);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mousePath.StartFigure();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            myPenWidth =Convert.ToInt32(this.numericUpDown1.Value);
            try
            {
                myColor = Color.Black;
                myAlpha = 255;
                Pen currentPen = new Pen(Color.FromArgb(myAlpha, myColor), myPenWidth);
                e.Graphics.DrawPath(currentPen, mousePath);
            }
            catch
            { }
        }

        private void Btn_Clear_Click(object sender, EventArgs e)
        {
            pictureBox1.CreateGraphics().Clear(Color.White);
            mousePath.Reset();
        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {
            saveBitMap = new Bitmap(pictureBox1.Width,pictureBox1.Height);
            pictureBox1.DrawToBitmap(saveBitMap,new Rectangle(0,0,pictureBox1.Width,pictureBox1.Height));
            saveBitMap.Save(@"C:\Users\dell\Desktop\wkq.jpg",ImageFormat.Jpeg);
            this.DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserService.UserClient client = new UserService.UserClient();
            string msg= client.ShowName("王康球");
            MessageBox.Show(msg);
        }
    }
}
