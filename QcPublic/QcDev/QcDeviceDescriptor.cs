using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Customparamtype;
using System.ComponentModel;
using System.Data;
namespace QcPublic
{
  public   class QcDeviceDescriptor : DynamicTypeDescriptor
    {
        static public List<string> 设备类型s = Enum.GetNames(typeof(Customparamtype.DeviceType)).ToList();
        static private string[] readonlyfields = { "设备UID" };
        static private Dictionary<string, string> catetory
            = new Dictionary<string, string>()
            {
                 {"使用单位","联系信息"}
                ,{"生产日期","基本信息"}
                ,{"生产厂商","基本信息"}
                ,{"设备类型","基本信息"}
                ,{"设备编号","基本信息"}
                ,{"设备型号","基本信息"}
                ,{"设备UID","自动属性"}
            };
        static private Dictionary<string, TypeConverter> converter
                = new Dictionary<string, TypeConverter>()
                {
        {"设备类型",new QcTypeConverter(设备类型s)}
        ,{"使用单位",new 使用单位Converter()}
                };

        public QcDeviceDescriptor(DynamicDataRowObject DynamicObject)
            : base(DynamicObject
            , readonlyfields
            , catetory
            , converter
            )
        {

        }

    }
    public class 使用单位Converter : QcTypeConverter<QcCompany, QcCompany>
    {
        static object ds = QcCompany.GetCompanys();

        public static IEnumerable<QcCompany> 使用单位s
        {
            get
            {
                return ds as IEnumerable<QcCompany>;

            }
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            QcCompany pt = null;
            if (value is string)
            {
                pt = 使用单位s.FirstOrDefault(t => t.Code  == value.ToString());
            }
            else
                pt = value as QcCompany;
            if (pt != null)
                return pt.Name;
            else
                return "";
            // return base.ConvertTo(context, culture, v, destinationType);            
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            //return base.ConvertFrom(context, culture, value);


            string v = value.ToString();
            QcCompany pt = 使用单位s.FirstOrDefault(t => t.Name == v);
            //if (v.IndexOf('|') > 0)
            //{
            //    v = v.Split('|')[0];
            //}          
            return pt.Code;
        }

        public 使用单位Converter()
            : base(null,
                t => t,
                () => 使用单位s) { }
    }

}
