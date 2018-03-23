using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using c = PublicConstValue.PublicConst;
using System.Data;
namespace QcPublic
{
   public class QcCheckRule:QcNode
    {

        public static readonly string NameField = c.cFieldCheckRuleID ;
        public static readonly string TableName = c.cQC_INS_checkruleTable ;
     
         public override string Name
        {
            get { return row[c.cFieldCheckRuleName].ToString(); }
            set { Row[c.cFieldCheckRuleName] = value; }

        }
        public override string CodeField
        {
            get { return NameField; }
        }
        public override string Code
        {
            get
            {
                return this[NameField];
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this[NameField] = "";
                    this["方案ID"] = "";
                 }
                else
                {
                    this[NameField] = value;
                    this["方案ID"] = value.Substring(0, 7);
                }
            }
        }
        public string GetNextPartCode()
        {
            return QcCode.GetNextPartNumber(this, GetCheckRule(Parent), 7);
        }
        public override QcCheckResult Check(string field = null)
        {
            QcCheckResult result = new QcCheckResult(this);
            bool checkall = (field == null);
            result.AddCheckNull(field, new[] { "算子编码", "规则名称", "是否可用", "检查项编码" });
            result.AddCheckUsed(field, Parent.Nodes, new[] { "名称" });
            if (result.Count > 0) return result;
            return null;
        }
      List<QcCheckOperator>  m_Nodes = null;
      public List<QcCheckOperator> Nodes
        {
            get
            {
                if (m_Nodes == null)
                {
                    m_Nodes = new List<QcCheckOperator>();
                    m_Nodes.Add(QcProductOperator.GetCheckOperatorByCode(this["算子编码"]));
                }
                 return m_Nodes;
            }
        }
        public QcCheckProject Parent = null;
        public QcCheckRule(QcCheckProject parent) : base(null, TableName) { Parent = parent; }
        private QcCheckRule(DataRow row)
            : base(row)
        {
            if (row == null) return;
        }

       /// <summary>
        /// where expr is not null
       /// </summary>
       /// <param name="expr"></param>
       /// <returns></returns>
        public static List<QcCheckRule> GetCheckRule(string expr)
        {
            var pls = DbHelper.Query("Select * from " + TableName + " where " + expr);
            return (from p in pls select new QcCheckRule(p)).ToList();
        }
        public static List<QcCheckRule> GetCheckRule(QcCheckProject prj)
        {
            string 方案id = prj.Code;
            return GetCheckRule("方案ID='" + 方案id + "'", prj);
        }
        public static List<QcCheckRule> GetCheckRule(string expr, QcCheckProject parent)
        {

            var pls = DbHelper.Query("Select * from " + TableName + " where " + expr);
            return (from p in pls select new QcCheckRule(p) { Parent = parent }).ToList();
        }
        public override bool Update(QcDbTransaction trans = null)
        {
            if (IsNew()) this.Code = Parent.GetNextPartCode();

            bool ret = base.Update(trans);
            if (ret == false && IsNew())
            {
                this.Code = "";
                return false;
            }
            if (Nodes.Any(t => t.Update(trans) == false))
            {
                if (IsNew()) this.Code = "";
                return false;
            }
            if (Parent.Nodes.Contains(this) == false)
                if ((Parent.Nodes.Any(t => t.Code == this.Code)) == false)
                    Parent.Nodes.Add(this);
            return true;
        }
        public override bool DeleteFromDb(QcDbTransaction trans = null)
        {
            /// 遍历删除子元素
            bool ret;
            /// 遍历删除子元素
            if (Nodes.ToList().Any(t => t.DeleteFromDb(trans) == false)) return false;
            ret = base.DeleteFromDb(trans);
            if (ret) Parent.Nodes.Remove(this);
            return ret;

        }
        public object Tag { get; set; }


    }
}
