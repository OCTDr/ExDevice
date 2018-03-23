using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace QcPublic
{
    public class QcNewNode:QcNode 
    {
        string codefield = null;
        string code = "";
        string GetMutiSql(string table,string code,string field)
        {
            var codes = code.Split(';');
            var fields = field.Split(';');
            int length = Math.Min(codes.Length, fields.Length);
            string expr = "";
            for(int i=0;i<length;i++)
            {
                if (expr != "") expr += " and ";
                expr += fields[i] + "='" + codes[i] + "'";
            }
            return "select * from " + table + " where " + expr;
        }
        public QcNewNode(string tablename,string code,string codefield)
            : base(null,tablename,code)
        {
            string sql = "";
            if(code.IndexOf(';')>0)
            {
                sql = GetMutiSql(tablename, code, codefield);
            }
            else
                sql="select * from " + tablename + " where " + codefield + "='" + code + "'";
            var dt=DbHelper.GetDataTable(sql);
            var rows = dt.Rows;
            if (rows != null)
            {
                     if(rows.Count>0)
                    this.row = rows[0];
                    if (this.row == null)
                    {
                        this.row = dt.NewRow();
                    }
            }
            this.codefield = codefield;
            this.code = code;
        }
        public override string Name { get { return ""; } set { } }
        /// <summary>
        ///  编码
        /// </summary>
        public override  string Code { get { return code; } set { this[codefield] = value; } }
        public override   string CodeField { get { return codefield; } }

        /// <summary>
        ///  检查已有数据的合法性
        /// </summary>
        /// <returns></returns>
        public override  QcCheckResult Check(string strField = null)
        {
            return null;
        }
    }
}
