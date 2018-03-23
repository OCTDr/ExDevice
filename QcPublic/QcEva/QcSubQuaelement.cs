////////////////////////////////////
//   质量子元素类
//  abao++
///////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace QcPublic
{
    public class QcSubQuaelement :QcNode
    {
        public static readonly string NameField = "名称";

        public static readonly string TableName = "QC_EVA_SUBQUAELEMENT";
        List<QcCheckEntry> m_Nodes=null;
          public override  string Name
        {
            get { return row[QcSubQuaelement.NameField ].ToString(); }
            set { Row[QcSubQuaelement.NameField] = value; }

        }
          public QcSubQuaelement(QcQuaelement parent) : base(null, TableName) { Parent = parent; }
        private QcSubQuaelement(DataRow row)
            : base(row)
        {
            

        }
        public QcQuaelement Parent = null;
        public override string CodeField
        {
            get { return "质量子元素编码"; }
        }
        public override  string Code
        {
            get
            {
                return this["质量子元素编码"];
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this["质量子元素编码"] = "";
                    this["质量元素编码"] = "";
                }
                else
                {
                    this["质量子元素编码"] = value;
                    this["质量元素编码"] = value.Substring(0, 7);
                }
            }
        }
        public string GetNextPartCode()
        {
            return QcCode.GetNextPartNumber(this
                ,Nodes
                , 9);
        }
        public QcSubQuaelement CloneTo(QcQuaelement  Parent)
        {
            QcSubQuaelement qsq = new QcSubQuaelement(Parent);
            qsq.CloneFields(this);            
            Nodes.ForEach(sq => qsq.Nodes.Add(sq.CloneTo(qsq)));
            return qsq;
        }
        public List<QcCheckEntry> Nodes
        {
            get
            {
                if (m_Nodes == null)
                    m_Nodes = QcCheckEntry.GetCheckEntry(this);
                if (m_Nodes == null)
                    m_Nodes = new List<QcCheckEntry>();
                return m_Nodes;
            }
        }
        public static List<QcSubQuaelement> GetSubQuaelement(QcQuaelement Quaelement)
        {
            var pls = DbHelper.Query("Select * from QC_EVA_SUBQUAELEMENT where 质量元素编码='" + Quaelement.Code+"'");
            return (from p in pls select new QcSubQuaelement(p) { Parent = Quaelement }).ToList();
        }
        public override bool DeleteFromDb(QcDbTransaction trans)
        {
            bool ret;
            /// 遍历删除子元素
            if (Nodes.ToArray().Any(t => t.DeleteFromDb(trans) == false)) return false;
            ret = base.DeleteFromDb(trans);
            if (ret) Parent.Nodes.Remove(this);
            return ret;
        }
        public override bool Update(QcDbTransaction trans)
        {
            
            if (IsNew()) this.Code = Parent.GetNextPartCode();
           // QcLog.LogString("Start QcSubQuaelement Update "+this.Name );
            bool ret = base.Update(trans);
            if (ret == false && IsNew()) {
                //QcLog.LogString("QcSubQuaelement Update Fail! " + this.Name);
                this.Code = ""; return false; }
            if (Nodes.ToArray().Any(t => t.Update(trans) == false))
            {
                if (IsNew()) this.Code = "";
                return false;
            }
            if (ret)
            {
                if(Parent.Nodes.Contains(this)==false)
                Parent.Nodes.Add(this);
            }
            return ret;
        }
        public override QcCheckResult Check(string field = null)
        {
            QcCheckResult result = new QcCheckResult(this);
            bool checkall = (field == null);
            result.AddCheckNull(field, new[] { "名称", "计分方式", "分组" });
            result.AddCheckUsed(field, Parent.Nodes, new[] { "名称" });
            result.AddCheckEnum(field, "计分方式",计分方式Converter.计分方式s);
            if (result.Count > 0) return result;
            return null;
           
          
        }
        public object Tag { get; set; }
    }
}
