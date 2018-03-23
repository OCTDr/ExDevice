using System;
using System.IO;
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

namespace DataEditor
{
    public partial class TaskDevicePage : DevExpress.XtraEditors.XtraUserControl
    {
        protected string _Model = "TASK";//TASK 模式。DEVICE 模式
        private int _selectedindex;
        public IList<FilterItem> FilterItems { get; set; }
        public IList<TaskDevice> TaskDevices { get; set; }
        protected TaskDevicePage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="skinName">默认 HybridApp </param>
        /// <param name="type"> TASK 模式。DEVICE 模式</param>
        public TaskDevicePage(string skinName = "HybridApp",string Model = "TASK")
        {
            _Model = Model;
            InitializeComponent();
            this.LookAndFeel.SetSkinStyle(skinName);
        }    
        internal void IniFilterItem(IList<FilterItem> aLltask)
        {
            FilterItems = aLltask;
            GridCtlFilter.DataSource = aLltask;
            if (aLltask.Count > 0)
            {
                TviewFilter.SelectRow(0);
                _selectedindex = 0;
                SelectedItem = aLltask[0];
                TviewFilter.ViewCaption = SelectedItem.Name;
            }
        }
    
        public void IniDeviceItem(IList<TaskDevice> devices)
        {
            TaskDevices = devices;
            GridCtl.DataSource = devices;

        }
        private FilterItem _SelectedItem;
        public FilterItem SelectedItem {
            get
            {
                return _SelectedItem;
            }
            set {
                _SelectedItem = value;
                loaddevice();
            }
        }

        public interface Iitemctl
        {
            string Name { get; set; }
            Image Img { get; set; }
            int Count { get; set; }

        }

        private void Bar_Tool_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            DevExpress.XtraBars.Docking2010.WindowsUIButton b = e.Button as DevExpress.XtraBars.Docking2010.WindowsUIButton;
            if (b != null) {
                if (b.Caption == "平铺")
                {
                    GridCtl.MainView = TView;
                }
                else if (b.Caption == "列表")

                {
                    GridCtl.MainView = GridView;
                }
            }
        }

        private void GridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle > -1)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
            //e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //if (e.Info.IsRowIndicator)
            //{
            //    if (e.RowHandle >= 0)
            //    {
            //        e.Info.DisplayText = (e.RowHandle + 1).ToString();
            //    }
            //    else if (e.RowHandle < 0 && e.RowHandle > -1000)
            //    {
            //       // e.Info.Appearance.BackColor = System.Drawing.Color.AntiqueWhite;
            //        e.Info.DisplayText = e.RowHandle.ToString();
            //    }
            //}
        }

       private void TviewFilter_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            _selectedindex = e.ControllerRow;
            SelectedItem = TviewFilter.GetRow(e.ControllerRow) as FilterItem;
            TviewFilter.ViewCaption = SelectedItem.Name;
        }

        private void TviewFilter_ItemPress(object sender, DevExpress.XtraGrid.Views.Tile.TileViewItemClickEventArgs e)
        {
            _selectedindex = e.Item.RowHandle;
            SelectedItem = TviewFilter.GetRow(e.Item.RowHandle) as FilterItem;
            TviewFilter.ViewCaption = SelectedItem.Name;
        }
        private void loaddevice()
        {
            object sor = null;
            switch (_Model)
            {
                case "TASK":                   
                    sor =Common. getDeviecSorByTask(_SelectedItem.Name );              
                    break;
                case "DEVICE":
                    sor = Common.getDeviecSorByDevice(_SelectedItem.Type);
                    break;

            }
            GridCtl.DataSource = sor;
            FilterItems[_selectedindex].Count = ((IEnumerable<TaskDevice>)sor).Where(t=>!string.IsNullOrEmpty(t.ID)).Count();
            GridCtlFilter.RefreshDataSource();
            GridCtl.RefreshDataSource();

        }
        public  void RefreshSelected()

        {
            loaddevice();
        }
     
    }
}
