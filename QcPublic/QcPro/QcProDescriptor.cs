////////////////////////////////////
//   产品级别包装类，主要完成新建的可以修改编码的问题
//  abao++
// 2014/6/20 new
///////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;
using Customparamtype;
namespace QcPublic
{
    /// <summary>
    ///  项目，任务和Job的属性包装器,
    ///  例外，可以输入编码
    /// </summary>
    public class QcProTaskJobDescriptor : DynamicTypeDescriptor
    {
            
            static private string[] readonlyfields = { "编号", "创建", "修改"};
            static private Dictionary<string, string> catetory
                = new Dictionary<string, string>()
            {
                 {"编号","系统属性"}
                ,{"创建","自动属性"}
                ,{"修改","自动属性"}
                ,{"成果名称","数据成果信息"}
                ,{"生产开始日期","数据成果信息"}
                ,{"生产结束日期","数据成果信息"}
                ,{"生产单位","数据成果信息"}
                ,{"成果资料","数据成果信息"}
                ,{"备注","杂项"}
                ,{"作业名称","必要信息"}
                ,{"启动类型","必要信息"}
                ,{"方案ID","方案信息"}
                ,{"产品级别编码","方案信息"}
                ,{"产品类别编码","方案信息"}
                ,{"任务名称","必要信息"}
                ,{"任务通知单号","必要信息"}
                ,{"项目名称","必要信息"}
                ,{"项目开始时间","必要信息"}
                ,{"项目结束时间","必要信息"}
                ,{"","其他信息"}
            };
            static private Dictionary<string, TypeConverter> converter
                = new Dictionary<string, TypeConverter>()
                {
                    
        {"项目来源",new 项目来源Converter()}
        ,{"项目类型",new 项目类型Converter()}
        ,{"创建人",new QcNameConverter()}
        ,{"修改人",new QcNameConverter()}
        ,{"作业状态",new 作业状态Converter()}  
        ,{"作业优先级",new 作业优先级Converter()}
         ,{"启动类型",new 启动类型Converter()}
         ,{"任务状态",new 任务状态Converter()}
        ,{"任务优先级",new 任务优先级Converter()}
        ,{"承担部门",new 承担部门Converter()}
        ,{"项目负责人",new 项目负责人Converter()}
        ,{"任务负责人",new 任务负责人Converter()}
        ,{"作业员",new 作业员Converter()}
        ,{"方案ID",new 方案IDConverter()}        
        ,{"产品级别编码",new 产品级别编码Converter()}
        ,{"产品类别编码",new 产品类别编码Converter()}
        ,{"质量评价模型类型",new QcTypeConverter(new List<string>{"标准","非标准"})}        
       ,{"生产单位",new 生产单位Converter()}
        };
            public QcProTaskJobDescriptor(DynamicDataRowObject DynamicObject)
                : base(DynamicObject 
                , readonlyfields
                , catetory
                , converter
                )
            {

            }
         
         }
    public class 项目负责人Converter:QcNameConverter 
    {
        public static IEnumerable<string> 项目负责人
        {
            get
            {
                return QcUser.GetUserByRole("项目负责人");
            }
        }

        public 项目负责人Converter() : base(()=>项目负责人) { }
    }
    public class 任务负责人Converter : QcNameConverter
    {
        public static IEnumerable<string> 任务负责人
        {
            get
            {
                return QcUser.GetUserByRole("任务负责人");
            }
        }

        public 任务负责人Converter() : base(()=>任务负责人) { }
    }
    public class 作业员Converter : QcNameConverter
    {
        public static IEnumerable<string> 作业员
        {
            get
            {
                return QcUser.GetUserByRole("作业员");
            }
        }

        public 作业员Converter() : base(()=>作业员) { }
    }
    public class 权限用户Converter : QcTypeConverter
    {

        public 权限用户Converter(string Permission) : base(QcUser.GetUserByPermission(Permission)) { }
    }
    public class 承担部门Converter : QcTypeConverter
    {

        public static IEnumerable<string> 承担部门
        {
            get
            {
                return QcDepartment.Departments.Select(t => t.Name);
            }
        }
        public 承担部门Converter() : base(()=>承担部门) { }
    }
    public class 项目来源Converter : QcTypeConverter
    {
        public static string[] 项目来源 = new string[] { "国家基础测绘", "省基础测绘", "市场项目" ,"其它"};
        public 项目来源Converter() : base(项目来源) { }

    }
    public class 项目类型Converter : QcTypeConverter
    {
        public static string[] 项目类型 = new string[] { "委托检验", "监督检查","自查","其它"};
        public 项目类型Converter() : base(项目类型) { }

    }
    public class 作业状态Converter : QcTypeConverter
    {
        public static string[] 作业状态 = new string[] { "未启动", "正在处理", "已经完成", "系统故障" };
        public 作业状态Converter() : base(作业状态) { }

    }
    public class 作业优先级Converter : QcTypeConverter
    {
        public static string[] 作业优先级 = new string[] { "1", "2", "3", "4", "5" };
        public 作业优先级Converter() : base(作业优先级) { }

    }
    public class 任务状态Converter : QcTypeConverter
    {
        public static string[] 任务状态 = new string[] { "未启动", "正在处理", "已经完成", "系统故障" };
        public 任务状态Converter() : base(任务状态) { }

    }
    public class 任务优先级Converter : QcTypeConverter
    {
        public static string[] 任务优先级 = new string[] { "1", "2", "3", "4", "5" };
        public 任务优先级Converter() : base(任务优先级) { }

    }
    public class 启动类型Converter : QcTypeConverter
    {
        public static string[] 启动类型 =System .Enum.GetNames(typeof (Customparamtype.Starttype ));
        public 启动类型Converter() : base(启动类型) { }

    }
 
    public class 方案IDConverter : QcTypeConverter<QcCheckProject,QcCheckProject>
    {
        
        public static IEnumerable<QcCheckProject> 方案s
        {
            get
            {
                var prjs = QcCheckProject.GetCheckProject(产品类别编码Converter.select);
                return prjs;
             /*
                return DbHelper.Query("select c.方案ID,c.方案名称,c.产品级别编码,c.产品类别编码,t.产品类别名称,l.产品级别名称 from qc_ins_checkproject c,QC_PRO_PRODUCTTYPE  t,"+
                    "QC_PRO_PRODUCTLevel  l where c.产品级别编码=l.产品级别编码 and c.产品类别编码=t.产品类别编码 and (c.产品类别编码='"
                    + ((产品类别编码Converter.select == null) ? "" : 产品类别编码Converter.select.Code) 
                    + "' and c.产品级别编码='" + 
                    ((产品级别编码Converter.select==null)?"":产品级别编码Converter.select.Code) + "')"
                    );*/
                    
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            QcCheckProject cp = null;
            if (value is string)
                cp = 方案s.First(t => t.Code == value.ToString());
            else
                cp = value as QcCheckProject;
            if (cp == null) return "";
            return base.ConvertTo(context, culture, cp.Name , destinationType);            
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            //return base.ConvertFrom(context, culture, value);
            QcProTaskJobDescriptor ds = context.Instance as QcProTaskJobDescriptor ;
            QcJob job = ds.RowObject as QcJob;
            
            string v = value.ToString();
            if (v.IndexOf('|') > 0)
            {
                v= v.Split('|')[1];
            }
            if (job != null)
            {
                var s = 方案s.Where(t => t["方案ID"] == v).FirstOrDefault();
                if(s!=null)
                {
                    job["产品级别编码"] = s["产品级别编码"];
                    job["产品类别编码"] = s["产品类别编码"];
                    job["质量评价模型类型"] = s["产品类别名称"];
                }
            }
            QcCheckProject cp = 方案s.First(t => t.Name == value.ToString());
            return cp.Code;
        }
      
        public 方案IDConverter() : base(null, 
            t =>t
            ,
            () => 方案s) { }
    }

    public class 产品级别编码Converter : QcTypeConverter<QcProductLevel,QcProductLevel>
    {

        static public QcProductLevel select=null;
        public static IEnumerable<QcProductLevel> 级别s
        {
            get
            {
                return QcProductLevel.GetProductLevel();
            }
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            QcProductLevel lvl=null;
            if (value is string)
                lvl = 级别s.First(t => t.Code == value.ToString());
            else
                lvl = value as QcProductLevel;
            if (lvl == null) return "";
            产品级别编码Converter.select = lvl;
            return lvl.Name;
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            //return base.ConvertFrom(context, culture, value);
            QcProTaskJobDescriptor ds = context.Instance as QcProTaskJobDescriptor;
            QcJob job = ds.RowObject as QcJob;
            string v = value.ToString();            
            if (job != null)
            {
                var s = 级别s.Where(t => t.Name == v).FirstOrDefault();
                产品级别编码Converter.select = s;
                if (s != null)
                {
                    if (job["产品类别编码"].Length > s.Code.Length)
                    {
                        if (job["产品类别编码"].Substring(0, s.Code.Length) != s.Code)
                        {
                            job["产品类别编码"] = "";
                            job["方案ID"] = "";
                        }
                    }
                    return s.Code;
                }
            }
            return "";
        }

        public 产品级别编码Converter()
            : base(null,
                t =>t ,
                () => 级别s) { }
    }

    public class 产品类别编码Converter : QcTypeConverter<QcProductType,QcProductType>
    {
        public static QcProductType select = null;
        public static IEnumerable<QcProductType> 类别s
        {
            get
            {
                if (产品级别编码Converter.select == null) return  new List<QcProductType >();
                return QcProductType.GetProductType(产品级别编码Converter.select);
            }
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {

            QcProductType pt = null;
            if (value is string)
            {
               
                    pt = 类别s.FirstOrDefault(t => t.Code == value.ToString());
              
                
            }
            else
                pt = value as QcProductType;
            if (pt != null)
            {
                产品类别编码Converter.select = pt;
                return pt.Name;
            }

            else
                return "";
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            //return base.ConvertFrom(context, culture, value);
            QcProTaskJobDescriptor ds = context.Instance as QcProTaskJobDescriptor;
            QcJob job = ds.RowObject as QcJob;
            string v = value.ToString();
            if (job != null)
            {
                var s = 类别s.Where(t => t.Name == v).FirstOrDefault();
                select = s;
                if (s != null)
                {
                    if (job["方案ID"].Length > s.Code.Length)
                    {
                        if (job["方案ID"].Substring(0, s.Code.Length) != s.Code)
                        {                           
                            job["方案ID"] = "";
                        }
                    }
                    return s.Code;
                }
            }            
            return "";
        }    

        public 产品类别编码Converter()
            : base(null,
                t => t,
                () => 类别s) { }
    }


    public class 生产单位Converter : QcTypeConverter<QcCompany,QcCompany >
    {

        public static IEnumerable<QcCompany> 生产单位s
        {
            get
            {
                return QcCompany.GetCompanys();

            }
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            QcCompany pt = null;
            if (value is string)
            {
                pt = 生产单位s.FirstOrDefault(t => t.Name == value.ToString());
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
            //if (v.IndexOf('|') > 0)
            //{
            //    v = v.Split('|')[0];
            //}          
            return v;
        }

        public 生产单位Converter()
            : base(null,
                t => t,
                () => 生产单位s) { }
    }

}
