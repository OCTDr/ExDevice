/////
// 信使服务的封装
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using QcNet;
namespace QcPublic
{
   
    public class QcMessagnerP2P:QcMessagnerBase
    {
     
     
        public event EventHandler<QcMessagerUpdateMsgEventArg> ReciveUpdateMsg = null;
        public event EventHandler<QcMessagerDataUpdateEventArg> DataUpdate=null;
        ushort port = 0;
       new  public  ICollection<QcMsgUser> Users
        {
            get
            {
                return lstUser.Values;
            }
        }      
        public IEnumerable<QcUser> OnlineUsers
        {
            get
            {
                var onlineusers= from u in lstUser.Values 
                                 where u.IsLinked 
                                 select u.User as QcUser ;
                return onlineusers;
            }
        }
        public QcMessagnerP2P():base(null)
        {

        }
        public bool SendMsg(string Name, string Msg, QcProtocol.QcCommand cmd = QcProtocol.QcCommand.QcMsg)
        {
            //发送消息到用户
            QcMsgUser user =null;
            if(lstUser.TryGetValue(Name,out user))
            {
                if(user.Send(QcCmd.MakeCmd(cmd,User.Name,Name,Msg )))
                {
                    return true;
                }
            }
            //存储消息到缓存组
            return false;
        }

      //  System.Threading.Timer keepalivetimer = null;
        public override void Start(ushort port,string ip="")
        {
            this.port = port;
            SaveIpToDb();
            base.Users = QcUser.Users;
            this.User = QcUser.User;
            base.Start(port);
            this.ReciveCmd += QcMessagner_ReciveCmd;
            QcNode.NodeUpdateToDb += (o, e) =>
                    {
                        //发现数据节点更新，就广播节点编码和表名
                        QcNode node = o as QcNode;
                        if (node != null)
                            this.BroadcastMsg(
                                QcCmd.MakeCmd(QcProtocol.QcCommand.QcDataUpdate
                                , node.Code
                                , node.TableName
                                , e.ChangeType
                                , e.Node.CodeField
                                ,(e.ChangeType==NodeChangeType.Delete)?  e.Node.ToString():""
                                ));
                    };

        }

      
        private void SaveIpToDb()
        {
            var hostname = System.Net.Dns.GetHostName();
            string ips = "";
            foreach (var v in System.Net.Dns.GetHostAddresses(hostname))
            {
                if (ips != "") ips += ",";
                ips += v.ToString();
            }
            var port = this.port;
            QcUser.User["ip"] = ips;
            QcUser.User["port"] = port.ToString();
            QcUser.User["计算机名"] = hostname;
            QcUser.User.Update();
        }
        void QcMessagner_ReciveCmd(object sender, QcMessagerCmdEventArg e)
        {
           
            switch(e.cmd.CmdType)
            {
                    case QcProtocol.QcCommand.QcUpdateMsg:
                        if (ReciveUpdateMsg != null)
                        {
                            var msg = QcMsg.GetMsg(e.cmd.tokens(4));
                            ReciveUpdateMsg(this, new QcMessagerUpdateMsgEventArg(msg));
                        }
                        break;
                    case QcProtocol.QcCommand.QcDataUpdate:
                        string index = e.cmd.tokens(2) + "|" + e.cmd.tokens(1);

                        //收到数据更新消息，进行更新
                      //  QcLog.LogString(e.cmd.ToString());
                        NodeChangeType type = NodeChangeType.Create;
                        if (Enum.TryParse<NodeChangeType>(e.cmd.tokens(3), out type))
                        {
                            switch (type)
                            {
                                case NodeChangeType.Create:

                                    break;
                                case NodeChangeType.Update:
                                    if (QcNode.lstNode.ContainsKey(index))
                                    {
                                        QcNode.lstNode[index].Refresh();
                                    }
                                    break;
                                case NodeChangeType.Delete:
                                    if (QcNode.lstNode.ContainsKey(index))
                                    {
                                        QcNode.lstNode[index].RiseDelete();
                                    }
                                    break;
                            }
                            if (DataUpdate != null)
                            {
                                if (QcNode.lstNode.ContainsKey(index))
                                {
                                    DataUpdate(this, new QcMessagerDataUpdateEventArg(QcNode.lstNode[index], type));
                                }
                                else
                                {
                                    string tablename = e.cmd.tokens(2);
                                    string code = e.cmd.tokens(1);
                                    string codefield = e.cmd.tokens(4);
                                    QcNode node = new QcNewNode(tablename, code, codefield);
                                    node.InitFromString(e.cmd.tokens(5));
                                    DataUpdate(this, new QcMessagerDataUpdateEventArg(node, type));
                                }
                            }
                        }
                        break;
                }
           
        }
        public void Stop()
        {
            base.Close();
            //循环遍历发送登出消息
        }
        ~QcMessagnerP2P()
        {
            try
            {
                Stop();
            }
            catch
            {

            }           
        }
    }
}
