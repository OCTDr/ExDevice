////////////////////////////////////
//   用户类
//  abao++
///////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Data;
namespace QcPublic
{
    public class QcUser : QcNode,QcNet.INetUser
    {
        public static IEnumerable<string> GetUserIDsFromNames(IEnumerable<string> lstName)
        {
            return lstName.Select(t => GetUserID(t));
        }
        public static string GetName(string id_or_name)
        {
            var u = Users.Where(t => t.Name == id_or_name || t.UserID == id_or_name).FirstOrDefault();
            if(u==null)
            return id_or_name;
            return u.Name;
        }
        public static string GetUserID(string Name)
        {
            var u = Users.Where(t => t.Name==Name).FirstOrDefault();
            if (u == null)
                return Name;
            return u.UserID;
        }
        static public string NameField = "姓名";
        /// <summary>
        ///  当前用户
        /// </summary>
        static public string TableName { get { return "QC_USE_USERINFO"; } }
        static public QcUser User;
        public string HostName { get { return this["计算机名"]; } }
        public ushort Port
        {
            get
            {
                ushort port = 0;
                ushort.TryParse(this["Port"], out port); return port;
            }
        }
        public string IP { get { return this["IP"]; } }
        /// <summary>
        /// 角色编码
        /// </summary>
        public string UserID { get { return base["用户ID"]; } set { base["用户ID"] = value; } }
        public override string CodeField
        {
            get { return "用户ID"; }
        }
        public override  string Code { get { return UserID; } set { UserID = value; } }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string UserName { get { return base["用户名"]; } set { base["用户名"] = value; } }

        /// <summary>
        ///  姓名
        /// </summary>
        public string 姓名 { get { return base["姓名"]; } set { base["姓名"] = value; } }

        public override  string Name { get { return 姓名; } set { 姓名 = value; } }
        public string 部门 { 
            get
            {
                
                QcDepartment dp = QcDepartment.GetDepartmentFromCode(this["所在分组"]);                
                return (dp==null)?"":dp.Name;                
            }
            set
            {
                
                QcDepartment dp = QcDepartment.GetDepartmentFromName(value);
                if (dp != null) this["所在分组"] = dp.Code;
            }
        }
        List<QcRoleEnum> lstRoleEnums = null;
        List<QcRole> lstRoles = null;
        public List<QcRoleEnum> RoleEnums
        {
            get
            {
                if (lstRoleEnums == null) InitRoles();
                return lstRoleEnums;
            }
        }

        List<QcPermissionEnum> lstPermissionEnum = null;

        public List<QcPermissionEnum> PermissionEnums
        {
            get
            {
                if (lstPermissionEnum == null) RefreshPermission();
                return lstPermissionEnum;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code">角色编码</param>
        /// <param name="name">角色名称</param>
        /// <param name="content"> 备注</param>
        public  QcUser(DataRow row)
            : base(row, "QC_USE_USERINFO")
        {
            QcMessagner.Messagner.DataUpdate += Messagner_DataUpdate;
        }

        void Messagner_DataUpdate(object sender, QcMessagerDataUpdateEventArg e)
        {
           
                if (e.Node.TableName == "QC_USE_ROLE")
                {
                    var qup = e.Node as QcRole;
                    if (e.Node is QcNewNode)
                        qup = new QcRole(e.Node.GetRow());
                    var qre=QcRoleEnum.RolesEnums.FirstOrDefault(
                        t=>t.RoleCode==qup.RoleCode);
                    if(qre!=null)
                    {
                        qre.InitPermission();
                    }                    
                    if (qup.UserID == User.UserID)
                    {
                        this.InitRoles();
                    }                    
                }
                else if(e.Node.TableName=="QC_USE_PERMISSION")
                {
                    var qp = e.Node as QcPermission;
                    if (e.Node is QcNewNode)
                        qp = new QcPermission(e.Node.GetRow());
                    var qre=QcRoleEnum.RolesEnums.FirstOrDefault(
                        t=>t.RoleCode==qp.RoleCode);
                    if(qre!=null)
                    {
                        qre.InitPermission();
                    } 
                    var re = RoleEnums.FirstOrDefault(t => t.RoleCode == qp.RoleCode);
                    if (re != null)
                    {
                        re.InitPermission();
                        this.RefreshPermission();
                    }
                }
           
        }
        public QcUser() : this(null) { }
        /// <summary>
        /// 检测用户是否具有某种权限
        /// </summary>
        /// <param name="PermissionName"></param>
        /// <param name="optype"></param>
        /// <returns></returns>
        public bool HasPermission(string PermissionName,string optype="")
        {
            QcPermissionEnum.Regsiter(PermissionName,optype);
            if (PermissionEnums.Any(p => p.PermisssionEnum == PermissionName))
                return true;           
            return false;
        }
        /// <summary>
        /// 检查指定的字段是否合法
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public override  QcCheckResult Check(string field = null)
        {
            QcCheckResult result = new QcCheckResult(this);
            result.AddCheckNull(field, new[] { "用户名", "姓名" ,"所在分组","性别"});
            if(IsNew())
                result.AddCheckNull(field, new[] { "用户密码"});
            result.AddCheckUsed(field, QcUser.lstUsers, new[] { "用户名", "姓名" }, IsNew() ? 0 : 1);
            result.AddCheckEnum(field, "性别", 性别Converter.性别s);
            result.AddCheckEnum(field, "状态", 状态Converter.状态s);
            result.AddCheckEnum(field, "所在分组", from p in QcDepartment.Departments select p.Code);
            if (result.Count > 0) return result;
            return null;
        }
        /// <summary>
        /// 重载，完成分组编码和分组名称的转换
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
      
        void InitRoles()
        {

            lstRoleEnums = new List<QcRoleEnum>();
            lstRoles = new List<QcRole>();
            var sql = "select * from QC_USE_ROLE where 用户ID='" + this.UserID+"'";
            foreach (var v in DbHelper.Query(sql))
            {
                var qp = new QcRole(v);
                
                var qpe = QcRoleEnum.GetRoleFromCode(v.Field<string>("角色编码"));
                qp.roleenum = qpe;
                lstRoles.Add(qp);
                lstRoleEnums.Add(qpe);
            }
            RefreshPermission();
        }

        void RefreshPermission()
        {
            lstPermissionEnum = new List<QcPermissionEnum>();
            RoleEnums.ForEach(r => { r.PermissionEnums.ForEach(p => 
            { if (lstPermissionEnum.Contains(p) == false) lstPermissionEnum.Add(p); }
            ); });
        }
        public override bool Update(QcDbTransaction trans = null)
        {
            if (IsNew())
            {
                this.Code = QcUser.GetNextCode();
                this["用户密码"] = QcEncrypt.Md5Hash(this.UserName + QcEncrypt.Md5Hash(this["用户密码"]));
            }
            bool ret = base.Update(trans);
           //子节点调用AddRole 和RemoveRole进行同步，此处不管           
            if (ret == false && IsNew()) this.Code = "";
            if (ret && !Users.Contains(this)) Users.Add(this);
            //if (ret && !Users.Any(t=>t.UserName==this.UserName)) Users.Add(this);
            return ret;
        }
        public bool RemoveRole(string rolename)
        {
            QcRole role = this.lstRoles.FirstOrDefault(r => r.roleenum.RoleName == rolename);            
            if(role!=null)
            {
                role.DeleteFromDb();
                InitRoles();
            }
            return true;
        }
        public bool AddRole(string rolename,string content="")
        {
            QcRoleEnum role = QcRoleEnum.GetRoleFromName(rolename);
            if (role != null)
            {
                var qr = new QcRole();
                qr["用户ID"] = this.Code;
                qr["角色编码"] = role.RoleCode;
                qr["备注"] = content;
                qr.roleenum = role;
                bool ret=qr.Update();
                if (ret) InitRoles();
                return ret;
            }
            return false ;
        }
        static List<QcUser> lstUsers = null;
        public static List<QcUser> Users
        {
            get
            {
                if (lstUsers == null) InitUsers();
                return lstUsers; }
        }
        /// <summary>
        ///  从用户ID获取用户对象
        /// </summary>
        /// <param name="UsersCode">角色编码</param>
        /// <returns></returns>
        public static QcUser GetUsersFromID(string UserID)
        {
           InitUsers();
            return lstUsers.FirstOrDefault(u => u.UserID == UserID);
        }
        static public QcUser  RefreshUser(string strname)
        {
            var tu = lstUsers.FirstOrDefault(t => t.Name == strname);

            if (tu != null)
            {
                tu.Refresh();
                return tu;
            }
            string sql = "select * from " + TableName + " where 姓名='" + strname + "'";
            var users = DbHelper.Query(sql);
            if (users != null)
            {
                var u=users.FirstOrDefault();
                if (u != null)
                {
                    var user = new QcUser(u);
                    lstUsers.Add(user);
                    return user;
                }                
            }
            return null; ;
        }
        /// <summary>
        /// 初始化用户信息到内存
        /// </summary>
        static public void InitUsers()
        {         
            var Userss = DbHelper.Query("select * from QC_USE_USERINFO");
            if(Userss !=null)
                lstUsers = (from p in Userss select new QcUser(p)).ToList();
            if (lstUsers == null) lstUsers = new List<QcUser>();
        }

        public override bool DeleteFromDb(QcDbTransaction trans = null)
        {
            if(Users.Contains(this)) Users.Remove(this);
            return base.DeleteFromDb(trans);
        }
        public bool ChangePassword(string newpassword,string oldpassword)
        {
            string sql = "select * from QC_USE_USERINFO where 用户名='" + this.UserName + "' and 用户密码='" + QcEncrypt.Md5Hash(this.UserName + QcEncrypt.Md5Hash(oldpassword)) + "'";
            var v=DbHelper.Query(sql);
            if(v!=null)
            {
                if(v.Count()>0)
                {
                    QcUser u = new QcUser(v.First());
                    u["用户密码"] = QcEncrypt.Md5Hash(this.UserName + QcEncrypt.Md5Hash(newpassword));
                    return u.Update();
                }
            }        
            return false;
        }
        public static IEnumerable<string> GetUserByRole(string Role)
        {
            //获取具有指定权限的用户列表
            string sql = "select distinct u.* from QC_USE_USERINFO  u,QC_USE_ROLE  r,QC_USE_ROLEENUM  re " +
"where  r.用户id=u.用户id and r.角色编码=re.角色编码 and re.角色名称='"+Role+"'";
            var lst = DbHelper.Query(sql);
            if (lst == null) return new List<string>();
            return lst.Select(r => r.Field<string>(QcUser.NameField));
        }

        public static IEnumerable<string> GetUserByPermission(string Permission)
        {
            //获取具有指定权限的用户列表
            string sql = "select distinct u.* from QC_USE_USERINFO  u,QC_USE_ROLE r,QC_USE_PERMISSION  p,QC_USE_PERMISSIONENUM  pe "+
"where pe.权限枚举='"+Permission+ "' and pe.权限编码=p.权限编码 and p.角色编码=r.角色编码 and r.用户id=u.用户id";
           var lst= DbHelper.Query(sql);
            if(lst==null) return new List<string>();
            return lst.Select(r => r.Field<string>(QcUser.NameField));
        }
       
   /// <summary>
   ///  获取下一个可用的编码
   /// </summary>
   /// <returns></returns>
        static string GetNextCode()
        {
            return QcCode.GetNextNumber(Users);
        }
     
        /// <summary>
        /// 登录为用户
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static QcUser LoginAs(string username, string password)
        {
            if (SqlChecker.CheckKeyword(username) == false) return null;
            string sql = "select * from QC_USE_USERINFO where 用户名='" + username + "' and 用户密码='" + QcEncrypt.Md5Hash(username+QcEncrypt.Md5Hash(password))+"' and 状态='启用'" ;
            if (DbHelper.Exists(sql) == false) return null;
            InitUsers();
            return  Users.FirstOrDefault(t => t.UserName == username);           
        }
        public static void LoginMsg()
        {
           QcMessagner .Messagner .SendMsg("*","上线",QcNet.QcProtocol.QcCommand.QcUserLogin);
        }
        public static void LogoutMsg()
        {
            QcMessagner.Messagner.DisConection();
        }
        public static bool Regsister(string username,string password,string name,string sex,DateTime birthday,string tel,string part,DateTime worktime,string address,string content,string status="注册")
        {
            if (QcUser.Users.Any(t => t.UserName == username || t.姓名 == name)) return false;
            QcUser user=new QcUser();
            user.UserName=username;
            user.部门=part;
            user.姓名=name;
            user["用户密码"]=password;
            user["性别"]=sex;
            user["出生日期"]=birthday.ToQcDateString();
            user["参加工作时间"]=worktime.ToQcDateString();
            user["联系电话"]=tel;
            user["通讯地址"]=address;
            user["备注"] = content;
            user.UserID = QcUser.GetNextCode();
            user["状态"] = status;
            //user["状态"] = "启用";
            bool ret=user.Update();
            if (ret)
            {
                user.AddRole("项目负责人");
                user.AddRole("任务负责人");
                user.AddRole("作业员");
            }
            //if (QcProject.GetProjects("").Any(t => t.Name == "快速项目") == false)
            //{
            //    QcProject prj = new QcProject();
            //    prj["项目名称"] = "快速项目";
            //    prj["项目负责人"] = "sys";
            //    prj["承担部门"] = "临时";
            //    prj["项目来源"] = "其它";
            //    prj["项目类型"] = "其它";
            //    prj["创建人"] = "sys";
            //    if (prj.Update())
            //    {
            //        QcTask task = new QcTask(prj);
            //        task.Name = "快速任务";
            //        task["任务负责人"] = "sys";
            //        task["创建人"] = "sys";
            //        task["任务优先级"] = "1";
            //        task["任务状态"] = "未启动";
            //        task.Update();
            //    }
            //}
            QcTask.Get_CreatQuickTask();
            QcMsgPoster.PostMeassage(user.Code, user.tablename, user.CodeField, NodeChangeType.Create);
            return ret;
        }
    }
}
