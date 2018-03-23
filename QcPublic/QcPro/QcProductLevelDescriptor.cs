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
    public class QcProductLevelDescriptor : DynamicTypeDescriptor
    {
        static private string[] readonlyfields={"创建"};
        public QcProductLevelDescriptor(DynamicDataRowObject DynamicObject)
            : base(DynamicObject
            ,DynamicObject.IsNew()?QcProductLevelDescriptor.readonlyfields:null)
        {
            
        }
    }
}
