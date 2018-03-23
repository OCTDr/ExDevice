using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace QcPublic
{
    public class QcStandardBorrow : QcNode
    {
        public override string Name
        {
            get
            {
                return null;
            }
            set
            {
                ;
            }
        }
        public override string Code
        {
            get
            {
                return null;
            }
            set
            {
               ;
            }
        }
        public override string CodeField
        {
            get { return null; }
        }
        public QcStandardBorrow(DataRow row = null)
            : base(row, "QC_PRO_STANDARDBORROW")
        {
        }
        public override QcCheckResult Check(string field = null)
        {
            QcCheckResult result = new QcCheckResult(this);
            bool checkall = (field == null);
            result.AddCheckNull(field, new[] { "标准ID", "借用人", "借用日期", });
         
            if (result.Count > 0) return result;
            return null;
        }
        public override bool Update(QcDbTransaction trans = null)
        {
            bool isnew = false;
            if (IsNew())
            {
              
            }
            return base.Update(trans);
        }

        public static IEnumerable<QcStandardBorrow> GetStandardBorrowByStandard(QcStandard standard)
        {
            return DbHelper.Query("select * from QC_PRO_STANDARDBORROW where 标准ID='" + standard.Code +"'").Select(t => new QcStandardBorrow(t));
        }


        public static IEnumerable<QcStandardBorrow> GetStandardBorrow(string expr = "", string orderby = "")
        {
            if (expr != "") expr = " where " + expr;
            if (orderby != "") expr = expr + orderby;
            return DbHelper.Query("select * from QC_PRO_STANDARDBORROW " + expr).Select(t => new QcStandardBorrow(t));
        }
        public string GetNextCode()
        {
            return QcCode.GetNextNumber("C",
               QcCompany.GetCompanys(),
                1, 6, "000000"
                );

        }

    }
}
