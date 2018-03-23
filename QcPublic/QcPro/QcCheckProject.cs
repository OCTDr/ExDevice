////////////////////////////////////
//   产品方案类，仿照产品类别 类
//  oct++
///////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using c = PublicConstValue.PublicConst;
using System.Data;

namespace QcPublic
{
 public  class QcCheckProject : QcNode 
    {
        public static readonly string NameField = c .cFieldProjectID ;
        public static readonly string TableName = c.cQC_INS_checkprojectTable;
        private List<QcCheckRule> m_Nodes = null;
        public override string Name
        {
            get { return row[c.cFieldProjectName ].ToString(); }
            set { Row[c.cFieldProjectName] = value; }

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
                    this["产品类别编码"] = "";
                    this["产品级别编码"] = "";                   
                }
                else
                {
                    this[NameField] = value;
                    this["产品级别编码"] = value.Substring(0, 3);
                    this["产品类别编码"] = value.Substring(0, 6);
                }
            }
        }
        public QcProductType  Parent = null;
        public QcCheckProject(QcProductType parent) : base(null, TableName) { Parent = parent; }
         private QcCheckProject(DataRow row)
            : base(row)
        {
            if (row == null) return;
        }
       //public bool CopyFrom(QcCheckProject src)
       // {
       //     src.Nodes.ForEach(t => this.Nodes.Add(
       //          t.CloneTo(this)
       //         ));
       //     return true;
       // }
       public override QcCheckResult Check(string field = null)
       {
           QcCheckResult result = new QcCheckResult(this);
           bool checkall = (field == null);
           result.AddCheckNull(field, new[] { "方案名称", "是否默认", "是否概查"});
           result.AddCheckUsed(field, this.PrjNodes(), new[] { "方案名称" }, 0);
           if (result.Count > 0) return result;
           return null;
       }
       public string GetNextPartCode()
       {
           
          return QcCode.GetNextNumber(this, Nodes, 7,3,"000");
       }
       public List<QcCheckRule > Nodes
       {
           get
           {
               m_Nodes = QcCheckRule.GetCheckRule(this);
               if (m_Nodes == null) m_Nodes = new List<QcCheckRule>();
               return m_Nodes;
           }
       }
       public bool CanEdit(QcUser user)
       {           
           if (user.Name == this["创建人"] || user.UserID == this["创建人"]) return true;
           if (this["修改权限"] == "完全公开") return true;
           if (user.HasPermission("内置系统管理")) return true;
           return false;
       }
       public bool CanEditRule(QcUser user)
       {
           if (user.Name == this["创建人"] || user.UserID == this["创建人"]) return true;
           if (this["修改权限"] == "完全公开" || this["修改权限"] == "规则公开") return true;
           if (user.HasPermission("内置系统管理")) return true;
           return false;
       }
       public bool CanDelete(QcUser user)
       {
           if (user.Name == this["创建人"] || user.UserID == this["创建人"]) return true;
           if (user.HasPermission("内置系统管理")) return true;
           return false;
       }
       public static List<QcCheckProject> GetCheckProject(QcProductType type)
       {
           if (type == null) return new List<QcCheckProject>();
           string 产品类别编码 = type.Code;
           return GetCheckProject("产品类别编码='" + 产品类别编码 + "'", type);
       }
       public static List<QcCheckProject> GetCheckProject(string expr, QcProductType parent)
       {

           var pls = DbHelper.Query("Select * from " + TableName + " where " + expr);
           return (from p in pls select new QcCheckProject(p) { Parent = parent }).ToList();
       }
       public static List<QcCheckProject> GetCheckProject(string expr)
       {

           var pls = DbHelper.Query("Select * from " + TableName + " where " + expr);
           if (pls == null) return new List<QcCheckProject>();
           return (from p in pls select new QcCheckProject(p)).ToList();
       }
       public static List<QcCheckProject> GetCheckProject( )
       {

           var pls = DbHelper.Query("Select * from " + TableName);
           if (pls == null) return new List<QcCheckProject>();
           return (from p in pls select new QcCheckProject(p)).ToList();
       }
       public List<QcCheckProject> PrjNodes()
       {
           var pls = DbHelper.Query("Select * from " + TableName );
           return (from p in pls select new QcCheckProject(p)).ToList();
       }
       public static QcCheckProject GetCheckProjectByid(string id)
       {
          
           var pls = DbHelper.Query("Select * from " + TableName + " where " + NameField + "='" + id + "'");

           if (pls.Count() >0)
           {
               if (pls.First() != null)
               {
                   return new QcCheckProject(pls.First());
               }
               else
               {
                   return null;
               }
           }
           else
               return null;
       }
       public override bool Update(QcDbTransaction trans = null)
       {
           if (IsNew())
           {
               this.Code = Parent.GetNextPrjCode();
               this["产品类别编码"] = Parent.Code;
               this["产品级别编码"] = Parent.Parent.Code;
               this["创建人"] = QcUser.User.UserID;
               this["修改日期"] = DateTime.Now.ToLongTimeString();
           }

           bool ret = base.Update(trans);
           if (ret == false && IsNew())
           {
               this.Code = "";
               return false;
           }
           //if (Nodes.Any(t => t.Update(trans) == false))
           //{
           //    if (IsNew()) this.Code = "";
           //return false;
           //}
           if (Parent.PrjNodes.Contains(this) == false)
               if ((Parent.PrjNodes.Any(t => t.Code == this.Code)) == false)
                   Parent.PrjNodes.Add(this);
           return true;
       }
       public override bool DeleteFromDb(QcDbTransaction trans = null)
       {
           /// 遍历删除子元素
           bool ret;
           //利用数据库外键约束删除规则，不需要用代码控制删除
            ret = base.DeleteFromDb(trans);
           //if (ret) Parent.PrjNodes.Remove(this);
           return ret;
         
       }
       public object Tag { get; set; }
    }
}
