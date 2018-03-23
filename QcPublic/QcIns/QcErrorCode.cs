using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace QcPublic
{
    public class QcErrorCode : QcNode
    {
        public static string TableName = "QC_INS_ERORECODE";
        
        public QcErrorCode(DataRow row = null)
            : base(row, QcErrorCode.TableName)
        {
        }
        public override string Name { get { return this["错误描述"]; } set { this["错误描述"] = value; } }
        /// <summary>
        ///  编码
        /// </summary>
        public override string Code { get { return this[CodeField]; } set { this[CodeField] = value; } }
        public override string CodeField { get { return "错误ID"; } }

        /// <summary>
        ///  检查已有数据的合法性
        /// </summary>
        /// <returns></returns>
        public override QcCheckResult Check(string strField = null)
        {
            return null;
        }
        public static IEnumerable<QcCheckData> GetCheckData(QcJob job)
        {
            var rows = DbHelper.Query("select * from " + QcCheckData.TableName + " where 作业编号='" + job.Code + "'");
            return rows.Select(t => new QcCheckData(t));
        }
    }
}
