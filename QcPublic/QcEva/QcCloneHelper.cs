using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace QcPublic
{
    static class QcCloneHelper
    {
        static string[] fieldfilter={"编码","创建"};
        public static bool CloneFields(this DynamicDataRowObject dest,DynamicDataRowObject src)
        {
            if (src.GetRow().Table.TableName != dest.GetRow().Table.TableName) return false;
            var cols = src.GetRow().Table.Columns;
            foreach(DataColumn  c in cols)
            {
                if (fieldfilter.Any(t => c.ColumnName.Contains(t)))
                    continue;
                dest[c.ColumnName] = src[c.ColumnName];
            }
            return true;
        }
    }
}
