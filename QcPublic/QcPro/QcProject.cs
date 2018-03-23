using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel.Design;
namespace QcPublic
{
    public class QcProject :QcNode
    {
        public static readonly string NameField = "项目名称";
        public static readonly string TableName = "QC_PRO_Project";
        public override  string Name
        {
            get { return row[QcProject.NameField].ToString(); }
            set { Row[QcProject.NameField] = value; }

        }
        public override string CodeField
        {
            get { return "项目编号"; }
        }
       public override  string Code
        {
            get
            {
                return this["项目编号"];
            }
            set
            {
                this["项目编号"] = value;
            }
        }
          public QcProject() : base(null, TableName) 
          {
              this["项目类型"] = "其它";
              this["项目来源"] = "其它";
          }
        public  QcProject(DataRow row)
            : base(row,TableName)
        {
            if (row == null) return;
        }
        public string GetNextCode()
        {
            string prefix = DbHelper.GetDateTime().ToString("yyyy-MM-dd") + "-";
            return QcCode.GetNextNumber(prefix,
                QcProject.GetProjects("项目编号 like '" + prefix+"%'"),
                11, 2, "00"
                );

        }


        public string 项目负责人
        {
            get
            {
                return QcUser.GetName(this["项目负责人"]);
            }
        }
        public bool CanEdit(QcUser user)
        {
            if(user.Name ==this["项目负责人"]||user.UserID==this["项目负责人"]) return true;
            if (user.Name == this["创建人"] || user.UserID == this["创建人"]) return true;
            if (user.HasPermission("项目管理")) return true;
            if (user.HasPermission("内置系统管理")) return true;
            return false;
        }
        public bool CanDelete(QcUser user)
        {           
            if (user.Name == this["创建人"] || user.UserID == this["创建人"]) return true;
            if (user.HasPermission("项目管理")) return true;
            if (user.HasPermission("内置系统管理")) return true;
            return false;
        }
        public bool CanCreat(QcUser user)
        {
            if (user.HasPermission("创建项目")) return true;           
            return false;
        }
        public bool CanCreatTask(QcUser user)
        {
            if (user.HasPermission("创建项目")) return true;
            if (user.Name == this["项目负责人"] || user.UserID == this["项目负责人"]) return true;
            return false;
        }
        public override bool Update(QcDbTransaction trans = null)
        {
            bool isnew=false;
            if (IsNew()) 
            {
                isnew=true;
                this.Code = this.GetNextCode();
                this["修改日期"] = DateTime.Now.ToLongDateString();
                this["创建人"] = QcUser.User.UserID;
            }
            if (Nodes.Any(t => t.Update(trans) == false)) return false;
            bool ret=base.Update(trans);
            if(ret && isnew)
            {
                QcProjectTimeLine.WriteApointment(this.Code, this.创建人 + "创建了项目");
                this.SendMessage(this.项目负责人,this.Name+"已创建，指定你为项目负责人");
                
            }
            return ret;
        }
        public override bool DeleteFromDb(QcDbTransaction trans = null)
        {
            if (this.创建人== "sys" && QcUser.User.Name != "sys") return false;
            bool ret;
            /// 遍历删除子元素
            if (Nodes.ToArray().Any(t => t.DeleteFromDb(trans) == false)) return false;

            ret = base.DeleteFromDb(trans);
            if (ret) QcProjectTimeLine.WriteApointment(this.Code, QcUser.User.Name + ":删除了项目");
            return ret;
        }
        public override QcCheckResult Check(string field = null)
        {
            QcCheckResult result = new QcCheckResult(this);
            bool checkall = (field == null);
            result.AddCheckNull(field, new[] { "项目类型", "项目来源", "承担部门","项目负责人","项目名称" });
            result.AddCheckUsed(field, QcProject.GetProjects(""), new[] { "项目名称" }, (IsNew() ? 0 : 1));
            result.AddCheckEnum(field, "项目类型", 项目类型Converter.项目类型);
            result.AddCheckEnum(field, "项目来源", 项目来源Converter.项目来源);
            result.AddCheckEnum(field, "承担部门", 承担部门Converter.承担部门);
            result.AddCheckEnum(field, "项目负责人", QcUser.GetUserIDsFromNames(项目负责人Converter.项目负责人));
            if (result.Count > 0) return result;
            return null;
        }

        private List<QcTask> m_Nodes = null;
        public List<QcTask> Nodes
        {
            get
            {
                m_Nodes = QcTask.GetTask(this);
                if (m_Nodes == null) m_Nodes = new List<QcTask>();
                return m_Nodes;
            }
        }
        static public IEnumerable<QcProject> GetProjects(QcUser user)
        {
            return GetProjects("");
        }
        static public IEnumerable<QcProject> GetProjects(string expr)
        {
             return GetProjectsFromSql("Select * from " + TableName + ((expr!="")?" where "+expr:""));          
        }
        static public IEnumerable<QcProject> GetProjectsFromSql(string sql)
        {
            var pls = DbHelper.Query(sql);
            if (pls == null) return null;
            return from p in pls select new QcProject(p);
        }
        public object Tag { get; set; }
    }
}
