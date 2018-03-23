using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace QcPublic
{
    public class QcCheckData:QcNode 
    {
        public static string TableName = "QC_PRO_CHECKDATA";
        
        public QcCheckData(DataRow row=null)
            : base(row,"QC_PRO_CHECKDATA")
        {
        }
        public override  string Name { get { return this["数据名称"]; } set { this["数据名称"]=value; } }
        /// <summary>
        ///  编码
        /// </summary>
        public override  string Code { get { return this["数据ID"]; } set { this["数据ID"] = value; } }
        public override   string CodeField { get { return "数据ID"; } }

        /// <summary>
        ///  检查已有数据的合法性
        /// </summary>
        /// <returns></returns>
        public override  QcCheckResult Check(string strField = null)
        {
            return null;
        }
        public static IEnumerable<QcCheckData> GetCheckData(QcJob job)
        {
            var rows = DbHelper.Query("select * from " + QcCheckData.TableName + " where 作业编号='" + job.Code + "'");
            return rows.Select(t => new QcCheckData(t));
        }
        /// <summary>
        /// 清除所有的检查数据，请谨慎使用该方法
        /// </summary>
        /// <returns></returns>
        public static void ClearnAllCheckData()
        {
            DbHelper.Execute("delete from "+QcCheckData .TableName);
        }
        /// <summary>
        /// 清除当前作业的检查数据，请谨慎使用该方法
        /// </summary>
        /// <returns></returns>
        public static void ClearnAllCheckData(QcJob job)
        {
            DbHelper.Execute("delete from "+QcCheckData .TableName + " where 作业编号='" + job.Code + "'");
        }
        public object Tag { get; set; }
    }
}
