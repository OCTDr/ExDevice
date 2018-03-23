using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataEditor;
using System.Drawing;
using System.Xml.Serialization;

namespace DataEditor.Model
{
    [XmlRoot(Namespace = "DataEditor.Model", ElementName = "EditTask", DataType = "string", IsNullable = true)]
    public class EditTask {
        public EditTask()
        {
        }

        public EditTask(string name,string type)
            {
            Name = name;
            Type = type;
            }
        public string Name { get; set; }
        public string Type { get; set; }
        [XmlArrayItem("Device")]
        public TaskDevice[] Devices { get; set; }
    }

    public class FilterItem : TaskDevicePage.Iitemctl
    {

        public FilterItem(int r)
        {
            rnd = new Random(r);
            _name = "2018" + rnd.Next(1, 12).ToString("00") + rnd.Next(1, 28).ToString("00");
            _count = rnd.Next(1, 20);
      
            switch (rnd.Next(0, 4))
            {
                case 0:
                    _img = Common.getImageByLocal("Device_GNSS.png", "64X64");
                    break;
                case 1:
                    _img = Common.getImageByLocal("Device_EDMI.png", "64X64");
                    break;
                case 2:
                    _img = Common.getImageByLocal("Device_LEVEL_.png", "64X64");
                    break;
                case 3:
                    _img = Common.getImageByLocal("Device_TTST.png", "64X64");
                    break;
                default:
                    _img = Common.getImageByLocal("Device_TTST.png", "64X64");
                    break;
            }
        }
        string _name,_type;
        int _count;
        Image _img;
        public FilterItem(string name, int count, string  type)
        {
            _name = name;
            _count = count;
            Type= type;
        }
        Random rnd;
  
        public int Count
        {
            get
            {
                return _count;
            }

            set
            {
                _count = value;
            }
        }

        public Image Img
        {
            get
            {

                return _img;
            }

            set
            {
                _img = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }
        public bool IsChecked{ get; set; }


        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
             
                _img = Common.getImageByLocal(_type+".png", "64X64");
               
              
                //switch (_type)
                //{
                //    case "全部":
                //        _img = Common.getImageByLocal("Device_ALL.png", "64X64");
                //        break;
                //    case "ALL":
                //        _img = Common.getImageByLocal("Device_ALL.png", "64X64");
                //        break;
                //    case "GNSS":
                //        _img = Common.getImageByLocal("Device_GNSS.png","64X64");
                //        break;
                //    case "测距仪":
                //        _img = Common.getImageByLocal("Device_EDMI.png", "64X64");
                //        break;
                //    case "水准标尺":
                //        _img = Common.getImageByLocal("Device_LEVEL_.png", "64X64");
                //        break;
                //    case "全站仪":
                //        _img = Common.getImageByLocal("Device_TTST.png", "64X64");
                //        break;
                //    case "Err":
                //        _img = Common.getImageByLocal("ERR.png", "64X64");
                //        break;
                //    case "ERR":
                //        _img = Common.getImageByLocal("ERR.png", "64X64");
                //        break;
                //    default:
                //        _img = Common.getImageByLocal("Device_TTST.png", "64X64");
                //        break;
              // };
            }
        }

        public object Tag { get; set; }

    }
    [XmlRoot("Device", Namespace = " DataEditor.Model", IsNullable = false)]
    public class TaskDevice
    {
        [XmlAttribute("ID")]
        public string ID
        {
            get; set;
        }
        /// <summary>
        /// 设备编号
        /// </summary>
        [XmlAttribute("DeviceNo")] 
        public string DeviceNo
        {
            get; set;
        }
        /// <summary>
        /// 设备类型
        /// </summary>
        [XmlAttribute("DeviceType")]
        public string  DeviceType
        {
            get; set;
        }
        /// <summary>
        /// 型号
        /// </summary>
        [XmlAttribute("Model")]
        public string Model
        {
            get; set;
        }
        /// <summary>
        /// 生产厂商
        /// </summary>
        [XmlAttribute("Producer")]
        public string Producer
        {
            get; set;
        }
        /// <summary>
        /// 送检单位
        /// </summary>
        [XmlAttribute("Company")]
        public string Company
        {
            get; set;
        }
        /// <summary>
        /// 计划完成实际
        /// </summary>
        [XmlAttribute("EliminateDate")]
        public string EliminateDate
        { get; set; }
        /// <summary>
        /// 作业编号
        /// </summary>
        [XmlAttribute("JobCode")]
        public string JobCode
        { get; set; }

        [XmlIgnore]
        public System.Drawing.Image Photo
        {
            get;set;
        }
        [XmlAttribute("IsChecked")]
        public Boolean IsChecked
         { get; set; }

        [XmlAttribute("GroupBy")]
        public string GroupBy { get; set; }
        private string _checker;
        [XmlAttribute("Checker")]
        public string Checker { get { return _checker; } set
            {
                _checker = value;
                Photo = Common.getImageByLocal(_checker + ".png","PHOTO", "UnknownPhoto");
            }
        }

    }


}
