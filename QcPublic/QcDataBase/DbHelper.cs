///////////////////////
// 这是数据库连接类，会使用ado.net 的oledb自动连接默认的数据库获取数据库集以及执行sql语句
//  abao++
//////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Text.RegularExpressions;

namespace QcPublic
{
    public enum RunModeType
    {
        Network,
        Local
    }

    public enum DatabaseType
    {
        Mdb,
        Oracle,
        MsSql,
        MySql,
        None,
        Accdb
    }
    public class DbHelper
    {
       private DbHelper()
        {

        }

       public class OraConnectionStringInfo
       {
           private string m_ip;
           private string m_databasename;
           private string m_username;
           private string m_password;
           public string ip
           {
               get { return m_ip; }
           }
           public string DatabaseName
           {
               get { return m_databasename; }

           }
           public string UserName
           {
               get { return m_username; }
           }
           public string PassWord
           {
               get { return m_password; }
           }
           public OraConnectionStringInfo(string connectionstring)
           {
               //string rule_ip_name = @"Data Source=(?<Source>[\S]);\S+User Id=(?<User>[\S]);\S+Password=(?<Password>[\S]);";
               string rule_ip_name = "Data Source=(?<Source>[\\S]+)[;]+?User Id=(?<User>[\\S]+)[;]+?Password=(?<Password>[\\S]+)[;]+?Pooling";
               Regex reg=new Regex(rule_ip_name,  RegexOptions.IgnoreCase);
               Match result = reg.Match(connectionstring);
               m_ip = result.Groups["Source"].ToString().Split("/".ToArray())[0];
              m_databasename = result.Groups["Source"].ToString().Split("/".ToArray())[1];
               m_username = result.Groups["User"].ToString();
               m_password = result.Groups["Password"].ToString();
           }
       
       }


        private static string DbConnectionString = null;
        public static Exception LastException = null;//最后一次的异常信息
        static DatabaseType m_dbtype = DatabaseType.None;
        public static RunModeType RunMode
        {
            get
            {
                if (m_dbtype == DatabaseType.None || m_dbtype == DatabaseType.Mdb || m_dbtype == DatabaseType.Accdb)
                {
                     return RunModeType.Local;
                }
                else
                {
                    return RunModeType.Network;
                }               
            }
        }
        private static OraConnectionStringInfo m_ConnectionStringInfo = null;
        public static OraConnectionStringInfo ConnectionInfo
        {
            get { return m_ConnectionStringInfo; }
        }

        public static DatabaseType DBType
        {
            get { return m_dbtype; }
        }
        private static DatabaseType TryGetDbType(string connectionstring)
        {
            string tstr=connectionstring.ToLower();
            if (tstr.Contains("oracle")) return DatabaseType.Oracle;
            if (tstr.Contains("ms sql")) return DatabaseType.MsSql;
            if (tstr.Contains("mysql")) return DatabaseType.MySql;
            if (tstr.Contains("mdb")) return DatabaseType.Mdb;
            if (tstr.Contains("accdb")) return DatabaseType.Accdb;
            if (tstr.Contains("ace.oledb")) return DatabaseType.Accdb;
            return DatabaseType.None;
        }
        public static void SetDbServer(string ConnectionString, DatabaseType type = DatabaseType.None)
        {

            m_dbtype = type;
            if (type == DatabaseType.None) m_dbtype = TryGetDbType(ConnectionString);
            if (m_dbtype == DatabaseType.Oracle) m_ConnectionStringInfo =new  OraConnectionStringInfo(ConnectionString);
            DbConnectionString = ConnectionString;
        }
        public static OraConnectionStringInfo TryGetConnectionInfo(string ConnectionString)
        {
            if (TryGetDbType(ConnectionString) == DatabaseType.Oracle)
            {
                OraConnectionStringInfo teminfo = new OraConnectionStringInfo(ConnectionString);
                if( teminfo .ip !=null && teminfo .DatabaseName !=null && teminfo .UserName !=null && teminfo .PassWord !=null)
                {
                    return teminfo ;
                }
            }
             return null;
        }
        public static string MakeOraConnectionstring(string ip, string dbname, string username, string password)
        {
            return string.Format("Provider=OraOLEDB.Oracle.1;Persist Security Info=TRUE;Data Source={0}/{1};User Id={2};Password={3};Pooling=True;Min Pool Size=50;Max Pool Size=1000",
                             ip, dbname, username, password);
        }
        public static string ConnectionString
        {
            get
            {
                return DbConnectionString;
            }
        }
        //static private OleDbConnection P_OleDbConnection=null;
        static OleDbConnection staticOledbConn = null;
        public static OleDbConnection GetConnection()
        {
            if (DBType == DatabaseType.Accdb || DBType == DatabaseType.Mdb)
            {
                if (staticOledbConn != null)
                {
                    // staticOledbConn.ConnectionString = ConnectionString;
                    if (staticOledbConn.ConnectionString != ConnectionString)
                    {
                        staticOledbConn.Close();
                        staticOledbConn.ConnectionString = ConnectionString;
                        staticOledbConn.Open();
                    }
                    return staticOledbConn;
                }
                else
                {
                    staticOledbConn = new OleDbConnection(ConnectionString);
                    return staticOledbConn;
                }
            }
            else if (DBType !=DatabaseType .None )
            {
                if (staticOledbConn != null && staticOledbConn.State == ConnectionState.Closed)
                {
                    staticOledbConn.ConnectionString = ConnectionString;
                    return staticOledbConn;
                }
                else
                {
                    staticOledbConn = new OleDbConnection(ConnectionString);
                    return staticOledbConn;
                }
            }
            return null;
           
        }
        public static EnumerableRowCollection<DataRow> Query(string sql, string tablename = "")
        {
            var dt = GetDataTable(sql, tablename);
            if (dt == null) return null;
            var tb = dt.AsEnumerable();
            dt.Dispose();
            return tb;
        }
        static string accessdb_sql_converter(string olesql)
        {
            string sql;
            sql = olesql.Replace("!=", "<>");//不支持"!=",运算符           
            return sql;
        }
        public static DataSet GetDataset(string sql, string tablename = "")
        {
            switch (DbHelper.DBType)
            {
                case DatabaseType.Accdb:
                   
                 sql=   accessdb_sql_converter(sql );
                    break;
                case DatabaseType.Mdb:
                    sql = accessdb_sql_converter(sql);
                    break;
            }
            OleDbConnection conn = GetConnection();
            //using (OleDbConnection conn = GetConnection())
            //{
                try
                {
                    if(conn.State == ConnectionState.Closed) conn.Open();
                    using (OleDbDataAdapter command = new OleDbDataAdapter(sql, conn))
                    {
                        //command.SelectCommand.Parameters.Clear();
                        DataSet ds = new DataSet();
                        //DataTable db = new DataTable();
                        if (tablename == "") tablename = GetTableNameFromSql(sql);
                        if (tablename == "") tablename = "ds";
                        command.Fill(ds, tablename);
                        return ds;
                    }
                }
                catch (Exception ex)
                {
                    QcLog.LogString("GetDataSet" + ex.Message+sql+ ex.StackTrace);
                    LastException = ex;
                    return null;
                }
                finally
                {
                    if (DBType == DatabaseType.Oracle)
                    {
                        conn.Close();
                        //conn.Dispose();
                        GC.Collect();
                    }
                }
            //}
        }
        public static int GetID(string sql)
        {
            object o = ExecuteScalar(sql);
            if (o != null)
            {
                int i = -1;
                if (int.TryParse(o.ToString(), out i))
                {
                    return i;
                }
            }
            return -1;

        }
        public static DateTime GetDateTime()
        {
            return DateTime.Now;
        }
        public static DataTable GetDataTable(string sql, string tablename = "")
        {
            DataSet ds = GetDataset(sql, tablename);
            if (ds != null) return ds.Tables[0];
            return null;
        }
        /*
        public static bool ExecuteBat(IEnumerable<string> sqls)
        {
            StringBuilder sb = new StringBuilder(sqls.Count() * 100);
            foreach (var v in sqls)
            {
                sb.Append(v);
                sb.Append(";\r\n");
            }
            return Execute(sb.ToString());
        }*/
        public static DataTable ToDataTable(IEnumerable<DataRow> rows)
        {
            if (rows.Count() > 0)
                return rows.CopyToDataTable();
            return new DataTable();
        }
     
        public static bool Execute(string sql )
        {

            switch (DbHelper.DBType)
            {
                case DatabaseType.Accdb:
                    sql = sql.Replace("!=", "<>");
                    break;
                case DatabaseType.Mdb:
                    sql = sql.Replace("!=", "<>");
                    break;
            }

            if (sql=="") return true;
            OleDbConnection conn = GetConnection();
            //using (OleDbConnection conn = GetConnection())
            //{
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    OleDbCommand cmd = new OleDbCommand(sql, conn);
                    int rows = cmd.ExecuteNonQuery();
                    return true;
                }
                catch (OleDbException e)
                {
                    LastException = e;
                    QcLog.LogString(e.Message+sql);
                }
                finally
                {
                    if (DBType == DatabaseType.Oracle)
                    {
                        conn.Close();
                       // conn.Dispose();
                        GC.Collect();
                    }
                }
            //}
            return false;
        }
        public static bool Execute(string sql, OleDbConnection privateconn)
        {
            switch (DbHelper.DBType)
            {
                case DatabaseType.Accdb:
                    sql = sql.Replace("!=", "<>");
                    break;
                case DatabaseType.Mdb:
                    sql = sql.Replace("!=", "<>");
                    break;
            }
            if (sql == "") return true;
            OleDbConnection conn = privateconn;
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                int rows = cmd.ExecuteNonQuery();
                return true;
            }
            catch (OleDbException e)
            {
                LastException = e;
                QcLog.LogString(e.Message + sql);
            }
            finally
            {
                if (DBType == DatabaseType.Oracle)
                {
                    conn.Close();
                  //  conn.Dispose();
                    GC.Collect();
                }
            }
           
            return false;
        }
        //		sql	"Insert Into QC_PRO_PRODUCTTYPE (产品类别编码,产品级别编码,产品类别,产品类别名称,产品规格单位,描述,备注,创建人,创建日期) Values ('20101','201','saf','f','f',null,null,'何鑫星',todate(2013/11/4 16:40:29,'yyyy-mm-dd hh24:mi:ss'))"	string

        private static string GetTableNameFromSql(string sql)
        {
            string tablename = "";
            int start = sql.IndexOf(" from ", StringComparison.OrdinalIgnoreCase);
            if (start > 0)
            {
                int end = sql.IndexOf(' ', start + 6);
                if (end < 0)
                    end = sql.Length;
                return sql.Substring(start + 6, end - start - 6);

            }
            if (tablename == "") return "ds";
            return tablename;
        }
        public static Object ExecuteScalar(string sql)
        {
            OleDbConnection conn = GetConnection();
            //using (OleDbConnection conn = GetConnection())
            //{

                //using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                //{
                    try
                    {
                        if(conn.State == ConnectionState.Closed)conn.Open();
                        OleDbCommand cmd = new OleDbCommand(sql, conn);
                        return cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        LastException = ex;
                        QcLog.LogString(ex.Message+sql);
                        return null;
                    }
                    finally
                    {
                        if (DBType == DatabaseType.Oracle)
                        {
                            conn.Close();
                          //  conn.Dispose();
                            GC.Collect();
                        }
                    }
                //}
            //}
        }
        public static bool Exists(string sql)
        {
            return (ExecuteScalar(sql) != null);

        }
        public static bool Check(string constr)
        {
            try
            {
                string testsql;
                SetDbServer(constr);
                if (DBType == DatabaseType.Oracle)
                {
                    testsql = "select sysdate from dual";
                }
                else
                {
                    testsql = "select * from qc_use_userinfo";
                }
                if (ExecuteScalar(testsql) != null)
                    return true;
            }
            catch
            {

                return false;
            }
            return false;
        }

    }
}
