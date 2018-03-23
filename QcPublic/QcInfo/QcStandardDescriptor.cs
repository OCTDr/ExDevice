using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
namespace QcPublic
{
 public   class QcStandardDescriptor : DynamicTypeDescriptor
    {
        static public List<string> 类型s = new List<string> { "国标", "国标推", "测绘", "测绘推", "其他行标", "指导", "地方", "其它" };
        static private string[] readonlyfields = { "标准ID" };
        static private Dictionary<string, string> catetory
            = new Dictionary<string, string>()
            {
                 {"标准名称","必要信息"}  
                ,{"标准号","必要信息"}
                 ,{"登记日期","必要信息"}
                ,{"类型","必要信息"}
                ,{"受控范围","管理信息"}
                ,{"来源","管理信息"}
                ,{"剩余数量","管理信息"}
                ,{"废除日期","管理信息"}
                 ,{"数量","管理信息"}
                ,{"发布单位","其他信息"}
                ,{"发布日期","其他信息"}
                ,{"标准ID","自动属性"}
            };
    
        static private Dictionary<string, TypeConverter> converter
                = new Dictionary<string, TypeConverter>()
                {
                {"类型",new QcTypeConverter(类型s)}
                };

        public QcStandardDescriptor(DynamicDataRowObject DynamicObject)
            : base(DynamicObject
            , readonlyfields
            , catetory
            , converter
                )
            {

            }
    }
}
