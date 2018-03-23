using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace QcPublic
{
  public   class QcCompany:QcNode 
    {
        public override string Name
        {
            get
            {
                return this["单位名称"];
            }
            set
            {
                base["单位名称"] = value;
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
            get { return "单位ID"; }
        }
        public QcCompany(DataRow  row=null)
            : base(row,"QC_PRO_COMPANY")
        {
        }
        public override  QcCheckResult Check(string field=null)
        {
            QcCheckResult result = new QcCheckResult(this);
            bool checkall = (field == null);
            result.AddCheckNull(field, new[] {"单位名称", "通讯地址", "等级","类型","行政隶属","联系电话" });
            result.AddCheckUsed(field,QcCompany.GetCompanys(), new[] { "单位名称" }, (IsNew() ? 0 : 1));            
            result.AddCheckEnum(field, "等级", QcCompanyDescriptor.资质等级s );
            result.AddCheckEnum(field, "类型", QcCompanyDescriptor.类型s );
            
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

        public static  IEnumerable<QcCompany> GetCompanys(string expr="",string orderby="")
        {
            if (expr != "") expr = " where " + expr;
            if (orderby != "") expr = expr + orderby;
            try
            {
                return DbHelper.Query("select * from QC_PRO_COMPANY " + expr).Select(t => new QcCompany(t));
            }
            catch
            {
                return GetCompanys();
            }
            
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
