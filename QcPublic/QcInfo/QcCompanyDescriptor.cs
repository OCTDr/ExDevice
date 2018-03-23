using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
namespace QcPublic
{
   public  class QcCompanyDescriptor : DynamicTypeDescriptor
    {
       static public List<string> 资质等级s=new List<string>{"甲级","乙级","丁级","丙级","其它"};
       static public List<string> 类型s=new List<string>{"检验机构","省测直事业","省直事业","其他事业","公司","其它"};
        static private string[] readonlyfields = { "单位ID" };
        static private Dictionary<string, string> catetory
            = new Dictionary<string, string>()
            {
                {"联系电话","联系信息"}
                ,{"传真","联系信息"}
                ,{"邮编","联系信息"}
                ,{"通讯地址","联系信息"}
                ,{"单位名称","基本信息"}
                ,{"等级","基本信息"}
                ,{"类型","基本信息"}
                ,{"行政隶属","基本信息"}
                ,{"单位ID","自动属性"}
            };
        static private Dictionary<string, TypeConverter> converter
                = new Dictionary<string, TypeConverter>()
                {
        {"等级",new QcTypeConverter(资质等级s)}
        ,{"类型",new QcTypeConverter(类型s)}
                };

        public QcCompanyDescriptor(DynamicDataRowObject DynamicObject)
            : base(DynamicObject
            , readonlyfields
            , catetory
            , converter
            )
        {

          }
    }
}
