////////////////////////////////////
//   角色类
//  abao++
///////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace QcPublic
{
    public class QcRole:QcNode
    {        
        public QcRoleEnum roleenum = null;
           public QcRole(DataRow row=null)
            : base(row, "QC_USE_ROLE")
        {

        }
           public override string Code
           {
               get
               {
                   return this.UserID + ";" + this.RoleCode;
               }
               set
               {
                   throw new NotImplementedException();
               }
           }
           public override string CodeField
           {
               get { return "用户ID;角色编码"; }
           }

           public string UserID { get { return this["用户ID"]; } }
           public string RoleCode { get { return this["角色编码"]; } }

           protected override string GetRefreshSql()
           {
               return "select * from QC_USE_Role where 角色编码='"
                             + this.RoleCode + "' and 用户ID='" + this.UserID + "'";
           }
    }

    public class QcRoleEnum :QcNode
    {
        /// <summary>
        /// 角色编码
        /// </summary>
        public string RoleCode { get{return row.Field<string>("角色编码");} set{this["角色编码"]=value;} }
        public override string CodeField
        {
            get { return "角色编码"; }
        }
        public override  string Code { get { return RoleCode; } set { RoleCode = value; } }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get { return Row.Field<string>("角色名称"); } set { this["角色名称"] = value; } }

        public override  string Name { get { return RoleName; } set { RoleName = value; } }

        public override bool DeleteFromDb(QcDbTransaction trans = null)
        {
            if (base.DeleteFromDb(trans))
            {
                InitRole();
                return true;
            }
            else
            {
                return false;
            }
        }
      
        /// <summary>
        ///  备注
        /// </summary>
        public string RoleContent{get{return Row.Field<string>("备注");}set{this["备注"]=value;}}


       
           public List<QcPermissionEnum> PermissionEnums
        {
            get
            {
                return InitPermission();              
            }
        }
        private QcRoleEnum(DataRow row)
            : base(row, "QC_USE_ROLEENUM")
        {
           
        }
        public QcRoleEnum() : this(null) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code">角色编码</param>
        /// <param name="name">角色名称</param>
        /// <param name="content"> 备注</param>
        public  QcRoleEnum(string name, string content=""):this(null)
        {
            RoleCode =QcRoleEnum.GetNextCode();
            RoleName = name;
            RoleContent = content;
        }

        public List<QcPermissionEnum> InitPermission()
        {
            List<QcPermissionEnum> lstPermissionEnum =  new List<QcPermissionEnum>();        
            var sql = "select * from QC_USE_PERMISSION where 角色编码='" + this.RoleCode+"'";
            foreach (var v in DbHelper.Query(sql))
            {
                var qp = new QcPermission(v);
                var qpe = QcPermissionEnum.GetPermissionFromCode(v.Field<string>("权限编码"));
                qp.PermissionEnum = qpe;
                lstPermissionEnum.Add(qpe);
            }
            return lstPermissionEnum;
        }

         public override QcCheckResult Check(string field = null)
         {
             QcCheckResult result = new QcCheckResult(this);
             result.AddCheckNull(field, new[] { "角色名称" });
             result.AddCheckUsed(field, QcRoleEnum.lstRoleEnums, new[] { "角色名称" }, IsNew()?0:1);
             if (result.Count > 0) return result;
             return null;
         }
        
        public bool AddPermission(string Permission,string content="")
         {
            QcPermissionEnum pe=QcPermissionEnum.GetPermissionFromName(Permission);
            if(pe!=null)
            {
                QcPermission p = new QcPermission();
                p.RoleCode = this.RoleCode;
                p.PermisssionCode = pe.PermisssionCode;
                p.Content = content;             
                bool ret= p.Update();
                if (ret)
                 return ret;
            }
            return false;
         }
        public bool RemovePermission(string PermissionName)
        {
            QcPermissionEnum pe = QcPermissionEnum.GetPermissionFromName(PermissionName);
            if (pe != null)
            {
                string sql = "delete  QC_USE_PERMISSION where 角色编码='" + this.RoleCode
                    + "' and 权限编码='" + pe.Code + "'";
                if (DbHelper.Execute(sql))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
         public override bool Update(QcDbTransaction trans = null)
         {
             if (IsNew()) this.Code = QcRoleEnum.GetNextCode();
             bool ret = base.Update(trans);
             if (ret == false && IsNew()) this.Code = "";
             //新建的角色加入到列表
             if (ret && !RolesEnums.Contains(this))
                 RolesEnums.Add(this);
             
             return ret;
         }

         static string GetNextCode()
         {
             return QcCode.GetNextNumber(lstRoleEnums);
         }
         static List<QcRoleEnum> lstRoleEnums = null;
         public static List<QcRoleEnum> RolesEnums
         {

             get
             {
                 if (lstRoleEnums == null)
                     InitRole();
                 return lstRoleEnums;

             }
         }
        /// <summary>
        ///  从角色编码获取角色对象
        /// </summary>
        /// <param name="RoleCode">角色编码</param>
        /// <returns></returns>
        public static QcRoleEnum GetRoleFromCode(string RoleCode)
        {
           
            return RolesEnums.FirstOrDefault(r => r.RoleCode == RoleCode);
            
        }
        /// <summary>
        ///  从角色编码获取角色对象
        /// </summary>
        /// <param name="RoleCode">角色编码</param>
        /// <returns></returns>
        public static QcRoleEnum GetRoleFromName(string RoleName)
        {
            
            return RolesEnums.FirstOrDefault(r => r.RoleName == RoleName);

        }
        static private void InitRole()
        {
            if(lstRoleEnums==null)
            {
                QcNode.NodeUpdateToDb += QcNode_NodeUpdateToDb;
            }
            lstRoleEnums = new List<QcRoleEnum>();
            var Roles = DbHelper.Query("select * from QC_USE_RoleENUM");
            foreach (var v in Roles)
            {
                var qr = new QcRoleEnum(v);
                qr.InitPermission();
                lstRoleEnums.Add(qr);
            }          
        }

        static void QcNode_NodeUpdateToDb(object sender, QcNodeEventArg e)
        {
            if(e.ChangeType==NodeChangeType.Create)
            {
                if(e.Node.TableName=="QC_USE_PERMISSION")
                {
                    var qup = e.Node as QcPermission;
                    var role = lstRoleEnums.Find(t => t.Code == qup.RoleCode);
                    role.Refresh();
                }
            }
        }

    }
}
