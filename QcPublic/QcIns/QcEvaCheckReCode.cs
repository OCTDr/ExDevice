using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace QcPublic
{
    public class QcEvaCheckReCode:DynamicDataRowObject 
    {
        public static string TableName = "qc_eva_checkrecode";
        public QcEvaCheckReCode(DataRow row=null)
            : base(row, TableName)
        {

        }
        
    }
}
