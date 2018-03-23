////////////////////////////////////
//   使用动态类来把一个数据行转变成一个动态类
//  abao++
///////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Dynamic;
namespace QcPublic
{

    public static class DataRowExpand/*数据行的扩展*/
    {
        static public ExpandoObject ToObject(this DataRow row)
        {
            dynamic obj = new ExpandoObject();
            IDictionary<string, Object> idic = obj as IDictionary<string, object>;
            obj.Row = row;
            foreach (DataColumn c in row.Table.Columns)
            {
                idic.Add(c.ColumnName, row[c, DataRowVersion.Current]);
            }

            return obj;
        }

    }
    /// <summary>
    ///  动态的数据行包装类
    /// </summary>
    public class DynamicDataRowObject : DynamicObject
    {
        protected DataRow row = null;
        /// <summary>
        ///  对应的数据行
        /// </summary>
        protected DataRow Row { get { return row; } }
        /// <summary>
        ///  表名
        /// </summary>
        protected string tablename = null;
        /// <summary>
        /// 这是一个缓存表结构的部分，避免每一次新建对象需要去查询数据库获取其结构
        /// </summary>
        ///static Dictionary<string, DataRow> lstTable = new Dictionary<string, DataRow>();
        /// <summary>
        ///  获取一个新行作为新建对象的数据容器
        /// </summary>
        /// <param name="tablename">数据表明</param>
        /// <returns></returns>
        static DataRow GetNewDataRow(string tablename)
        {
            ///这里采用查询语句获取一行来作为数据库容器，因为包装类需要datatable的列信息，包括字段名称和类型等
            DataRow row = null;
            //lstTable.TryGetValue(tablename, out row);

            if (row == null)
            {
                DataTable dt;
                if (DbHelper.DBType == DatabaseType.Oracle)
                {
                    dt = DbHelper.GetDataTable("select * from " + tablename + " where rownum=1");
                }
                else if (DbHelper.DBType == DatabaseType.MsSql)
                {
                    dt = DbHelper.GetDataTable("select top 1 * from " + tablename + "");
                }
                else
                    dt = DbHelper.GetDataTable("select top 1 * from " + tablename + "");
                row = dt.NewRow();
            }
            return row.Table.NewRow();
        }
        /// <summary>
        ///  虚拟的访问，使用字段名称作为索引访问
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual string this[string key]
        {
            get
            {
                if (key == null)
                    key = row.Table.Columns[0].ColumnName;
                if (row == null)
                    return "";

                if (row.Table.Columns.Contains(key))
                {
                    if (row[key] is DBNull)
                        return "";
                    return row[key].ToString();
                }
                else
                {
                    QcLog.LogString(this.tablename + "不包含字段" + key);
                }
                return "";
            }
            set
            {
                if (row == null)
                    return;
                if (row.Table.Columns.Contains(key))
                {
                    if (row.Table.Columns[key].DataType == typeof(DateTime))
                    {
                        DateTime date = new DateTime();
                        if (DateTime.TryParse(value, out date))
                        {
                            row[key] = value;
                        }
                        else
                            row[key] = DBNull.Value;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(value))
                            row[key] = DBNull.Value;
                        else row[key] = value;
                    }
                }
                else
                    throw new Exception("数据库中不存在对应的字段");
            }

        }
        /// <summary>
        ///  检查指定的字段值是否为空
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <returns>为空则返回字段名称和错误信息</returns>
        public string CheckNull(string field)
        {
            if (this[field] == "") return "值不能为空";
            return "";
        }
        /// <summary>
        ///  返回当前的行
        /// </summary>
        /// <returns></returns>
        public DataRow GetRow()
        {
            return Row;
        }

        public DynamicDataRowObject()
        {

        }
        /// <summary>
        ///  从指定的行构建一个动态对象
        /// </summary>
        /// <param name="row">数据行</param>
        /// <param name="tablename">数据表名</param>
        public DynamicDataRowObject(DataRow row, string tablename = null)
        {
            if (row == null)
            {
                if (tablename == null) return;
                row = DynamicDataRowObject.GetNewDataRow(tablename);
            }
            this.row = row;
            if (tablename == null)
                tablename = row.Table.TableName;
            this.tablename = tablename;
        }
        /// <summary>
        ///  动态访问数据元素的表
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (row == null)
            {
                result = null;
                return false;
            }
            if (Row.Table.Columns.Contains(binder.Name))
            {
                result = Row.Field<object>(binder.Name);
                return true;
            }
            result = null;
            return false;
        }
        /// <summary>
        /// 动态设置数据元素值
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (row == null) return false;
            if (Row.Table.Columns.Contains(binder.Name))
            {
                Row[binder.Name] = value;
                return true;
            }
            return false;
        }
        /// <summary>
        ///  生成更新的sql
        /// </summary>
        /// <returns></returns>
        private string MakeUpdateSql()
        {
            string prefix = "";
            string suffix = "";
            if (DbHelper.DBType == DatabaseType.Mdb)
            {
                prefix = "[";
                suffix = "]";
            }
            if (row == null || tablename == null) return "";
            string TableName = this.tablename;
            StringBuilder updatesql = new StringBuilder(200);
            updatesql.Append("Update ");
            updatesql.Append(TableName);
            updatesql.Append(" Set ");
            bool updateflag = false;
            foreach (DataColumn c in row.Table.Columns)
            {
                string split = "";

                if (c.DataType == typeof(string))
                    split = "\'";
                else if (c.DataType == typeof(DateTime))
                    if (DbHelper.DBType == DatabaseType.Mdb || DbHelper.DBType == DatabaseType.Accdb)
                    {
                        split = "#";
                    }
                    else
                        split = "\'";
                if (row[c, DataRowVersion.Original].Equals(row[c, DataRowVersion.Current]))
                {
                }
                else
                {
                    if (updateflag)
                    {
                        updatesql.Append(",");
                    }
                    else
                    {
                        updateflag = true;
                    }

                    if (row[c, DataRowVersion.Current] == null || row[c, DataRowVersion.Current] is System.DBNull)
                    {
                        updatesql.Append(prefix + c.ColumnName + suffix + "=null");
                    }
                    else
                    {
                        if (DbHelper.DBType == DatabaseType.Oracle && c.DataType == typeof(DateTime))
                        {
                            updatesql.Append(prefix + c.ColumnName + suffix + "=to_date('" + ((DateTime)row[c, DataRowVersion.Current]).ToQcDateString() + "','yyyy-mm-dd hh24:mi:ss')");
                        }
                        else
                            updatesql.Append(prefix + c.ColumnName + suffix + "=" + split + row[c, DataRowVersion.Current] + split + " ");
                    }
                }
            }
            updatesql.Append(" where ");
            updatesql.Append(GetExpr());
            if (updateflag)
                return updatesql.ToString();//99999999999999
            else
                return "";
        }
        /// <summary>
        ///  判断是否为新建的行
        /// </summary>
        /// <returns></returns>
        public bool IsNew()
        {
            return row.RowState == DataRowState.Detached;
        }
        /// <summary>
        ///  判断是否发生了编辑更新
        /// </summary>
        /// <returns></returns>
        public bool IsEdit()
        {
            return row.RowState == DataRowState.Modified;
        }
        /// <summary>
        /// 生成删除的sql语句
        /// </summary>
        /// <returns></returns>
        private string MakeDeleteSql()
        {

            if (row == null || tablename == null) return "";
            string TableName = tablename;
            StringBuilder updatesql = new StringBuilder(200);
            updatesql.Append("Delete from ");
            updatesql.Append(TableName);
            string expr = GetExpr();
            updatesql.Append(" where ");
            updatesql.Append(expr);
            return updatesql.ToString();
        }
        protected virtual  string GetExpr()
        {
            string prefix = "";
            string suffix = "";
            string expr = "";
            if (DbHelper.DBType == DatabaseType.Mdb || DbHelper.DBType == DatabaseType.Accdb)
            {
                prefix = "[";
                suffix = "]";
            }
            foreach (DataColumn c in row.Table.Columns)
            {               
                string split = "";
                if (c.DataType == typeof(string))
                    split = "\'";
                else if (c.DataType == typeof(DateTime))
                    if (DbHelper.DBType == DatabaseType.Mdb ||DbHelper .DBType ==DatabaseType.Accdb)
                    {
                        split = "#";
                    }
                    else
                        split = "\'";
                if (expr != "") expr += " and ";
                object v = null;
                if (row.RowState != DataRowState.Detached)
                {
                    v = row[c, DataRowVersion.Original];
                }
                else
                {
                    v = row[c];
                }
                if (v is System.DBNull)
                    expr += prefix + c.ColumnName + suffix + " is null ";
                else
                {
                    if (DbHelper.DBType == DatabaseType.Oracle && c.DataType == typeof(DateTime))
                    {

                        // expr += "to_char('" + c.ColumnName + "','yyyy-mm-dd hh24:mi:ss')='" + ((DateTime)v).ToQcDateString() + "' ";
                        expr += c.ColumnName + "=to_date('" + ((DateTime)v).ToQcDateString() + "','yyyy-mm-dd hh24:mi:ss')";
                        //expr+= "to_date('" + row[c] + "','yyyy-mm-dd hh24:mi:ss')";
                    }
                    else
                    {
                        expr += prefix + c.ColumnName + suffix + "=" + split + v + split + " ";
                    }
                }

            }
            return expr;

        }
        /// <summary>
        /// 生成插入语句
        /// </summary>
        /// <returns></returns>
        private string MakeInsertSql()
        {
            string prefix = "";
            string suffix = "";
            string expr = "";
            if (DbHelper.DBType == DatabaseType.Mdb)
            {
                prefix = "[";
                suffix = "]";
            }
            if (row == null || tablename == null) return "";
            string TableName = tablename;
            StringBuilder updatesql = new StringBuilder(200);
            updatesql.Append("Insert Into ");
            updatesql.Append(TableName);
            updatesql.Append(" (");

            foreach (DataColumn c in row.Table.Columns)
            {

                if (expr != "")
                {
                    updatesql.Append(",");
                    expr += ",";
                }
                updatesql.Append(prefix);
                updatesql.Append(c.ColumnName);
                updatesql.Append(suffix);
                if (row[c] == null || row[c] is System.DBNull)
                    expr += "null";
                else
                {
                    string split = "";
                    if (c.DataType == typeof(string))
                        split = "\'";
                    else if (c.DataType == typeof(DateTime))
                        if (DbHelper.DBType == DatabaseType.Mdb)
                        {
                            split = "#";
                        }
                        else
                            split = "\'";
                    if (DbHelper.DBType == DatabaseType.Oracle && c.DataType == typeof(DateTime))
                    {
                        expr += "to_date('" + ((DateTime)row[c]).ToQcDateString() + "','yyyy-mm-dd hh24:mi:ss')";
                    }
                    else
                    {
                        expr += split;
                        expr += row[c];
                        expr += split;
                    }
                }
            }
            updatesql.Append(") Values (");
            updatesql.Append(expr);
            updatesql.Append(")");
            return updatesql.ToString();
        }
        /// <summary>
        /// 刷新数据行，以获得一致性数据
        /// </summary>
        private void Refresh()
        {
            string expr = GetExpr();
            string sql = "select * from " + tablename + " where " + expr;
            var d = DbHelper.Query(sql);
            if (d != null)
            {
                if (d.Count() > 0)
                {
                    row = d.First();
                }
            }
        }
        /// <summary>
        ///  更新数据到数据库
        /// </summary>
        /// <returns></returns>
        public virtual bool Update(QcDbTransaction trans = null)
        {
            if (tablename == null) return false;
            bool returnvalue = true;
            string Sql = null;
            var cols = row.Table.Columns;
            if (row.RowState == DataRowState.Detached)
            {
                if (cols.Contains("创建人"))
                    if (this["创建人"] != "" && QcUser.User != null) 
                            this["创建人"] = QcUser.User.UserID ;
                if (cols.Contains("创建日期"))
                    this["创建日期"] = QcDate.DateString();
                Sql = MakeInsertSql();
                if (trans != null)
                    returnvalue = trans.Execute(Sql);
                else
                    returnvalue = DbHelper.Execute(Sql);
                this.Refresh();
            }
            else if (row.RowState == DataRowState.Modified)
            {
                if (cols.Contains("修改人"))
                    if( QcUser.User != null)
                        this["修改人"] = QcUser.User.UserID ;
                if (cols.Contains("修改日期"))
                    this["修改日期"] = QcDate.DateString();
                Sql = MakeUpdateSql();
                if (trans != null)
                    returnvalue = trans.Execute(Sql);
                else
                    returnvalue = DbHelper.Execute(Sql);
                if (returnvalue)
                    row.AcceptChanges();
            }
            return returnvalue;
        }
        /// <summary>
        ///  从数据库中删除数据行
        /// </summary>
        /// <param name="trans">事务支持类</param>
        /// <returns></returns>
        public virtual bool DeleteFromDb(QcDbTransaction trans = null)
        {
            if (row.RowState == DataRowState.Detached)//分离行
                return true;
            if (tablename == null) return false;
            bool returnvalue = true;
            string Sql = MakeDeleteSql();
            if (trans != null)
                returnvalue = trans.Execute(Sql);
            else
                returnvalue = DbHelper.Execute(Sql);
            if (returnvalue)
                row.AcceptChanges();
            return returnvalue;

        }
    }
}
