using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using QcNet;
namespace QcPublic
{
    public class QcMsgClient: QcClient
    {
        public event EventHandler<QcMessagerUpdateMsgEventArg> ReciveUpdateMsg = null;
        public event EventHandler<QcMessagerDataUpdateEventArg> DataUpdate = null;
        
        protected IEnumerable<INetUser> Users = null;
        protected QcUser User = null;
        public QcMsgClient(IEnumerable<INetUser> users,QcUser user)
            : base()
        {
            Users = users;
            User = user;
        }
        public ConcurrentDictionary<string, QcMsgUser> UserChanel
        {
            get
            {
                return lstUser;
            }
        }
        protected ConcurrentDictionary<string, QcMsgUser> lstUser = new ConcurrentDictionary<string, QcMsgUser>();

        bool Conntect(string ip,int port)
        {
            base.Connect(ip, port);
            base.ConnectedServer += QcMsgClient_ConnectedServer;
            return true;
        }

        void QcMsgClient_ConnectedServer(object sender, EventArgs e)
        {
            this.Send(QcCmd.MakeCmd(QcProtocol.QcCommand.QcUserLogin, User.Name, "*"));
        }
        void QcMessagner_ReciveCmd(object sender, QcMessagerCmdEventArg e)
        {

            switch (e.cmd.CmdType)
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
                   // QcLog.LogString(e.cmd.ToString());
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
    }
}
