namespace tiaoxinama
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_BuildLogoTwo = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtStr = new System.Windows.Forms.TextBox();
            this.txt_PicPath = new System.Windows.Forms.TextBox();
            this.btnOpenImage = new System.Windows.Forms.Button();
            this.btn_BuildOne = new System.Windows.Forms.Button();
            this.btn_BuildTwo = new System.Windows.Forms.Button();
            this.btn_ReadOne = new System.Windows.Forms.Button();
            this.btn_ReadTwo = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_BuildLogoTwo
            // 
            this.btn_BuildLogoTwo.Location = new System.Drawing.Point(541, 107);
            this.btn_BuildLogoTwo.Name = "btn_BuildLogoTwo";
            this.btn_BuildLogoTwo.Size = new System.Drawing.Size(145, 30);
            this.btn_BuildLogoTwo.TabIndex = 0;
            this.btn_BuildLogoTwo.Text = "生成带logo二维码";
            this.btn_BuildLogoTwo.UseVisualStyleBackColor = true;
            this.btn_BuildLogoTwo.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(18, 107);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(331, 233);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "条码内容：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "图片路径：";
            // 
            // txtStr
            // 
            this.txtStr.Location = new System.Drawing.Point(100, 6);
            this.txtStr.Name = "txtStr";
            this.txtStr.Size = new System.Drawing.Size(320, 25);
            this.txtStr.TabIndex = 5;
            this.txtStr.Text = "9787302380979";
            // 
            // txt_PicPath
            // 
            this.txt_PicPath.Location = new System.Drawing.Point(100, 37);
            this.txt_PicPath.Name = "txt_PicPath";
            this.txt_PicPath.Size = new System.Drawing.Size(228, 25);
            this.txt_PicPath.TabIndex = 6;
            // 
            // btnOpenImage
            // 
            this.btnOpenImage.Location = new System.Drawing.Point(334, 37);
            this.btnOpenImage.Name = "btnOpenImage";
            this.btnOpenImage.Size = new System.Drawing.Size(91, 29);
            this.btnOpenImage.TabIndex = 7;
            this.btnOpenImage.Text = "打开图片";
            this.btnOpenImage.UseVisualStyleBackColor = true;
            this.btnOpenImage.Click += new System.EventHandler(this.btnOpenImage_Click);
            // 
            // btn_BuildOne
            // 
            this.btn_BuildOne.Location = new System.Drawing.Point(445, 2);
            this.btn_BuildOne.Name = "btn_BuildOne";
            this.btn_BuildOne.Size = new System.Drawing.Size(102, 29);
            this.btn_BuildOne.TabIndex = 7;
            this.btn_BuildOne.Text = "生成条形码";
            this.btn_BuildOne.UseVisualStyleBackColor = true;
            this.btn_BuildOne.Click += new System.EventHandler(this.btn_BuildOne_Click);
            // 
            // btn_BuildTwo
            // 
            this.btn_BuildTwo.Location = new System.Drawing.Point(577, 2);
            this.btn_BuildTwo.Name = "btn_BuildTwo";
            this.btn_BuildTwo.Size = new System.Drawing.Size(109, 29);
            this.btn_BuildTwo.TabIndex = 7;
            this.btn_BuildTwo.Text = "生成二维码";
            this.btn_BuildTwo.UseVisualStyleBackColor = true;
            this.btn_BuildTwo.Click += new System.EventHandler(this.btn_BuildTwo_Click);
            // 
            // btn_ReadOne
            // 
            this.btn_ReadOne.Location = new System.Drawing.Point(445, 37);
            this.btn_ReadOne.Name = "btn_ReadOne";
            this.btn_ReadOne.Size = new System.Drawing.Size(102, 29);
            this.btn_ReadOne.TabIndex = 7;
            this.btn_ReadOne.Text = "读取一维码";
            this.btn_ReadOne.UseVisualStyleBackColor = true;
            this.btn_ReadOne.Click += new System.EventHandler(this.btn_ReadOne_Click);
            // 
            // btn_ReadTwo
            // 
            this.btn_ReadTwo.Location = new System.Drawing.Point(577, 37);
            this.btn_ReadTwo.Name = "btn_ReadTwo";
            this.btn_ReadTwo.Size = new System.Drawing.Size(109, 29);
            this.btn_ReadTwo.TabIndex = 7;
            this.btn_ReadTwo.Text = "读取二维码";
            this.btn_ReadTwo.UseVisualStyleBackColor = true;
            this.btn_ReadTwo.Click += new System.EventHandler(this.btn_ReadTwo_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "图片：";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 352);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_ReadTwo);
            this.Controls.Add(this.btn_ReadOne);
            this.Controls.Add(this.btn_BuildTwo);
            this.Controls.Add(this.btn_BuildOne);
            this.Controls.Add(this.btnOpenImage);
            this.Controls.Add(this.txt_PicPath);
            this.Controls.Add(this.txtStr);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btn_BuildLogoTwo);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_BuildLogoTwo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtStr;
        private System.Windows.Forms.TextBox txt_PicPath;
        private System.Windows.Forms.Button btnOpenImage;
        private System.Windows.Forms.Button btn_BuildOne;
        private System.Windows.Forms.Button btn_BuildTwo;
        private System.Windows.Forms.Button btn_ReadOne;
        private System.Windows.Forms.Button btn_ReadTwo;
        private System.Windows.Forms.Label label3;
    }
}

