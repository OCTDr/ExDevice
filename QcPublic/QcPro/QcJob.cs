//2014-6-20
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace QcPublic
{
    public class QcJob : QcNode
    {
        public static readonly string NameField = "作业名称";
        public static readonly string TableName = "QC_PRO_JOB";
        public override string Name
        {
            get { return row[QcJob.NameField].ToString(); }
            set { Row[QcJob.NameField] = value; }

        }
        public override string CodeField
        {
            get { return "作业编号"; }
        }
        public override string Code
        {
            get
            {
                return this["作业编号"];
            }
            set
            {
                this["作业编号"] = value;
            }
        }
        public QcJob(QcTask parent) : base(null, TableName) { Parent = parent; }
        public QcJob(DataRow row)
            : base(row,TableName)
        {
            if (row == null) return;
        }
      
        QcTask Parent = null;
        public static List<QcJob> GetJob(QcTask task)
        {

            var pls = DbHelper.Query("Select * from " + TableName + " where 任务编号='" + task.Code + "'");
            if (pls == null) return null;
            return (from p in pls select new QcJob(p) { Parent = task }).ToList();
        }
        public void SetParent(QcTask task)
        {
            Parent = task;
        }
        public QcTask GetParent()
        {
            if (Parent != null)
            {
                return Parent;
            }
            else
            {
                return QcTask.GetTask("任务编号='" + this["任务编号"] + "'").FirstOrDefault();

            }
        }
        public bool CanEdit(QcUser user)
        {
            if (user.Name == this["作业员"] || user.UserID == this["作业员"]) return true;
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
            if (user.HasPermission("创建项目")) return true;
            return false;
        }

        public static IEnumerable<QcJob> GetJob(QcUser user)
        {
            string name = user.UserID;
             string sql = "select distinct j.* from " + QcProject.TableName + " p," 
        + QcTask.TableName + " t," + QcJob.TableName + " j where (p.项目编号=t.项目编号 and" 
        + " t.任务编号=j.任务编号) and (t.任务负责人=' " + name + "' or t.创建人='"+name+ 
        "' or p.项目负责人='"+name +"' or p.创建人='"+name+"' or j.创建人='"+name+
        "' or j.作业员='" + name + "') order by j.修改日期 desc";
             var rows = DbHelper.Query(sql);
             return rows.Select(t => new QcJob(t));
        }
        public static List<QcJob> GetJob(string expr)
        {

           // QcLog.LogString(":"+expr);
            var pls = DbHelper.Query("Select * from " + TableName + ((expr != "") ? " where " + expr: ""));
            if (pls == null) return new List<QcJob>();
            return (from p in pls select new QcJob(p) { Parent = null }).ToList();
        }
        public static QcJob GetJobByid(string expr)
        {

            // QcLog.LogString(":"+expr);
            var pls = DbHelper.Query("Select * from " + TableName + ((expr != "") ? " where 作业编号='" + expr + "'" :""));
            if (pls == null) return null;
            {
                return (from p in pls select new QcJob(p) { Parent = null }).ToList().FirstOrDefault();
            }
        }

        public static List<QcJob> GetMyJob(QcUser user)
        {
            return GetJob(string.Format("作业员='{0}'",user .UserID));
        }
        public static List<QcJob> GetMyJob(QcUser user,string state)
        {
            return GetJob(string.Format("作业员='{0}' and 作业状态='{1}'", user.UserID,state));
        }
        public IEnumerable<string> Get方案IDS()
        {
            var rows = DbHelper.Query("select c.方案ID,c.方案名称,c.产品级别编码,c.产品类别编码,t.产品类别名称,l.产品级别名称 from qc_ins_checkproject c,QC_PRO_PRODUCTTYPE  t," +
                    "QC_PRO_PRODUCTLevel  l where c.产品级别编码=l.产品级别编码 and c.产品类别编码=t.产品类别编码 and (c.产品类别编码='"
                    + ((this["产品类别编码"] == null) ? "" : this["产品类别编码"])
                    + "' and c.产品级别编码='" +
                    ((this["产品级别编码"] == null) ? "" : this["产品级别编码"]) + "')"
                    );
            return from s in rows select s["方案ID"].ToString();
        }
        public override  QcCheckResult Check(string field = null)
        {
            QcCheckResult result = new QcCheckResult(this);
            bool checkall = (field == null);
            result.AddCheckNull(field, new[] { "作业名称", "作业员", "作业状态", "作业优先级" });
            result.AddCheckUsed(field, this.GetParent().Nodes, new[] { "作业名称" }, (IsNew() ? 0 : 1));
            result.AddCheckEnum(field, "作业员", QcUser.GetUserIDsFromNames(作业员Converter.作业员));
            result.AddCheckEnum(field, "作业状态", 作业状态Converter.作业状态);
            result.AddCheckEnum(field, "作业优先级", 作业优先级Converter.作业优先级);
            result.AddCheckEnum(field, "启动类型", 启动类型Converter.启动类型);
            result.AddCheckEnum(field, "产品级别编码", 产品级别编码Converter.级别s.Select(t => t.Code).ToArray());
            result.AddCheckEnum(field, "产品类别编码", 产品类别编码Converter.类别s.Select(t => t.Code).ToArray());
            result.AddCheckEnum(field, "方案ID", Get方案IDS());
            if (result.Count > 0) return result;            
            return null;
        }
        public void SetCurProductLevel()
        {
            产品级别编码Converter.select = 产品级别编码Converter.级别s.Where(t => t.Code == this["产品级别编码"]).FirstOrDefault();
            产品类别编码Converter.select=产品类别编码Converter.类别s.Where(t=>t.Code ==this["产品类别编码"]).FirstOrDefault();
        }
            public string GetNextCode()
        {
            string prefix=DbHelper.GetDateTime().ToString("yyyy-MM-dd")+"-";
            return QcCode.GetNextNumber(prefix,
                QcJob.GetJob("作业编号 like '" + prefix+"%'"),
                11, 4, "0000"
                );

        }
            public override bool Update(QcDbTransaction trans = null)
            {
                bool isnew = false;
                if (IsNew())
                {
                    isnew = true;
                    this.Code = this.GetNextCode();
                    this["创建人"] = QcUser.User.UserID;
                    this["修改人"] = QcUser.User.UserID;
                    this["任务编号"] = Parent.Code;
                    this["项目编号"] = Parent["项目编号"];
                    this["修改日期"] = DateTime.Now.ToLongTimeString();
                }
                bool ret = base.Update(trans);
                if (ret == false && IsNew())
                {
                    this.Code = "";
                    return false;
                }
                if (Parent != null)
                {
                    if (ret == true && Parent.Nodes.Contains(this) == false)
                    {
                        Parent.Nodes.Add(this);
                    }
                }
                if (ret && isnew)
                {
                    QcTaskTimeLine.WriteApointment(this.Parent.Code, this.创建人 + "对任务进行了确认并分配了新作业", this.Code);
                    QcJobTimeLine.WriteApointment(this.Code, this.创建人 + "创建了新作业");
                   
                    this.SendMessage(this["作业员"], this.Name + "作业已创建，指定你为作业负责人");
                }
                return ret;
            }         
            public override bool DeleteFromDb(QcDbTransaction trans = null)
            {
                /// 遍历删除子元素
                bool ret;
                /// 遍历删除子元素
                ret = base.DeleteFromDb(trans);
                if (ret && Parent !=null) Parent.Nodes.Remove(this);
                return ret;
            }
            public object Tag { get; set; }
        /// <summary>
        /// 根据启动类型自动创建系统作业，但缺省了方案ID，不成功时返回null
        /// </summary>
        /// <param name="starttype"></param>
        /// <returns></returns>
            public static  QcJob Get_CreatQuickJob(string starttype)
            {

                QcTask task = QcTask.Get_CreatQuickTask();
                if (task != null)
                {
                    QcJob chekjob = QcJob.GetJob(task).Where(t => t["启动类型"] == starttype).FirstOrDefault();
                    if (chekjob == null)
                    {
                        QcJob job = new QcJob(task);
                        job.Name = string.Format("我的 {0} 快速作业", starttype);
                        job["启动类型"] = starttype;
                        job["作业员"] = QcUser.User.UserID;
                        job["作业优先级"] = "1";
                        job["作业状态"] = "未启动";
                        job["作业描述"] = "该作业为系统自动分配的快速作业";
                        job["备注"] = "#*9999*#";
                        job.Update();
                        return job;
                    }
                    else
                    {
                        return chekjob;
                    }

                }
                else
                {
                    return null;
                }
            }
        /// <summary>
        /// 根据名称，类型，方案创建作业，不成功时返回null
        /// </summary>
        /// <param name="jobname"></param>
        /// <param name="starttype"></param>
        /// <param name="chkprjid"></param>
            /// <returns></returns>
            public static QcJob CreatQuickJob(string jobname, string starttype,string productlevelcode, string producttypecode,string chkprjid)
            {
                QcTask task = QcTask.Get_CreatQuickTask();
                if (task != null)
                {
                    QcJob job = new QcJob(task);
                    job.Name = jobname;
                    job["启动类型"] = starttype;
                    job["作业员"] = QcUser.User.UserID;
                    job["产品级别编码"] = productlevelcode;
                    job["产品类别编码"] = producttypecode;
                    job["方案ID"] = chkprjid;
                    job["作业优先级"] = "1";
                    job["作业状态"] = "未启动";
                    job["修改日期"] = DateTime.Now.ToString();   
                    job.Update();
                    return job;
                }
                else
                {
                    return null;
                }
            }

    }
}
