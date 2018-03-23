namespace DataEditor
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraEditors.TileItemElement tileItemElement1 = new DevExpress.XtraEditors.TileItemElement();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            DevExpress.XtraEditors.TileItemElement tileItemElement2 = new DevExpress.XtraEditors.TileItemElement();
            this.NaviTitle = new DevExpress.XtraBars.Navigation.TileNavPane();
            this.navButton2 = new DevExpress.XtraBars.Navigation.NavButton();
            this.bt_form_min = new DevExpress.XtraBars.Navigation.NavButton();
            this.bt_form_max = new DevExpress.XtraBars.Navigation.NavButton();
            this.bt_form_close = new DevExpress.XtraBars.Navigation.NavButton();
            this.Frame_Pages = new DevExpress.XtraBars.Navigation.NavigationFrame();
            this.Page_Task = new DevExpress.XtraBars.Navigation.NavigationPage();
            this.Page_Device = new DevExpress.XtraBars.Navigation.NavigationPage();
            this.Bar_Group_A = new DevExpress.XtraBars.Navigation.TileBarGroup();
            this.BarItem_Device = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.BarItem_Task = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.Bar_Funtons = new DevExpress.XtraBars.Navigation.TileBar();
            ((System.ComponentModel.ISupportInitialize)(this.NaviTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Frame_Pages)).BeginInit();
            this.Frame_Pages.SuspendLayout();
            this.SuspendLayout();
            // 
            // NaviTitle
            // 
            this.NaviTitle.BackColor = System.Drawing.Color.Transparent;
            this.NaviTitle.Buttons.Add(this.navButton2);
            this.NaviTitle.Buttons.Add(this.bt_form_min);
            this.NaviTitle.Buttons.Add(this.bt_form_max);
            this.NaviTitle.Buttons.Add(this.bt_form_close);
            // 
            // Nav_Title
            // 
            this.NaviTitle.DefaultCategory.Name = "Nav_Title";
            this.NaviTitle.DefaultCategory.OwnerCollection = null;
            // 
            // 
            // 
            this.NaviTitle.DefaultCategory.Tile.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            this.NaviTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.NaviTitle.Location = new System.Drawing.Point(0, 0);
            this.NaviTitle.Name = "NaviTitle";
            this.NaviTitle.Size = new System.Drawing.Size(900, 55);
            this.NaviTitle.TabIndex = 0;
            this.NaviTitle.Text = "tileNavPane1";
            this.NaviTitle.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tileNavPane1_MouseDoubleClick);
            this.NaviTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tileNavPane1_MouseDown);
            this.NaviTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tileNavPane1_MouseMove);
            // 
            // navButton2
            // 
            this.navButton2.Alignment = DevExpress.XtraBars.Navigation.NavButtonAlignment.Left;
            this.navButton2.Caption = "Device Data Editor";
            this.navButton2.Name = "navButton2";
            // 
            // bt_form_min
            // 
            this.bt_form_min.Alignment = DevExpress.XtraBars.Navigation.NavButtonAlignment.Right;
            this.bt_form_min.Caption = "—";
            this.bt_form_min.Name = "bt_form_min";
            this.bt_form_min.ElementClick += new DevExpress.XtraBars.Navigation.NavElementClickEventHandler(this.Form_BT_ElementClick);
            // 
            // bt_form_max
            // 
            this.bt_form_max.Alignment = DevExpress.XtraBars.Navigation.NavButtonAlignment.Right;
            this.bt_form_max.Caption = "口";
            this.bt_form_max.Name = "bt_form_max";
            this.bt_form_max.ElementClick += new DevExpress.XtraBars.Navigation.NavElementClickEventHandler(this.Form_BT_ElementClick);
            // 
            // bt_form_close
            // 
            this.bt_form_close.Alignment = DevExpress.XtraBars.Navigation.NavButtonAlignment.Right;
            this.bt_form_close.Caption = "X";
            this.bt_form_close.Name = "bt_form_close";
            this.bt_form_close.ElementClick += new DevExpress.XtraBars.Navigation.NavElementClickEventHandler(this.Form_BT_ElementClick);
            // 
            // Frame_Pages
            // 
            this.Frame_Pages.Controls.Add(this.Page_Task);
            this.Frame_Pages.Controls.Add(this.Page_Device);
            this.Frame_Pages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Frame_Pages.Location = new System.Drawing.Point(0, 165);
            this.Frame_Pages.Name = "Frame_Pages";
            this.Frame_Pages.Pages.AddRange(new DevExpress.XtraBars.Navigation.NavigationPageBase[] {
            this.Page_Task,
            this.Page_Device});
            this.Frame_Pages.SelectedPage = this.Page_Task;
            this.Frame_Pages.Size = new System.Drawing.Size(900, 348);
            this.Frame_Pages.TabIndex = 2;
            this.Frame_Pages.Text = "navigationFrame1";
            // 
            // Page_Task
            // 
            this.Page_Task.Caption = "Page_Task";
            this.Page_Task.Name = "Page_Task";
            this.Page_Task.Size = new System.Drawing.Size(900, 348);
            // 
            // Page_Device
            // 
            this.Page_Device.Caption = "Page_Device";
            this.Page_Device.ControlName = "";
            this.Page_Device.Name = "Page_Device";
            this.Page_Device.Size = new System.Drawing.Size(900, 348);
            // 
            // Bar_Group_A
            // 
            this.Bar_Group_A.Items.Add(this.BarItem_Device);
            this.Bar_Group_A.Items.Add(this.BarItem_Task);
            this.Bar_Group_A.Name = "Bar_Group_A";
            // 
            // BarItem_Device
            // 
            this.BarItem_Device.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement1.Image = ((System.Drawing.Image)(resources.GetObject("tileItemElement1.Image")));
            tileItemElement1.Text = "任务";
            this.BarItem_Device.Elements.Add(tileItemElement1);
            this.BarItem_Device.Id = 1;
            this.BarItem_Device.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.BarItem_Device.Name = "BarItem_Device";
            // 
            // BarItem_Task
            // 
            this.BarItem_Task.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement2.Image = ((System.Drawing.Image)(resources.GetObject("tileItemElement2.Image")));
            tileItemElement2.Text = "设备清单";
            this.BarItem_Task.Elements.Add(tileItemElement2);
            this.BarItem_Task.Id = 0;
            this.BarItem_Task.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.BarItem_Task.Name = "BarItem_Task";
            // 
            // Bar_Funtons
            // 
            this.Bar_Funtons.AllowDrag = false;
            this.Bar_Funtons.AllowSelectedItem = true;
            this.Bar_Funtons.AppearanceGroupText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.Bar_Funtons.AppearanceGroupText.Options.UseForeColor = true;
            this.Bar_Funtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.Bar_Funtons.Cursor = System.Windows.Forms.Cursors.Default;
            this.Bar_Funtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.Bar_Funtons.DropDownButtonWidth = 30;
            this.Bar_Funtons.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            this.Bar_Funtons.Groups.Add(this.Bar_Group_A);
            this.Bar_Funtons.HorizontalContentAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Bar_Funtons.IndentBetweenGroups = 10;
            this.Bar_Funtons.IndentBetweenItems = 10;
            this.Bar_Funtons.ItemCheckMode = DevExpress.XtraEditors.TileItemCheckMode.Multiple;
            this.Bar_Funtons.ItemPadding = new System.Windows.Forms.Padding(8, 6, 12, 6);
            this.Bar_Funtons.Location = new System.Drawing.Point(0, 55);
            this.Bar_Funtons.MaxId = 2;
            this.Bar_Funtons.Name = "Bar_Funtons";
            this.Bar_Funtons.Padding = new System.Windows.Forms.Padding(22, 7, 22, 20);
            this.Bar_Funtons.ScrollMode = DevExpress.XtraEditors.TileControlScrollMode.ScrollButtons;
            this.Bar_Funtons.SelectionBorderWidth = 2;
            this.Bar_Funtons.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.Bar_Funtons.SelectionColorMode = DevExpress.XtraBars.Navigation.SelectionColorMode.UseItemBackColor;
            this.Bar_Funtons.Size = new System.Drawing.Size(900, 110);
            this.Bar_Funtons.TabIndex = 1;
            this.Bar_Funtons.Text = "tileBar1";
            this.Bar_Funtons.VerticalContentAlignment = DevExpress.Utils.VertAlignment.Bottom;
            this.Bar_Funtons.WideTileWidth = 150;
            this.Bar_Funtons.SelectedItemChanged += new DevExpress.XtraEditors.TileItemClickEventHandler(this.Bar_Funtons_SelectedItemChanged);
            // 
            // MainForm
            // 
            this.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 513);
            this.ControlBox = false;
            this.Controls.Add(this.Frame_Pages);
            this.Controls.Add(this.Bar_Funtons);
            this.Controls.Add(this.NaviTitle);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.None;
            this.Name = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.NaviTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Frame_Pages)).EndInit();
            this.Frame_Pages.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraBars.Navigation.TileNavPane NaviTitle;
        private DevExpress.XtraBars.Navigation.NavButton bt_form_min;
        private DevExpress.XtraBars.Navigation.NavButton bt_form_close;
        private DevExpress.XtraBars.Navigation.NavButton bt_form_max;
        private DevExpress.XtraBars.Navigation.NavigationFrame Frame_Pages;
        private DevExpress.XtraBars.Navigation.NavigationPage Page_Task;
        private DevExpress.XtraBars.Navigation.NavigationPage Page_Device;
        private DevExpress.XtraBars.Navigation.TileBarGroup Bar_Group_A;
        private DevExpress.XtraBars.Navigation.TileBarItem BarItem_Task;
        private DevExpress.XtraBars.Navigation.TileBarItem BarItem_Device;
        private DevExpress.XtraBars.Navigation.TileBar Bar_Funtons;
        private DevExpress.XtraBars.Navigation.NavButton navButton2;
    }
}