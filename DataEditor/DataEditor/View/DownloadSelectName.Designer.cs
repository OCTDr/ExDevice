namespace DataEditor
{
    partial class DownloadSelectName
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraEditors.TableLayout.TableColumnDefinition tableColumnDefinition3 = new DevExpress.XtraEditors.TableLayout.TableColumnDefinition();
            DevExpress.XtraEditors.TableLayout.TableColumnDefinition tableColumnDefinition4 = new DevExpress.XtraEditors.TableLayout.TableColumnDefinition();
            DevExpress.XtraEditors.TableLayout.TableRowDefinition tableRowDefinition4 = new DevExpress.XtraEditors.TableLayout.TableRowDefinition();
            DevExpress.XtraEditors.TableLayout.TableRowDefinition tableRowDefinition5 = new DevExpress.XtraEditors.TableLayout.TableRowDefinition();
            DevExpress.XtraEditors.TableLayout.TableRowDefinition tableRowDefinition6 = new DevExpress.XtraEditors.TableLayout.TableRowDefinition();
            DevExpress.XtraEditors.TableLayout.TableSpan tableSpan3 = new DevExpress.XtraEditors.TableLayout.TableSpan();
            DevExpress.XtraEditors.TableLayout.TableSpan tableSpan4 = new DevExpress.XtraEditors.TableLayout.TableSpan();
            DevExpress.XtraGrid.Views.Tile.TileViewItemElement tileViewItemElement3 = new DevExpress.XtraGrid.Views.Tile.TileViewItemElement();
            DevExpress.XtraGrid.Views.Tile.TileViewItemElement tileViewItemElement4 = new DevExpress.XtraGrid.Views.Tile.TileViewItemElement();
            this.tName = new DevExpress.XtraGrid.Columns.TileViewColumn();
            this.tImg = new DevExpress.XtraGrid.Columns.TileViewColumn();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.GridCtlFilter = new DevExpress.XtraGrid.GridControl();
            this.TviewFilter = new DevExpress.XtraGrid.Views.Tile.TileView();
            this.tCount = new DevExpress.XtraGrid.Columns.TileViewColumn();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.IsMulti = new DevExpress.XtraEditors.CheckEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.tIsChecked = new DevExpress.XtraGrid.Columns.TileViewColumn();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridCtlFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TviewFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsMulti.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
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
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.GridCtlFilter);
            this.layoutControl1.Controls.Add(this.simpleButton1);
            this.layoutControl1.Controls.Add(this.simpleButton2);
            this.layoutControl1.Controls.Add(this.IsMulti);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(568, 593, 450, 400);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(728, 435);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // GridCtlFilter
            // 
            this.GridCtlFilter.Location = new System.Drawing.Point(12, 12);
            this.GridCtlFilter.MainView = this.TviewFilter;
            this.GridCtlFilter.Name = "GridCtlFilter";
            this.GridCtlFilter.Padding = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.GridCtlFilter.Size = new System.Drawing.Size(704, 359);
            this.GridCtlFilter.TabIndex = 6;
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
            this.tImg,
            this.tIsChecked});
            this.TviewFilter.ColumnSet.CheckedColumn = this.tIsChecked;
            this.TviewFilter.GridControl = this.GridCtlFilter;
            this.TviewFilter.Name = "TviewFilter";
            this.TviewFilter.OptionsBehavior.AllowSmoothScrolling = true;
            this.TviewFilter.OptionsLayout.StoreAppearance = true;
            this.TviewFilter.OptionsTiles.AllowPressAnimation = false;
            this.TviewFilter.OptionsTiles.ItemSize = new System.Drawing.Size(120, 75);
            this.TviewFilter.OptionsView.ShowViewCaption = true;
            tableColumnDefinition3.Length.Value = 45D;
            tableColumnDefinition4.Length.Value = 45D;
            this.TviewFilter.TileColumns.Add(tableColumnDefinition3);
            this.TviewFilter.TileColumns.Add(tableColumnDefinition4);
            tableRowDefinition4.Length.Value = 49D;
            tableRowDefinition5.Length.Value = 20D;
            tableRowDefinition6.Length.Value = 35D;
            this.TviewFilter.TileRows.Add(tableRowDefinition4);
            this.TviewFilter.TileRows.Add(tableRowDefinition5);
            this.TviewFilter.TileRows.Add(tableRowDefinition6);
            tableSpan3.ColumnSpan = 2;
            tableSpan3.RowIndex = 2;
            tableSpan4.RowSpan = 2;
            this.TviewFilter.TileSpans.Add(tableSpan3);
            this.TviewFilter.TileSpans.Add(tableSpan4);
            tileViewItemElement3.Appearance.Normal.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            tileViewItemElement3.Appearance.Normal.Options.UseFont = true;
            tileViewItemElement3.Column = this.tName;
            tileViewItemElement3.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            tileViewItemElement3.ImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.ZoomInside;
            tileViewItemElement3.RowIndex = 2;
            tileViewItemElement3.Text = "tName";
            tileViewItemElement3.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleLeft;
            tileViewItemElement4.Column = this.tImg;
            tileViewItemElement4.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleLeft;
            tileViewItemElement4.ImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.ZoomInside;
            tileViewItemElement4.Text = "tImg";
            tileViewItemElement4.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleLeft;
            this.TviewFilter.TileTemplate.Add(tileViewItemElement3);
            this.TviewFilter.TileTemplate.Add(tileViewItemElement4);
            this.TviewFilter.ItemClick += new DevExpress.XtraGrid.Views.Tile.TileViewItemClickEventHandler(this.TviewFilter_ItemClick);
            // 
            // tCount
            // 
            this.tCount.Caption = "数量";
            this.tCount.FieldName = "Count";
            this.tCount.Name = "tCount";
            this.tCount.Visible = true;
            this.tCount.VisibleIndex = 1;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(601, 375);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(115, 48);
            this.simpleButton1.StyleController = this.layoutControl1;
            this.simpleButton1.TabIndex = 4;
            this.simpleButton1.Text = "取消";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(475, 375);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(122, 48);
            this.simpleButton2.StyleController = this.layoutControl1;
            this.simpleButton2.TabIndex = 5;
            this.simpleButton2.Text = "确定";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // IsMulti
            // 
            this.IsMulti.Location = new System.Drawing.Point(12, 385);
            this.IsMulti.Name = "IsMulti";
            this.IsMulti.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsMulti.Properties.Appearance.Options.UseFont = true;
            this.IsMulti.Properties.Caption = "允许多人";
            this.IsMulti.Size = new System.Drawing.Size(459, 33);
            this.IsMulti.StyleController = this.layoutControl1;
            this.IsMulti.TabIndex = 7;
            this.IsMulti.CheckedChanged += new System.EventHandler(this.IsMulti_CheckedChanged);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.emptySpaceItem3,
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(728, 435);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // emptySpaceItem3
            // 
            this.emptySpaceItem3.AllowHotTrack = false;
            this.emptySpaceItem3.Location = new System.Drawing.Point(0, 363);
            this.emptySpaceItem3.Name = "emptySpaceItem3";
            this.emptySpaceItem3.Size = new System.Drawing.Size(463, 10);
            this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.simpleButton1;
            this.layoutControlItem1.Location = new System.Drawing.Point(589, 363);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(91, 26);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(119, 52);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.simpleButton2;
            this.layoutControlItem2.Location = new System.Drawing.Point(463, 363);
            this.layoutControlItem2.MinSize = new System.Drawing.Size(91, 26);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(126, 52);
            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.GridCtlFilter;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(708, 363);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.IsMulti;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 373);
            this.layoutControlItem4.MinSize = new System.Drawing.Size(86, 23);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(463, 42);
            this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // tIsChecked
            // 
            this.tIsChecked.Caption = "是否选择";
            this.tIsChecked.FieldName = "IsChecked";
            this.tIsChecked.Name = "tIsChecked";
            this.tIsChecked.Visible = true;
            this.tIsChecked.VisibleIndex = 3;
            // 
            // DownloadSelectName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Name = "DownloadSelectName";
            this.Size = new System.Drawing.Size(728, 435);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridCtlFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TviewFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsMulti.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        public DevExpress.XtraGrid.GridControl GridCtlFilter;
        private DevExpress.XtraGrid.Views.Tile.TileView TviewFilter;
        private DevExpress.XtraGrid.Columns.TileViewColumn tName;
        private DevExpress.XtraGrid.Columns.TileViewColumn tCount;
        private DevExpress.XtraGrid.Columns.TileViewColumn tImg;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.CheckEdit IsMulti;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraGrid.Columns.TileViewColumn tIsChecked;
    }
}
