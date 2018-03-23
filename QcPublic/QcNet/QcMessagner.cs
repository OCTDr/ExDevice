using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QcNet;
namespace QcPublic
{
    #region event定义
    public class QcMessagerLoginEventArg : EventArgs
    {
        public QcMsgUser User { get { return user; } }
        QcMsgUser user;
        public QcMessagerLoginEventArg(QcMsgUser user)
        {
            this.user = user;
        }
    }
    public class QcSystemMsgEventArg : EventArgs
    {
        public string Msg { get { return msg; } }
        public string FromUser { get { return from; } }
        public string Date { get { return date; } }
        public string ID { get { return id; } }//消息标志符
        string msg;
        string from;
        string date;
        string id;
        public QcSystemMsgEventArg(string id,string from, string msg, string date)
        {
            this.id = id;
            this.msg = msg;
            this.from = from;
            this.date = date;
        }
    }

    public class QcMessagerMsgEventArg : EventArgs
    {
        public string Msg { get { return msg; } }
        public string FromUser { get { return from; } }
        public string Date { get { return date; } }
        string msg;
        string from;
        string date;
        public QcMessagerMsgEventArg(string from, string msg, string date)
        {
            this.msg = msg;
            this.from = from;
            this.date = date;
        }
    }
    public class QcMessagerDataUpdateEventArg : EventArgs
    {
        public QcNode Node { get { return node; } }
        QcNode node;
        NodeChangeType type = NodeChangeType.Create;
        public NodeChangeType ChangeType { get { return type; } }
        public QcMessagerDataUpdateEventArg(QcNode node, NodeChangeType type)
        {
            this.node = node;
            this.type = type;
        }
    }
    public class QcMessagerUpdateMsgEventArg : EventArgs
    {
        public QcMsg Msg { get { return msg; } }
        QcMsg msg;
        public QcMessagerUpdateMsgEventArg(QcMsg msg)
        {
            this.msg = msg;
        }
    }
    #endregion

    public class QcMessagner:QcMessagnerCS 
    {
       public  static string IP;
       public static ushort Port;
        public static QcMessagner Messagner = new QcMessagner();
        public override void Start(ushort port, string ip)
        {
            IP = ip;
            Port = port;
            base.Start(port, ip);
        }      
    }
}
