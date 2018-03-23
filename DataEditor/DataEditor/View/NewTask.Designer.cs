namespace DataEditor.View
{
    partial class NewTask
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
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraEditors.TableLayout.TableColumnDefinition tableColumnDefinition1 = new DevExpress.XtraEditors.TableLayout.TableColumnDefinition();
            DevExpress.XtraEditors.TableLayout.TableColumnDefinition tableColumnDefinition2 = new DevExpress.XtraEditors.TableLayout.TableColumnDefinition();
            DevExpress.XtraEditors.TableLayout.TableRowDefinition tableRowDefinition1 = new DevExpress.XtraEditors.TableLayout.TableRowDefinition();
            DevExpress.XtraEditors.TableLayout.TableRowDefinition tableRowDefinition2 = new DevExpress.XtraEditors.TableLayout.TableRowDefinition();
            DevExpress.XtraEditors.TableLayout.TableRowDefinition tableRowDefinition3 = new DevExpress.XtraEditors.TableLayout.TableRowDefinition();
            DevExpress.XtraEditors.TableLayout.TableSpan tableSpan1 = new DevExpress.XtraEditors.TableLayout.TableSpan();
            DevExpress.XtraEditors.TableLayout.TableSpan tableSpan2 = new DevExpress.XtraEditors.TableLayout.TableSpan();
            DevExpress.XtraGrid.Views.Tile.TileViewItemElement tileViewItemElement1 = new DevExpress.XtraGrid.Views.Tile.TileViewItemElement();
            DevExpress.XtraGrid.Views.Tile.TileViewItemElement tileViewItemElement2 = new DevExpress.XtraGrid.Views.Tile.TileViewItemElement();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewTask));
            this.tName = new DevExpress.XtraGrid.Columns.TileViewColumn();
            this.tImg = new DevExpress.XtraGrid.Columns.TileViewColumn();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.GridCtlFilter = new DevExpress.XtraGrid.GridControl();
            this.TviewFilter = new DevExpress.XtraGrid.Views.Tile.TileView();
            this.tCount = new DevExpress.XtraGrid.Columns.TileViewColumn();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridCtlFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TviewFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            this.SuspendLayout();
            // 
            // tName
            // 
            this.tName.Caption = "名称";
            this.tName.FieldName = "Name";
            this.tName.Name = "tName";
            this.tName.Visible = true;
            this.tName.VisibleIndex = 0;
            // 
            // tImg
            // 
            this.tImg.Caption = "图片";
            this.tImg.FieldName = "Img";
            this.tImg.Name = "tImg";
            this.tImg.Visible = true;
            this.tImg.VisibleIndex = 2;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(600, 340);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(116, 42);
            this.simpleButton1.StyleController = this.layoutControl1;
            this.simpleButton1.TabIndex = 1;
            this.simpleButton1.Text = "取消";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.GridCtlFilter);
            this.layoutControl1.Controls.Add(this.simpleButton1);
            this.layoutControl1.Controls.Add(this.simpleButton2);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(385, 509, 514, 619);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(728, 394);
            this.layoutControl1.TabIndex = 2;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // GridCtlFilter
            // 
            this.GridCtlFilter.Location = new System.Drawing.Point(12, 12);
            this.GridCtlFilter.MainView = this.TviewFilter;
            this.GridCtlFilter.Name = "GridCtlFilter";
            this.GridCtlFilter.Padding = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.GridCtlFilter.Size = new System.Drawing.Size(704, 324);
            this.GridCtlFilter.TabIndex = 4;
            this.GridCtlFilter.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.TviewFilter});
            // 
            // TviewFilter
            // 
            this.TviewFilter.Appearance.ItemPressed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.TviewFilter.Appearance.ItemPressed.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.TviewFilter.Appearance.ItemPressed.Options.UseBackColor = true;
            this.TviewFilter.Appearance.ItemSelected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.TviewFilter.Appearance.ItemSelected.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.TviewFilter.Appearance.ItemSelected.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.TviewFilter.Appearance.ItemSelected.Options.UseBackColor = true;
            this.TviewFilter.Appearance.ItemSelected.Options.UseBorderColor = true;
            this.TviewFilter.Appearance.ItemSelected.Options.UseFont = true;
            this.TviewFilter.Appearance.ItemSelected.Options.UseForeColor = true;
            this.TviewFilter.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.tName,
            this.tCount,
            this.tImg});
            this.TviewFilter.GridControl = this.GridCtlFilter;
            this.TviewFilter.Name = "TviewFilter";
            this.TviewFilter.OptionsBehavior.AllowSmoothScrolling = true;
            this.TviewFilter.OptionsLayout.StoreAppearance = true;
            this.TviewFilter.OptionsTiles.AllowPressAnimation = false;
            this.TviewFilter.OptionsTiles.ItemSize = new System.Drawing.Size(120, 75);
            this.TviewFilter.OptionsView.ShowViewCaption = true;
            tableColumnDefinition1.Length.Value = 45D;
            tableColumnDefinition2.Length.Value = 45D;
            this.TviewFilter.TileColumns.Add(tableColumnDefinition1);
            this.TviewFilter.TileColumns.Add(tableColumnDefinition2);
            tableRowDefinition1.Length.Value = 49D;
            tableRowDefinition2.Length.Value = 20D;
            tableRowDefinition3.Length.Value = 35D;
            this.TviewFilter.TileRows.Add(tableRowDefinition1);
            this.TviewFilter.TileRows.Add(tableRowDefinition2);
            this.TviewFilter.TileRows.Add(tableRowDefinition3);
            tableSpan1.ColumnSpan = 2;
            tableSpan1.RowIndex = 2;
            tableSpan2.RowSpan = 2;
            this.TviewFilter.TileSpans.Add(tableSpan1);
            this.TviewFilter.TileSpans.Add(tableSpan2);
            tileViewItemElement1.Appearance.Normal.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            tileViewItemElement1.Appearance.Normal.Options.UseFont = true;
            tileViewItemElement1.Column = this.tName;
            tileViewItemElement1.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            tileViewItemElement1.ImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.ZoomInside;
            tileViewItemElement1.RowIndex = 2;
            tileViewItemElement1.Text = "tName";
            tileViewItemElement1.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleLeft;
            tileViewItemElement2.Column = this.tImg;
            tileViewItemElement2.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleLeft;
            tileViewItemElement2.ImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.ZoomInside;
            tileViewItemElement2.Text = "tImg";
            tileViewItemElement2.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleLeft;
            this.TviewFilter.TileTemplate.Add(tileViewItemElement1);
            this.TviewFilter.TileTemplate.Add(tileViewItemElement2);
            // 
            // tCount
            // 
            this.tCount.Caption = "数量";
            this.tCount.FieldName = "Count";
            this.tCount.Name = "tCount";
            this.tCount.Visible = true;
            this.tCount.VisibleIndex = 1;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(493, 340);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(103, 42);
            this.simpleButton2.StyleController = this.layoutControl1;
            this.simpleButton2.TabIndex = 1;
            this.simpleButton2.Text = "确定";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.emptySpaceItem1,
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(728, 394);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 328);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(481, 46);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.simpleButton2;
            this.layoutControlItem1.Location = new System.Drawing.Point(481, 328);
            this.layoutControlItem1.MaxSize = new System.Drawing.Size(0, 46);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(37, 46);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(107, 46);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.simpleButton1;
            this.layoutControlItem2.Location = new System.Drawing.Point(588, 328);
            this.layoutControlItem2.MaxSize = new System.Drawing.Size(0, 46);
            this.layoutControlItem2.MinSize = new System.Drawing.Size(37, 46);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(120, 46);
            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.GridCtlFilter;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(708, 328);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Device_EDMI.png");
            this.imageList1.Images.SetKeyName(1, "Device_GNSS.png");
            this.imageList1.Images.SetKeyName(2, "Device_LEVEL_.png");
            this.imageList1.Images.SetKeyName(3, "Device_TTST.png");
            // 
            // NewTask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Name = "NewTask";
            this.Size = new System.Drawing.Size(728, 394);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridCtlFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TviewFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.ImageList imageList1;
        public DevExpress.XtraGrid.GridControl GridCtlFilter;
        private DevExpress.XtraGrid.Views.Tile.TileView TviewFilter;
        private DevExpress.XtraGrid.Columns.TileViewColumn tName;
        private DevExpress.XtraGrid.Columns.TileViewColumn tCount;
        private DevExpress.XtraGrid.Columns.TileViewColumn tImg;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
    }
}
