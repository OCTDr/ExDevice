////////////////////////////////////
//   产品级别类
//  abao++
// 待修复问题，新建项输入编码的有效性规则
///////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace QcPublic
{
    public class QcProductLevel : QcNode
    {

        public static readonly string NameField = "产品级别";
        public static readonly string TableName = "QC_PRO_PRODUCTLevel";
        public static List<QcProductLevel> QcProductLevels=GetProductLevel();
        private List<QcProductType> m_Nodes=null;
        public override  string Name
        {
            get { return row[QcProductLevel.NameField].ToString(); }
            set { Row[QcProductLevel.NameField] = value; }

        }
        public override string CodeField
        {
            get { return "产品级别编码"; }
        }
        public override  string Code
        {
            get
            {
                return this["产品级别编码"];
            }
            set
            {
                this["产品级别编码"] = value;
            }
        }
        public override QcCheckResult Check(string field = null)
        {

            QcCheckResult result = new QcCheckResult(this);           
            result.AddCheckNull(field, new[] { "产品级别编码", "产品级别", "产品级别名称" });            
            if (base.IsNew())
            {
                result .AddCheckEnable(field ,new[] { "产品级别编码" });
                result.AddCheckUsed(field,QcProductLevels, new[] { "产品级别编码" },0);                 
            }
            result.AddCheckUsed(field, QcProductLevels, new[] { "产品级别", "产品级别名称" }, 0);                 
            //与别的类不同，新建未加入静态的QcProductLevels列表           
            if (result.Count > 0) return result;
            return null;
        }
   
        public string GetNextPartCode()
        {
            if (this.Nodes.Count() == 0)
                return this.Code + "00";
            return QcCode.GetNextPartNumber(this,Nodes, 3);
        }
        public List<QcProductType> Nodes
        {
            get
            {
                m_Nodes = QcProductType.GetProductType(this);
                if (m_Nodes == null) m_Nodes = new List<QcProductType>();
                return m_Nodes;
            }
        }
        public QcProductLevel():base(null,TableName)
        {

        }
        private QcProductLevel(DataRow row)
            : base(row, TableName)
        {
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
        public bool CanCreat(QcUser user)        {
            if (user.HasPermission("产品级别管理")) return true;         
            return false;
        }
        public  static List<QcProductLevel> GetProductLevel()
        {

            var pls = DbHelper.Query("Select * from "+TableName);
            QcProductLevels = (from p in pls select new QcProductLevel(p)).ToList();
            return QcProductLevels;
        }

        public static List<QcProductLevel> GetProductLevel(string pExpr)
        {
            var pls = DbHelper.Query("Select * from " + TableName + " where " + pExpr);
            QcProductLevels = (from p in pls select new QcProductLevel(p)).ToList();
            return QcProductLevels;
        }
        public static QcProductLevel GetProductLevelById(string id)
        {
            return GetProductLevel(string.Format("产品级别编码='{0}'",id)).FirstOrDefault();
        }
        public override bool Update(QcDbTransaction trans=null)
        {
            //////////////////////////oct修改 为什么要遍历修改子元素呢 
            //if (Nodes.Any(t => t.Update(trans) == false)) return false;
            //////////////////////////oct修改 为什么要遍历修改子元素呢 

            bool ret= base.Update(trans);
            if (ret)
            {
                if(QcProductLevels.Contains(this)==false)
                    QcProductLevel.QcProductLevels.Add(this);
            }
            return ret;
        }
        public override bool DeleteFromDb(QcDbTransaction trans=null)
        {
            bool ret ;
            /// 遍历删除子元素
            if (Nodes.ToArray().Any(t => t.DeleteFromDb(trans) == false)) return false; 
            
            ret= base.DeleteFromDb(trans);
            if (ret) QcProductLevels.Remove(this);
            return ret;
        }
        public object Tag { get; set; }
        
     
       

    }
}
