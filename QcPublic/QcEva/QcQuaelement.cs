////////////////////////////////////
//   质量元素类
//  abao++
///////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace QcPublic
{

    public class QcQuaelement : QcNode
    {

        public static readonly string NameField = "名称";
        public static readonly string TableName = "QC_EVA_QUAELEMENT";
        List<QcSubQuaelement> m_Nodes = null;
        public override  string Name
        {
            get { return row[QcQuaelement.NameField ].ToString(); }
            set { Row[QcQuaelement.NameField] = value; }

        }
        public QcQuaelement(QcProductType parent) : base(null, TableName) { Parent = parent; }
        private QcQuaelement(DataRow row)
            : base(row)
        {
            if (row == null) return;
        }
        public QcProductType Parent = null;
        public override string CodeField
        {
            get { return "质量元素编码"; }
        }
        public override  string Code
        {
            get
            {
                return this["质量元素编码"];
            }
            set
            {
                
                if (string.IsNullOrEmpty(value))
                {
                    this["质量元素编码"] = "";
                    this["产品类别编码"] = "";
                    this["产品级别编码"] = "";
                }
                else
                {
                    this["质量元素编码"] = value;
                    this["产品类别编码"] = value.Substring(0, 5);
                    this["产品级别编码"] = value.Substring(0, 3);
                }
            }
        }
        public string GetNextPartCode()
        {
            return QcCode.GetNextPartNumber(this,
                Nodes
                , 7);
        }

        public List<QcSubQuaelement> Nodes
        {
            get
            {
                if (m_Nodes == null) m_Nodes = QcSubQuaelement.GetSubQuaelement(this);
                if (m_Nodes == null) m_Nodes = new List<QcSubQuaelement>();
                return m_Nodes;
            }
        }
        public static List<QcQuaelement> GetQuaelement(QcProductType ProductType)
        {
            var pls = DbHelper.Query("Select * from QC_EVA_QUAELEMENT where 产品类别编码='" + ProductType.Code + "'");
            return (from p in pls select new QcQuaelement(p) { Parent=ProductType}).ToList();
        }
        public override bool DeleteFromDb(QcDbTransaction trans)
        {
            /// 遍历删除子元素
            bool ret;
            /// 遍历删除子元素
            if (Nodes.ToArray().Any(t => t.DeleteFromDb(trans) == false)) return false;
            ret = base.DeleteFromDb(trans);
            if (ret) Parent.Nodes.Remove(this);
            return ret;
            
        }
        public QcQuaelement CloneTo(QcProductType parent)
        {
            QcQuaelement qq = new QcQuaelement(parent);
            qq.CloneFields(this);
            Nodes.ForEach(sq => qq.Nodes.Add( sq.CloneTo(qq)));
            return qq;
        }

        public override bool Update(QcDbTransaction trans)
        {
            if (IsNew()) this.Code = Parent.GetNextPartCode();
            //QcLog.LogString("Start QcQuaelement Update"+this.Name );
            bool ret = base.Update(trans);
            if (ret == false && IsNew()) {
               // QcLog.LogString("QcQuaelement Update Fail! " + this.Name);
                this.Code = ""; return false; 
            }
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
            result.AddCheckEnum(field, "计分方式", 计分方式Converter.计分方式s);            
            if (result.Count > 0) return result;
            return null;
           
        }
        public object Tag { set; get; }
    }
}
