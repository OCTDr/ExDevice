namespace DataEditor
{
    partial class TaskPage
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.Split_Ctl)).BeginInit();
            this.Split_Ctl.SuspendLayout();
            this.SuspendLayout();
            // 
            // Split_Ctl
            // 
            this.Split_Ctl.Appearance.BackColor = System.Drawing.Color.White;
            this.Split_Ctl.Appearance.Options.UseBackColor = true;
            this.Split_Ctl.Panel1.Controls.Add(this.comboBox1);
            this.Split_Ctl.Panel2.Appearance.BackColor = System.Drawing.Color.White;
            this.Split_Ctl.Panel2.Appearance.Options.UseBackColor = true;
            this.Split_Ctl.Size = new System.Drawing.Size(850, 247);
            this.Split_Ctl.SplitterPosition = 271;
            // 
            // Bar_Tool
            // 
            this.Bar_Tool.AppearanceButton.Hovered.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.Bar_Tool.AppearanceButton.Hovered.FontSizeDelta = -1;
            this.Bar_Tool.AppearanceButton.Hovered.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.Bar_Tool.AppearanceButton.Hovered.Options.UseBackColor = true;
            this.Bar_Tool.AppearanceButton.Hovered.Options.UseFont = true;
            this.Bar_Tool.AppearanceButton.Hovered.Options.UseForeColor = true;
            this.Bar_Tool.AppearanceButton.Normal.FontSizeDelta = -1;
            this.Bar_Tool.AppearanceButton.Normal.Options.UseFont = true;
            this.Bar_Tool.AppearanceButton.Pressed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(159)))), ((int)(((byte)(159)))));
            this.Bar_Tool.AppearanceButton.Pressed.FontSizeDelta = -1;
            this.Bar_Tool.AppearanceButton.Pressed.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(159)))), ((int)(((byte)(159)))));
            this.Bar_Tool.AppearanceButton.Pressed.Options.UseBackColor = true;
            this.Bar_Tool.AppearanceButton.Pressed.Options.UseFont = true;
            this.Bar_Tool.AppearanceButton.Pressed.Options.UseForeColor = true;
            this.Bar_Tool.Location = new System.Drawing.Point(0, 247);
            this.Bar_Tool.Size = new System.Drawing.Size(850, 75);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(8, 8);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 22);
            this.comboBox1.TabIndex = 1;
            // 
            // TaskPage
            // 
            this.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.LookAndFeel.SkinName = "Office 2010 Silver";
            this.LookAndFeel.TouchUIMode = DevExpress.Utils.DefaultBoolean.True;
            this.Name = "TaskPage";
            this.Size = new System.Drawing.Size(850, 322);
            ((System.ComponentModel.ISupportInitialize)(this.Split_Ctl)).EndInit();
            this.Split_Ctl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
    }
}
