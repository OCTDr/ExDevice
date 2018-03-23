////////////////////////////////////
//   产品类别类
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
    public class QcProductType : QcNode
    {

        public static readonly string NameField = "产品类别名称";
        public static readonly string TableName = "QC_PRO_PRODUCTTYPE";
        private List<QcQuaelement> m_Nodes = null;
        private List<QcCheckProject> p_Nodes = null;
        public override string Name
        {
            get { return row[QcProductType.NameField].ToString(); }
            set { Row[QcProductType.NameField] = value; }

        }
        public override string CodeField
        {
            get { return "产品类别编码"; }
        }
        public override string Code
        {
            get
            {
                return this["产品类别编码"];
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this["产品类别编码"] = "";
                    this["产品级别编码"] = "";
                }
                else
                {
                    this["产品类别编码"] = value;
                    this["产品级别编码"] = value.Substring(0, 3);
                }
            }
        }
        public bool CanEdit(QcUser user)
        {
            if (user.Name == this["创建人"] || user.UserID == this["创建人"]) return true;
            if (user.HasPermission("内置系统管理")) return true;
            return false;
        }
        public bool CanDelete(QcUser user)
        {
            if (user.Name == this["创建人"] || user.UserID == this["创建人"]) return true;           
            if (user.HasPermission("内置系统管理")) return true;
            return false;
        }
        public bool CanCreat(QcUser user)
        {
            if (user.HasPermission("产品类别管理")) return true;
            return false;
        }
        public bool CanModelEdit(QcUser user)
        {
            if (user.HasPermission("模型管理")) return true;
            if (user.HasPermission("内置系统管理")) return true;
            return false;
        }
        public bool CanPrjEdit(QcUser user)
        {
            if (user.HasPermission("方案管理")) return true;
            if (user.HasPermission("内置系统管理")) return true;
            return false;
        }
        public QcProductLevel Parent = null;
        public QcProductType(QcProductLevel parent) : base(null, TableName) { Parent = parent; }
        private QcProductType(DataRow row)
            : base(row)
        {
            if (row == null) return;
        }
        public bool CopyFrom(QcProductType src)
        {
            src.Nodes.ForEach(t => this.Nodes.Add(
                 t.CloneTo(this)
                ));         
            return true;
        }
        public override QcCheckResult Check(string field = null)
        {
            QcCheckResult result = new QcCheckResult(this);
            bool checkall = (field == null);
            result.AddCheckNull(field, new[] { "产品类别", "产品类别名称", "产品规格单位" });
            result.AddCheckUsed(field, Parent.Nodes, new[] { "产品类别", "产品类别名称" }, 0);
            if (result.Count > 0) return result;
            return null;
        }
        public  QcCheckResult NoCheckParent(string field = null)
        {
            QcCheckResult result = new QcCheckResult(this);
            bool checkall = (field == null);
            result.AddCheckNull(field, new[] { "产品类别", "产品类别名称", "产品规格单位" });
            if (result.Count > 0) return result;
            return null;
        }
        public string GetNextPartCode()
        {
            return QcCode.GetNextPartNumber(this, Nodes, 5);
        }
        public string GetNextPrjCode()
        {
            return QcCode.GetNextPartNumber(this, PrjNodes, 5);
        }
        public List<QcQuaelement> Nodes
        {
            get
            {   if(m_Nodes==null || m_Nodes.Count==0)  m_Nodes = QcQuaelement.GetQuaelement(this);
                if (m_Nodes == null) m_Nodes = new List<QcQuaelement>();
                return m_Nodes;
            }
        }
        public List<QcCheckProject> PrjNodes
        {
            get
            {
                p_Nodes = QcCheckProject.GetCheckProject(this);
                if (p_Nodes == null) p_Nodes = new List<QcCheckProject>();
                return p_Nodes;
            }
        }
        public static List<QcProductType> GetProductType(QcProductLevel level)
        {
            string 产品级别编码 = level.Code;
            return GetProductType("产品级别编码='" + 产品级别编码 + "'", level);
        }
        public static List<QcProductType> GetProductType(string expr, QcProductLevel parent)
        {

            var pls = DbHelper.Query("Select * from " + TableName + " where " + expr);
            return (from p in pls select new QcProductType(p) { Parent = parent }).ToList();
        }

        public static List<QcProductType> GetProductType(string expr)
        {
            var pls = DbHelper.Query("Select * from " + TableName + " where " + expr);
            return (from p in pls select new QcProductType(p)).ToList();
        }
        public static List<QcProductType> GetProductType()
        {
            var pls = DbHelper.Query("Select * from " + TableName);
            return (from p in pls select new QcProductType(p)).ToList();
        }

        public static QcProductType GetProductTypeById(string id)
        {
            var pls = DbHelper.Query("Select * from " + TableName + " where 产品类别编码='" + id+"'");
            return (from p in pls select new QcProductType(p)).ToList().FirstOrDefault();
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
            //////////////////////////oct修改 为什么要遍历修改子元素呢 
            //if (Nodes.Any(t => t.Update(trans) == false))
            //{
            //    if (IsNew()) this.Code = "";
            //    return false;
            //}
            //////////////////////////oct修改 为什么要扁率修改子元素呢 

            //if(Parent.Nodes.Contains(this)==false)
            if ((Parent.Nodes.Any(t => t.Code == this.Code)) == false)
                Parent.Nodes.Add(this);
            return true;
        }
        public bool UpdatebyID(string code, QcDbTransaction trans = null)
        {

            if (IsNew()) this.Code = code;

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
