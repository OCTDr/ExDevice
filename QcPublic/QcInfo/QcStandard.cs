using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace QcPublic
{
  public  class QcStandard : QcNode 
    {
         public override string Name
        {
            get
            {
                return this["标准名称"];
            }
            set
            {
                base["标准名称"] = value;
            }
        }
        public override string Code
        {
            get
            {
                return this[CodeField];
            }
            set
            {
                this[CodeField] = value;
            }
        }
        public override string CodeField
        {
            get { return "标准ID"; }
        }
        public QcStandard(DataRow row = null)
            : base(row, "QC_PRO_STANDARD")
        {
        }
        public override  QcCheckResult Check(string field=null)
        {
            QcCheckResult result = new QcCheckResult(this);
            bool checkall = (field == null);
            result.AddCheckNull(field, new[] { "标准名称", "标准号", "登记日期", "类型"});
            result.AddCheckEnum(field, "类型", QcStandardDescriptor.类型s);
            
            if (result.Count > 0) return result;            
            return null;
        }
        public override bool Update(QcDbTransaction trans = null)
        {
            bool isnew = false;
            if (IsNew())
            {
                isnew = true;
                this.Code = this.GetNextCode();                
            }
            return base.Update(trans);
        }
        public static QcStandard GetStandardByID(string id)
        {
            return QcStandard.GetStandards(string.Format("标准ID='{0}'", id)).First();
        }
        public static IEnumerable<QcStandard> GetStandards(string expr = "", string orderby = "")
        {
            if (expr != "") expr = " where " + expr;
            if (orderby != "") expr = expr + orderby;
            try
            {
                return DbHelper.Query("select * from QC_PRO_STANDARD " + expr).Select(t => new QcStandard(t));
            }
            catch
            {
                return GetStandards();
            }
        }
        public string GetNextCode()
        {            
            return QcCode.GetNextNumber("S",
               QcCompany.GetCompanys(),
                1, 6, "000000"
                );

        }
        
    }
}
