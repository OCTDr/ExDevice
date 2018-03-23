using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace QcPublic
{
   public  class QcCheckProjectDescriptor : DynamicTypeDescriptor
    {
        static public List<string> 修改权限s = new List<string> { "个人私有", "完全公开", "规则公开"};
        private static string[] readonlyfields = { "编码", "创建", "编号", "ID", "修改人", "修改日期" };
        /// <summary>
        /// 默认的属性分类
        /// </summary>
        private static Dictionary<string, string> catetory
            = new Dictionary<string, string>(){
            { "编码", "系统属性" }
              ,{ "编号", "系统属性" }
              , { "创建", "自动属性" } 
              , { "修改人", "自动属性" }
               , { "修改日期", "自动属性" }              
              , { "ID", "自动属性" } 
              ,{"名称","基本属性"}
              ,{"","其他属性"}
            };

        static private Dictionary<string, TypeConverter> converter
                = new Dictionary<string, TypeConverter>()
                {
                   {"修改权限",new QcTypeConverter(修改权限s)}
                  ,{"是否概查",new BoolConverter()}
                  ,{"是否默认",new BoolConverter()}
                  ,{"创建人",new QcNameConverter()}
                  ,{"修改人",new QcNameConverter()}
                };

        public QcCheckProjectDescriptor(DynamicDataRowObject DynamicObject)
                : base(DynamicObject 
                , readonlyfields
                , catetory 
                , converter
                )
            {

            }
        public class BoolConverter : TypeConverter
        {
            public static string[] 标准检查项s = new string[] { "是", "否" };
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }


            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                if (value.ToString() == "是")
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                if (value.ToString() == "0")
                {
                    return "否";
                }
                else
                {
                    return "是";
                }
            }

            public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(标准检查项s);
            }
            //
        }
    }
}
