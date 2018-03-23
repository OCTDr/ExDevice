using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QcNet;
namespace QcPublic
{
    static class QcNodeMsg
    {
        public static bool SendMessage(this QcNode node, string name, string msg)
        {
            var u = QcUser.Users.Where(t => t.Name == name || t.UserID == name).FirstOrDefault();
            if (u == null) return false;
            var qcmsg = new QcMsg(u.Name, msg, node.TableName, node.Code);
            if(qcmsg.Send())
                return true;
            return false;
        }
    }
   public  class QcMsg:DynamicDataRowObject
    {
       
        public QcMsg(string name,string msg,string datatable=null,string dataid=null):base(null,"QC_USE_MSG")
        {
            if (QcUser.User != null)
            {
                this["消息编号"] = Guid.NewGuid().ToString();
                this["发送者"] = QcUser.User.Name;
                this["消息内容"] = msg;
                this["接收者"] = name;
                this["发送时间"] = DateTime.Now.ToString();
                this["数据表名"] = datatable;
                this["数据编号"] = dataid;
                this["标记"] = "0";
            }
        }
        public QcMsg(DataRow row):base(row)
        {

        }
        public bool Send()
        {
            if (QcUser.User == null) return false;
            if (this.Update() == false) return false ;
            if(QcMessagner.Messagner.SendMsg(this["接收者"],this.ToCmdStr(),QcProtocol.QcCommand.QcUpdateMsg))
            {
                this["标记"] = "1";
                this.Update();
            }
            return true;
        }
        public static IEnumerable<QcMsg> GetUserMsg(string name)
        {
            List<QcMsg> lstmsg = new List<QcMsg>();
            string sql = "select * from QC_USE_MSG where 接收者='" + name + "' and 标记<>'1'";
            var msgs = DbHelper.Query(sql);
            if (msgs != null)
            {
                foreach (var v in msgs)
                {
                    lstmsg.Add(new QcMsg(v));
                }
            }
            return lstmsg;
        }

        public static QcMsg GetMsg(string msg)
        {
            var t = msg.Split('|');            
            string sql = "select * from QC_USE_MSG where 消息编号='" + t[0] + "'";
            var rows = DbHelper.Query(sql);
            if(rows!=null)
            {
                var v=rows.FirstOrDefault();
                if (v != null)
                    return new QcMsg(v);
            }
            return null;
        }
        public string ToCmdStr()
        {
            return this["消息编号"] + "|" + this["消息内容"] + "|" + this["数据表名"]
                + "|" + this["数据编号"] + "|" + this["发送时间"];
        }
    }
}
