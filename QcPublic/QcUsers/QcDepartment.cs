////////////////////////////////////
//   部门类
//  abao++
///////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace QcPublic
{
    public class QcDepartment :QcNode
    {
       
        /// <summary>
        /// 角色编码
        /// </summary>
        public string PartCode { get { return this["分组编码"]; } set{this["分组编码"]=value; }}
        public override string Code { get { return PartCode; } set { PartCode = value; } }
        public override string CodeField
        {
            get { return "分组编码"; }
        }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string PartName { get { return this["组名"]; } set { this["组名"] = value; } }

        public override  string Name { get { return PartName; } set { PartName = value; } }
        /// <summary>
        ///  备注
        /// </summary>
        public string PartContent { get { return Row.Field<string>("备注"); } set { this["备注"] = value; } }

        public QcDepartment(DataRow row)
            : base(row, "QC_USE_Department")
        {
        }
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="content"></param>
        private QcDepartment(string name, string content=""):this(null)
        {
            PartName = name;
            PartContent = content;
        }
        public QcDepartment()
            : this(null) 
        { 
        }

        static string GetNextCode()
        {
            //return QcCode.GetNextPartNumber(null, lstDepartment , 0);
            return QcCode.GetNextNumber("", Departments, 0, 2, "");
        }
        /// <summary>
        /// 检查指定的字段是否合法
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public override QcCheckResult Check(string field = null)
        {
            QcCheckResult result = new QcCheckResult(this);
            result.AddCheckNull(field, new[] { "组名" });
            result.AddCheckUsed(field, QcDepartment.lstDepartment, new[] { "组名" }, IsNew() ? 0 : 1);
            if (result.Count > 0) return result;
            return null;
        }
        static List<QcDepartment> lstDepartment = null;
        public static List<QcDepartment> Departments
        {
            get
            {
                if (lstDepartment == null) InitDepartment();
                return lstDepartment; }
        }
        /// <summary>
        ///  从部门编码获取部门
        /// </summary>
        /// <param name="RoleCode">角色编码</param>
        /// <returns></returns>
         public static QcDepartment GetDepartmentFromCode(string DepartmentCode)
        {
          
            return Departments.FirstOrDefault(d => d.PartCode == DepartmentCode);
        }
         public static QcDepartment GetDepartmentFromName(string DepartmentName)
         {

             return Departments.FirstOrDefault(d => d.PartName== DepartmentName);
         }
         public override bool DeleteFromDb(QcDbTransaction trans = null)
         {
             Departments.Remove(this);
             return base.DeleteFromDb(trans);
         }
         public override bool Update(QcDbTransaction trans=null)
         {
             if (IsNew()) this.Code =QcDepartment.GetNextCode();
             bool ret = base.Update(trans);
             if (ret == false && IsNew()) this.Code = "";
             if (!Departments.Contains(this)) Departments.Add(this);
             return ret;
         }
        public static bool  AddNewDepartment(string name, string content="")
         {
             QcDepartment d = new QcDepartment(name, content);
             bool ret = d.Update();
             return ret;
         }
         static private void InitDepartment()
        {
            lstDepartment =new List<QcDepartment>();
            var Departments = DbHelper.Query("select * from QC_USE_Department");
            foreach (var v in Departments)
            {
                var qp = new QcDepartment(v);
                lstDepartment.Add( qp);
            }
        }

    }
}
