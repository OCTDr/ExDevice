/////////////////////////
//  数据库事务处理类，为动态row类更新和删除，都可以传入事务，进行处理
// abao++
////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
namespace QcPublic
{
    /// <summary>
    ///  数据库事物处理支持
    /// </summary>
   public class QcDbTransaction:IDisposable
    {
        OleDbTransaction m_transaction;
        OleDbConnection Connection = null;
       /// <summary>
       /// 从一个数据库连接创建事务
       /// </summary>
       /// <param name="conn"></param>
        public QcDbTransaction(OleDbConnection conn )
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            m_transaction = conn.BeginTransaction();
            Connection = conn; 
                
        }
        public void Dispose()
        {
            m_transaction.Dispose();            
        }
       /// <summary>
       /// 使用事务执行一个sql语句
       /// </summary>
       /// <param name="sql"></param>
       /// <returns></returns>
        public  bool Execute(string sql)
        {
            if (sql == "") return true;
                try
                {
                    if(Connection.State!=System.Data.ConnectionState.Open)
                    Connection.Open();
                    OleDbCommand cmd = new OleDbCommand(sql, Connection, m_transaction);
                  
                    int rows = cmd.ExecuteNonQuery();
                    return true;
                }
                catch (OleDbException e)
                {
                    QcLog.LogString(e.Message+sql);
                }          
            return false;
        }
       /// <summary>
       /// 对事物进行回滚操作
       /// </summary>
       /// <returns></returns>
        public bool RollBack()
        {
            m_transaction.Rollback();
            return true;
        }
       /// <summary>
       ///  对事物进行提交操作
       /// </summary>
       /// <returns></returns>
        public bool Commit()
        {
            m_transaction.Commit();
            return true;
        }

    }
}
