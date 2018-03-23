using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel ;
namespace QcPublic
{
   
        /// <summary>
        ///  QcProductLevel属性包装器,
        ///  例外，可以输入编码
        /// </summary>
        public class QcUserDescriptor : DynamicTypeDescriptor
        {
            static private string[] readonlyfieldsnew = { "ID" };
            static private string[] readonlyfields = { "ID", "用户名", "用户密码" };
            static private Dictionary<string, string> catetory
                = new Dictionary<string, string>()
            {
                {"ID","系统属性"}
               // ,{"用户名","不可变属性"}
                ,{"用户名","不可变属性"}
                ,{"用户密码","不可变属性"}
                ,{"","基本属性"}
            };
            static private Dictionary<string, TypeConverter> converter
                = new Dictionary<string, TypeConverter>()
                {
        {"性别",new 性别Converter()}
        ,{"所在分组",new DepartmentConverter()}
        ,{"用户密码",new 密码Converter()}  
        ,{"状态",new 状态Converter()}
        };
            public QcUserDescriptor(QcUser  DynamicObject)
                : base(DynamicObject as DynamicDataRowObject 
                , DynamicObject.IsNew() ? readonlyfieldsnew : readonlyfields
                , catetory
                , converter
                )
            {
            }
        }
  
        public class 性别Converter : QcTypeConverter
        {
            public static string[] 性别s = new string[] { "男", "女" };
            public 性别Converter() : base(性别s) { }

        }
        public class 状态Converter : QcTypeConverter
        {
            public static string[] 状态s = new string[] { "启用", "停用","离职" };
            public 状态Converter() : base(状态s) { }
          

        }
        public class 密码Converter : TypeConverter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }
            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                return value;
            }
            
            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                QcUserDescriptor qu = context.Instance as QcUserDescriptor ;
                if (qu.RowObject.IsNew())
                    return value;
                else
                    return "*";
                //return base.ConvertTo(context, culture, value, destinationType);
            }

        }
        public class DepartmentConverter : TypeConverter
        {

           List<string> lstDepartment = null;
            public DepartmentConverter()
            {
                lstDepartment = (from p in QcDepartment.Departments select p.Code).ToList();
            }
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }
            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                QcDepartment d = QcDepartment.GetDepartmentFromName(value.ToString());
                return (d == null) ? "" : d.Code ;
                
            }
            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                //return base.ConvertTo(context, culture, value, destinationType);
                QcDepartment d=  QcDepartment.GetDepartmentFromCode(value.ToString());
                return (d == null) ? "" : d.Name;
            }

            public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(lstDepartment);
            }

        }
    
}
