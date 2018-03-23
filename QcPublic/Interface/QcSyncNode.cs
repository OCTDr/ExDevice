using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace QcPublic
{
    class QcSyncNode:QcNode 
    {

        public string m_code;
        public override string Code
        {
            get
            {
                return m_code;
            }
            set
            {
                m_code = value;
            }
        }
        string m_codefield = "";
        public override string CodeField
        {
            get { return this.m_codefield; }
        }
        public QcSyncNode(DataRow row,string tablename,string code,string codefield):base(row,tablename,code)
        {
            this.m_codefield = codefield;
        }
    }
}
