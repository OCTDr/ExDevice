////////////////////////////////////
//   节点接口
//  abao++
///////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace QcPublic
{
    public enum NodeChangeType
    {
        Update,
        Create,
        Delete
    }
    public class QcNodeEventArg:EventArgs
    {
        public QcNodeEventArg(QcNode node,NodeChangeType type)
        {
            this.node=node;
            this.type = type;
        }
        QcNode node;
        NodeChangeType  type=NodeChangeType.Create;
        public QcNode Node{get{return node;}}
        public NodeChangeType ChangeType{ get{return type;}}
    }
 
    public abstract class QcNode:DynamicDataRowObject,IQcNode
    {
        /// <summary>
        ///  存储所有Qcnode创建的对象
        /// </summary>
        public static Dictionary<string, QcNode> lstNode = new Dictionary<string, QcNode>();
        public static event EventHandler<QcNodeEventArg> NodeUpdateToDb = null;
        /// <summary>
        ///  节点名称
        /// </summary>
        private string m_Name = string.Empty;
        public Action<string> UpdateName = null;
        public  virtual  string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
                if (UpdateName != null)
                {
                    UpdateName(m_Name);
                }
            }
        }
        /// <summary>
        ///  编码
        /// </summary>
        public abstract  string Code{get;set;}
        public abstract string CodeField { get;}
        public string TableName { get { return tablename; } }
       
        /// <summary>
        ///  检查已有数据的合法性
        /// </summary>
        /// <returns></returns>
        public virtual QcCheckResult Check(string strField = null)
        {
            return null;
        }
        public QcNode(DataRow row, string tablename = null,string code=""):base(row,tablename)
        {
            if (code == "") code = this.Code;
            string key = this.tablename + "|"+this.Code;
            if (lstNode.ContainsKey(key))
            {
                lstNode[key] = this;
            }
            else
                lstNode.Add(key, this);
        }
        protected override string GetExpr()
        {
           string[] MultCodeFiled =CodeField .Split(';').ToArray();
           
            if(MultCodeFiled .Count()>1)
            {
               
                string[] multcode =Code.Split(';').ToArray();
                 string str=MultCodeFiled[0]+"='" + multcode[0]+"'";
                for(int i=1;i<MultCodeFiled .Count();i++)
                {
                    str +=  " and " + MultCodeFiled[i] + "='" + multcode[i] + "'";
                }
                return str;
            }
            else
            {
                return CodeField + "='" + Code+"'";
            }
            
            
           //return base.GetExpr();
        }
        public override bool DeleteFromDb(QcDbTransaction trans = null)
        {
            bool isnew = this.IsNew();
            if(isnew) return true;
            bool ret = base.DeleteFromDb();
            if (ret)
            {
                if (NodeUpdateToDb != null)
                    NodeUpdateToDb(this, new QcNodeEventArg(this, NodeChangeType.Delete ));
            }
            return ret;
        }
        /// <summary>
        ///  
        /// </summary>        
        public override bool Update(QcDbTransaction trans = null)
        {
            bool isnew=this.IsNew();
 	         bool ret=base.Update(trans);
            if(ret)
            {
                if(NodeUpdateToDb!=null)
                    NodeUpdateToDb(this, new QcNodeEventArg(this, isnew ? NodeChangeType.Create : NodeChangeType.Update));
            }
            return ret;
        }
        public event EventHandler evtDelete = null;
        public void RiseDelete()
        {
            if (evtDelete != null)
                evtDelete(this, null);
        }

        public event EventHandler evtRefresh = null;
        protected void RiseRefresh()
        {
            if (evtRefresh != null) evtRefresh(this, null);            
        }
        protected virtual string GetRefreshSql()
        {
            return "select * from " + tablename + " where " + CodeField + "='" + Code + "'";
        }
        public virtual void  Refresh()
        {
            string sql = GetRefreshSql();
            var d = DbHelper.Query(sql);
            if (d != null)
            {
                if (d.Count() > 0)
                {                    
                    row = d.First();
                    this.RiseRefresh();
                }
            }
        }

        public bool InitFromString(string str)
        {
            var ary = str.Split('\f');
            foreach (var v in ary)
            {
                var fieldary=v.Split('\b');
                if (fieldary.Length > 1)
                {
                    var field = fieldary[0];
                    var value = fieldary[1];
                    this[field] = value;
                    
                }
            }
            return true;
        }
        
        public override string ToString()
        {

            if (row != null)
            {
                string sql = "";
                foreach (DataColumn  v in row.Table.Columns)
                {
                    if (sql != "") sql += "\f";
                    sql += v.ColumnName + "\b";
                    sql += row[v.ColumnName].ToString();
                }
                return sql;
            }
            return "";
        }
        public string 创建人
        {
            get
            {
                return QcUser.GetName(this["创建人"]);
            }
        }
        public string 修改人
        {
            get
            {
                return QcUser.GetName(this["修改人"]);
            }
        }
               
    }
}
