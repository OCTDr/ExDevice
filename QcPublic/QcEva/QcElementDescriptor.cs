////////////////////////////////////
//   产品级别包装类，主要完成新建的可以修改编码的问题
//  abao++
///////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;

namespace QcPublic
{
    /// <summary>
    ///  QcProductLevel属性包装器,
    ///  例外，可以输入编码
    /// </summary>
    public class QcElementDescriptor : DynamicTypeDescriptor
    {
        static private string[] CheckEntryreadonlyfields = { "创建", "检查项编码", "质量子元素编码", "标准检查项编码" };
        private static Dictionary<string, TypeConverter> checkentryconverter
         = new Dictionary<string, TypeConverter>()
            {
                {"计分方式",new 计分方式Converter()},
                {"结果值枚举",new 结果值枚举Converter()},
                //{"标准检查项编码",new 标准检查项Converter()},
                {"备注",new 检查项备注Converter()}
            };
        
        public QcElementDescriptor(DynamicDataRowObject DynamicObject)
            : base(DynamicObject)
        {
            if(DynamicObject is QcCheckEntry )
            {
                this._readonlyfields = CheckEntryreadonlyfields;
                this._dicconverter  = checkentryconverter;
            }
        }
    }

    public class 计分方式Converter : TypeConverter
    {
        public static string[] 计分方式s = new string[] { "最低分", "带权平均值","合并计分","不计分" };
                public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }


        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
      
            return value;
        }


        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(计分方式s);
        }

    }

    public class 结果值枚举Converter : TypeConverter
    {
        public static string[] 结果值枚举s = new string[] { "Y/N", "R:R0", "M;M0", "A?B?C?D?" };
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }


        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
                 var ds = context.Instance as QcElementDescriptor;
                 if (ds.RowObject is QcCheckEntry)
                 {
                     QcCheckEntry check = ds.RowObject as QcCheckEntry;
                     string v = value.ToString();
                     if (v != check["结果值枚举"])
                     {
                         switch (v)
                         {
                             case "Y/N":
                                 check["限差值"] = "Y=100;N=0";
                                 break;
                             case "A?B?C?D?":
                                 check["限差值"] = "A=42;B=12;C=4;D=1";
                                 break;
                             default:
                                 break;
                         }
                     }
                 }
            return value;
        }


        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {

            return new StandardValuesCollection(结果值枚举s);
        }

    }
        public class 检查项备注Converter : TypeConverter
    {
        public static string[] 检查项备注s = new string[] {"面积总数","要素总数","其它","无参数"};
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }


        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return value;
        }


        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(检查项备注s);
        }
        //
    }
    public class 标准检查项Converter : TypeConverter
    {
        public static string[] 标准检查项s = new string[] { "是","否"};
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }


        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return value;
        }


        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(标准检查项s);
        }
        //
    }
    //public class 标准检查项Converter : TypeConverter
    //{
    //    List<QcCheckEntry> lstSelect = null;
    //           public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }


    //    public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
    //    {
    //        if(lstSelect!=null)
    //        {
    //            var v = lstSelect.FirstOrDefault(t => t.Name == value.ToString());
    //            if (v == null)
    //                return value;
    //            else
    //                return v.Code;
    //        }
    //        return value;
    //    }


    //    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        QcElementDescriptor qu = context.Instance as QcElementDescriptor ;
    //        QcCheckEntry check = qu.RowObject as QcCheckEntry ;
    //        string code=check.Parent.Parent.Parent.Parent.Code+"00";
    //        lstSelect = QcCheckEntry.GetCheckEntry(code).ToList();

    //        return new StandardValuesCollection((from p in lstSelect select p.Name).ToList());
    //    }

    //}
}
