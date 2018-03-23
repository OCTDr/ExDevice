////////////////////////////////////
//   权限类
//  abao++
///////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace QcPublic
{
    public class QcPermission : QcNode
    {
    
        public QcPermissionEnum PermissionEnum = null;
        public QcPermission(DataRow row = null)
            : base(row, "QC_USE_PERMISSION")
        {
        }

        public override string Code
        {
            get
            {
                return RoleCode + ';' + PermisssionCode;
                ;
            }
            set
            {

            }
        }
        public override string CodeField
        {
            get { return "角色编码;权限编码"; }
        }
        public string RoleCode
        {
            get { return this["角色编码"]; }
            set { this["角色编码"] = value; }
        }
        public string PermisssionCode { get { return this["权限编码"]; } set { this["权限编码"] = value; } }
        public string Content { get { return this["备注"]; } set { this["备注"] = value; } }

        protected override string GetRefreshSql()
        {
            return "select * from QC_USE_PERMISSION where 角色编码='"
                          + this.RoleCode + "' and 权限编码='" + this.PermisssionCode + "'";
        }

    }
    public class QcPermissionEnum : QcNode
    {
        
        public override  string Code
        {
            get { return PermisssionCode; }
            set { PermisssionCode = value; }
        }
        public override string CodeField
        {
            get { return "权限编码"; }
        }
        /// <summary>
        /// 权限编码
        /// </summary>
        public string PermisssionCode
        {
            get { return this["权限编码"]; }
            set { this["权限编码"] = value; }
        }
        /// <summary>
        /// 操作类型
        /// </summary>
        public string OperationType{ get
        {
            return row.Field<string>("操作类型");
        }
            set { this["操作类型"]=value;}
        }
        public override  string Name { get { return PermisssionEnum; } set { PermisssionEnum = value; } }
        /// <summary>
        ///  权限枚举
        /// </summary>
        public string PermisssionEnum
        {
            get
            { return row.Field<string>("权限枚举"); }
            set { this["权限枚举"]=value;}
        }
        /// <summary>
        ///  备注
        /// </summary>
        public string Content { get { return row.Field<string>("备注"); } set { this["备注"]=value ;} }
        private  QcPermissionEnum(DataRow row)
            : base(row, "QC_USE_PERMISSIONENUM")
        {
        }
        /// <summary>
        ///  创建权限类
        /// </summary>
        /// <param name="code">权限编码</param>
        /// <param name="type">操作类型</param>
        /// <param name="permissionenum">权限枚举</param>
        /// <param name="content">备注</param>
        private QcPermissionEnum(string permissionenum, string type="", string content = "") :
            base(null, "QC_USE_PERMISSIONENUM")
        {
            PermisssionCode = QcPermissionEnum.GetNextCode();
            OperationType = type;
            PermisssionEnum = permissionenum;
            Content = content;
        }
        /// <summary>
        /// 检查指定的字段是否合法
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public override  QcCheckResult Check(string field=null)
        {
            QcCheckResult result=new QcCheckResult(this);
            result.AddCheckNull(field,new[] { "权限枚举" });
            result.AddCheckUsed(field, QcPermissionEnum.Permissions, new[] { "权限枚举" }, 0);
            if (result.Count > 0) return result;
            return null;
        }

        public override bool Update(QcDbTransaction trans = null)
        {
            if (IsNew()) this.Code = QcPermissionEnum.GetNextCode();
            bool ret = base.Update(trans);
            if (ret == false && IsNew()) this.Code = "";
            //新建的加入到列表
            if (ret && !Permissions.Contains(this))
                Permissions.Add(this);
            return ret;
        }
        static List<QcPermissionEnum> m_lstPermission=null;
        public static List<QcPermissionEnum> Permissions
        {
            get
            {
                if (m_lstPermission == null)
                    InitPermission();
                return m_lstPermission;
            }
        }
        
        static string GetNextCode()
        {
            return QcCode.GetNextNumber(Permissions);
        }
        /// <summary>
        ///  按照权限编码获取权限类
        /// </summary>
        /// <param name="PermissionCode"></param>
        /// <returns></returns>
        public  static QcPermissionEnum GetPermissionFromCode(string PermissionCode)
        {
            return Permissions.FirstOrDefault(t => t.PermisssionCode == PermissionCode);            
        }
        public static QcPermissionEnum GetPermissionFromName(string PermissionName)
        {
            return Permissions.FirstOrDefault(t => t.PermisssionEnum  == PermissionName);
        }
        /// <summary>
        /// 初始化权限列表，
        /// </summary>
        static private void InitPermission()
        {
            m_lstPermission = new List<QcPermissionEnum>();
            var permissions = DbHelper.Query("select * from QC_USE_PERMISSIONENUM");
            foreach (var v in permissions)
            {
                var qp=new QcPermissionEnum(v);
                Permissions.Add(qp);
            }
        }
        /// <summary>
        /// 注册一个新的权限类型，可以直接调用，中程序请求该类程序验证时，会被创建到角色表
        /// </summary>
        /// <param name="name">权限枚举</param>
        /// <param name="optype">操作类型</param>
        /// <returns></returns>
        static public bool Regsiter(string name,string optype="默认")
        {
            if (Permissions.Any(t => t.PermisssionEnum == name)) return false;
            QcPermissionEnum p = new QcPermissionEnum(name, optype);
            if (p.Update())
            {
                Permissions.Add(p);
                return true;
            }
            return false;
        }
    }
}
