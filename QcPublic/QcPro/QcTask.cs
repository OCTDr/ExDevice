using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace QcPublic
{
    public class QcTask :QcNode
    {
        public static readonly string NameField = "任务名称";
        public static readonly string TableName = "QC_PRO_TASK";
        public override  string Name
        {
            get { return row[QcTask.NameField].ToString(); }
            set { Row[QcTask.NameField] = value; }

        }
        public override string CodeField
        {
            get { return "任务编号"; }
        }
        public override  string Code
        {
            get
            {
                return this["任务编号"];
            }
            set
            {
                this["任务编号"] = value;
            }
        }
        public string 任务负责人
        {
            get
            {
                return QcUser.GetName(this["任务负责人"]);
            }           
        }
        public bool CanEdit(QcUser user)
        {
            if (user.Name == this["任务负责人"] || user.UserID == this["任务负责人"]) return true;
            if (user.Name == this["创建人"] || user.UserID == this["创建人"]) return true;
            if (user.HasPermission("项目管理")) return true;
            if (user.HasPermission("内置系统管理")) return true;
            if (Parent != null)
                if (Parent.CanEdit(user))
                    return true;
            return false;
        }
        public bool CanDelete(QcUser user)
        {
            if (user.Name == this["创建人"] || user.UserID == this["创建人"]) return true;
            if (user.HasPermission("项目管理")) return true;
            if (user.HasPermission("内置系统管理")) return true;
            if (Parent != null)
                if (Parent.CanEdit(user))
                    return true;
            return false;
        }
        public bool CanCreat(QcUser user)
        {
            if (user.HasPermission("创建项目")) return true;
            return false;
        }
        public bool CanCreatJob(QcUser user)
        {
            if (user.HasPermission("创建项目")) return true;
            if (user.Name == this["任务负责人"] || user.UserID == this["任务负责人"]) return true;
            return false;
        }
        public QcTask(QcProject parent) : base(null, TableName) { Parent = parent; }
        public void  SetParent(QcProject prj)
        {
            Parent=prj;
        }
        public QcTask(DataRow row)
            : base(row,TableName)
        {
            if (row == null) return;
        }
       public   QcProject Parent=null;
        private List<QcJob> m_Nodes = null;
        public List<QcJob> Nodes
        {
            get
            {
                 m_Nodes = QcJob.GetJob(this);
                if (m_Nodes == null) m_Nodes = new List<QcJob>();
                return m_Nodes;
            }
        }
        public static List<QcTask> GetTask(QcProject pro)
        {

            var pls = DbHelper.Query("Select * from " + TableName + " where 项目编号='" + pro.Code + "'");
            if (pls == null) return null;
            return (from p in pls select new QcTask(p) { Parent = pro }).ToList();
        }
        public static List<QcTask> GetTask(string expr)
        {
            return GetTaskBySql("Select * from " + TableName + ((expr != "") ? " where " + expr : ""));
        }
        public static QcTask GetTaskByid(string id)
        {
            try
            {
                return GetTaskBySql("Select * from " + TableName + " where 任务编号='" + id + "'").FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
        public static List<QcTask> GetTaskBySql(string sql)
        {
            var pls = DbHelper.Query(sql);
            if (pls == null) return null;
            return (from p in pls select new QcTask(p) { Parent = null }).ToList();
        }
        public string GetNextCode()
        {
            string prefix = DbHelper.GetDateTime().ToString("yyyy-MM-dd") + "-";
            return QcCode.GetNextNumber(prefix,
                QcTask.GetTask("任务编号 like '" + prefix+"%'"),
                11, 3, "000"
                );

        }
        public override  QcCheckResult Check(string field = null)
        {
            QcCheckResult result = new QcCheckResult(this);
            bool checkall = (field == null);
            result.AddCheckNull(field, new[] { "任务名称", "任务负责人", "任务状态", "任务优先级" });
            result.AddCheckUsed(field, Parent.Nodes, new[] { "任务名称" }, (IsNew() ? 0 : 1));
            result.AddCheckEnum(field, "任务负责人", QcUser.GetUserIDsFromNames(任务负责人Converter.任务负责人));
            result.AddCheckEnum(field, "任务状态", 任务状态Converter.任务状态);
            result.AddCheckEnum(field, "任务优先级", 任务优先级Converter.任务优先级);           
            if (result.Count > 0) return result;
            return null;
        }
        public override bool Update(QcDbTransaction trans = null)
        {
            bool isnew = false;
            if (IsNew())
            {
                this.Code = this.GetNextCode();
                this["创建人"] = QcUser.User.UserID;
                this["项目编号"] = Parent.Code;
                this["修改日期"] = DateTime.Now.ToLongDateString();
                isnew = true;
            }

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
            if (Parent != null)
            {
                if (ret == true && Parent.Nodes.Contains(this) == false)
                    Parent.Nodes.Add(this);
            }
            if (ret && isnew)
            {
                QcProjectTimeLine.WriteApointment(this.Parent.Code, this.创建人 + "对项目进行了确认并分配了新任务", this.Code);
                QcTaskTimeLine.WriteApointment(this.Code, this.创建人 + "创建了新任务");
                this.SendMessage(this.任务负责人, this.Name + "任务已创建，指定你为任务负责人");
            }
            return ret;
        }
        public override bool DeleteFromDb(QcDbTransaction trans = null)
        {

            if (this.创建人 == "sys" && QcUser.User.Name != "sys") return false;
            /// 遍历删除子元素
            bool ret;
            /// 遍历删除子元素
            if (Nodes.ToList().Any(t => t.DeleteFromDb(trans) == false)) return false;
            ret = base.DeleteFromDb(trans);
            if (ret) Parent.Nodes.Remove(this);
            return ret;
        }
        public object Tag { get; set; }
       
        /// <summary>
        /// 系统获取或创建内置系统任务
        /// </summary>
        /// <returns></returns>
        public static QcTask Get_CreatQuickTask()
        {

            QcProject prj = QcProject.GetProjects("项目通知单号='#*9999*#'").FirstOrDefault();
            if (prj == null)
            {
                prj = new QcProject();
                prj["项目名称"] = "系统内置快速项目";
                prj["项目通知单号"] = "#*9999*#";//特殊项目处理
                prj["项目负责人"] = "sys";
                prj["承担部门"] = "sys";
                prj["项目来源"] = "其它";
                prj["项目类型"] = "其它";
                prj["创建人"] = "1";
                if (prj.Update())
                {
                    QcTask task = new QcTask(prj);
                    task.Name = "系统内置快速任务";
                    task["任务描述"] = "内置任务不可删除";
                    task["任务通知单号"] = "#*9999*#";//特殊项目处理
                    task["任务负责人"] = "sys";
                    task["创建人"] = "sys";
                    task["任务优先级"] = "1";
                    task["任务状态"] = "未启动";
                    task.Update();
                    return task;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                QcTask task = QcTask.GetTask(prj).FirstOrDefault();
                if (task != null)
                {
                    return task;
                }
                else
                {
                    task = new QcTask(prj);
                    task.Name = "系统内置快速项目";
                    task["任务描述"] = "内置任务不可删除";
                    task["任务通知单号"] = "#*9999*#";//特殊项目处理
                    task["任务负责人"] = "sys";
                    task["创建人"] = "sys";
                    task["任务优先级"] = "1";
                    task["任务状态"] = "未启动";
                    task.Update();
                    return task;
                }

            }
        }
    }
}
