/////
// 信使服务的封装
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using QcNet;
using System.Threading.Tasks;
namespace QcPublic
{
  
    public class QcMessagnerCS:QcMessagnerBase
    {     
        public event EventHandler<QcMessagerUpdateMsgEventArg> ReciveUpdateMsg = null;
        public event EventHandler<QcMessagerDataUpdateEventArg> DataUpdate=null;
        protected new event EventHandler<QcMessagerCmdEventArg> ReciveCmd = null;
        public new event EventHandler<QcMessagerLoginEventArg> Logined = null;
        public new event EventHandler<QcMessagerLoginEventArg> Loginout = null;
        public new event EventHandler<QcMessagerMsgEventArg> ReciveMsg = null;
        public event EventHandler<QcSystemMsgEventArg> ReciveSystemMsg = null;
        ushort port = 0;
        string ip = "";
        System.Threading.Timer m_Timer;
        QcMsgClient client = null;
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
        public QcMessagnerCS():base(null)
        {

        }
        public bool SendSystemMsg(string ID,string DestUser, string Msg)
        {
            QcProtocol.QcCommand cmd = QcProtocol.QcCommand.QcSystemMsg;
             //发送消息到用户
            /*QcMsgUser user =null;
            if(lstUser.TryGetValue(DestUser,out user))
            {
                if (user.Send(QcCmd.MakeCmd(cmd, User.Name, DestUser,Msg, DateTime.Now.ToString(), ID)))
                {
                    return true;
                }
            }
             return false
            //存储消息到缓存组
             * */
            //直接发送给服务器，服务器负责转发
            if (client == null) return false;
            return client.Send(QcCmd.MakeCmd(cmd, QcUser.User.Name, DestUser, Msg, DateTime.Now.ToString(), ID));
        }
        public bool SendMsg(string Name, string Msg, QcProtocol.QcCommand cmd = QcProtocol.QcCommand.QcMsg)
        {
            /*
            //发送消息到用户
            QcMsgUser user =null;
            if(lstUser.TryGetValue(Name,out user))
            {
                if(user.Send(QcCmd.MakeCmd(cmd,User.Name,Name,Msg,DateTime.Now.ToString() )))
                {
                    return true;
                }
            }
              return false;
            //存储消息到缓存组
             * */
            if (client == null) return false ;
            return client.Send(QcCmd.MakeCmd(cmd, QcUser.User.Name, Name, Msg, DateTime.Now.ToString()));
            
        }
        public QcMsgUser AddUser(INetUser user)
        {
            var qmu = new QcMsgUser(user);
            lstUser.TryAdd(qmu.Name, qmu);
            return qmu;
        }
        //System.Timers.Timer m_Timer;   

       private void ConnectServer()
        {
            try
            {
                if (client == null)
                {
                    client = new QcMsgClient(QcUser.Users, QcUser.User);
                    client.ConnectedServer += (o, e) =>
                    {
                        client.Send(QcCmd.MakeCmd(QcProtocol.QcCommand.QcUserLogin, User.Name, "*"));
                        //client.Send(QcCmd.MakeCmd(QcProtocol.QcCommand.QcLoginOut, "*", User.Name));
                    };
                    client.DataUpdate += (o, e) => { if (this.DataUpdate != null)this.DataUpdate(o, e); };
                    client.DisConnectedServer += (o, e) =>
                    {
                        client.Send(QcCmd.MakeCmd(QcProtocol.QcCommand.QcLoginOut, User.Name, "*"));
                    };
                    client.ReceiveCmd += QcMessagner_ReciveCmd;                  
                }
                client.Connect(ip, port);
            }
            catch (Exception ex)
            {
                QcLog.LogException("MessagnerCS", ex);
            }
        }
        
        void m_Timer_Elapsed(object sender)
        {            
          if(client==null)
          {
              ConnectServer();
          }
          else
          {
              if (QcUser.User != null)
              {
                  client.Send(QcCmd.MakeCmd(QcProtocol.QcCommand.QcKeepAlive, QcUser.User.Name));
              }
          }
        }
        public  virtual  new void Start(ushort port,string ip)
        {
            base.Users = QcUser.Users;
            foreach (var v in base.Users)
            {
                var qmu = new QcMsgUser(v);
                lstUser.TryAdd(qmu.Name, qmu);
            }
            this.port = port;
            this.ip = ip;
            this.User = QcUser.User;
            object state=new object();
            m_Timer = new System.Threading.Timer(m_Timer_Elapsed, state, 100000, 100000);
            ConnectServer();          
            Task.Factory.StartNew(() =>
                {
                              
                    QcNode.NodeUpdateToDb += (o, e) =>
                            {
                                //发现数据节点更新，就广播节点编码和表名
                                QcNode node = o as QcNode;
                                if (node != null)
                                    client.Send(
                                         QcCmd.MakeCmd(QcProtocol.QcCommand.QcDataUpdate
                                         , node.Code
                                         , node.TableName
                                         , e.ChangeType
                                         , e.Node.CodeField
                                         , (e.ChangeType == NodeChangeType.Delete) ? e.Node.ToString() : ""
                                         ));
                            };

                });
        }
       
      
    
        void QcMessagner_ReciveCmd(object sender,QcCmdEventArgs e)
        {
            try
            {
                if (e.Cmd.CmdType== QcProtocol .QcCommand .Undefine)
                    return;
                QcMsgUser userfrom = null;
                if (lstUser.ContainsKey(e.Cmd.From))
                    userfrom = lstUser[e.Cmd.From];
                if (this.ReciveCmd != null)
                {
                    var cmdevt = new QcMessagerCmdEventArg()
                    {
                        cmd = e.Cmd,
                        user = userfrom
                    };
                    this.ReciveCmd(this, cmdevt);
                }

                switch (e.Cmd.CmdType)
                {
                    case QcProtocol.QcCommand.QcLoginReplay:
                        client.Send(QcCmd.MakeCmd(QcProtocol.QcCommand.QcListUser));
                        break;
                    case QcProtocol.QcCommand.QcLoginOut:
                        if (userfrom != null)
                        {
                            userfrom.Chanel = null;
                            userfrom.IsLinked = false;
                        }
                        if (this.Loginout != null)
                            if(userfrom!=null)
                               this.Loginout(this, new QcMessagerLoginEventArg(userfrom));

                        break;
                    case QcProtocol.QcCommand.QcMsg:
                        if (this.ReciveMsg != null)
                        {
                            var msgevt=new QcMessagerMsgEventArg(userfrom.Name,e.Cmd.tokens(3),e.Cmd.tokens(4));
                            this.ReciveMsg(this, msgevt);
                        }
                        break;
                    case QcProtocol.QcCommand.QcSystemMsg:
                        if (this.ReciveSystemMsg != null)
                        {
                            var systemevt = new QcSystemMsgEventArg(e.Cmd.tokens(5),e.Cmd.From, e.Cmd.tokens(3), e.Cmd.tokens(4));
                            this.ReciveSystemMsg(this, systemevt);
                        }
                        break;
                    case QcProtocol.QcCommand.QcUserLogin:
                        foreach (QcMsgUser u in lstUser.Values)
                        {
                            if (u.Name == userfrom.Name)
                            {
                                u.IsLinked = true;
                                break;
                            }
                        }

                            
                        if (this.Logined != null)
                            this.Logined(this, new QcMessagerLoginEventArg(userfrom));
                        break;
                    case QcProtocol.QcCommand.QcListUser:
                        var v = e.Cmd.tokens(1).Split(',');
                        foreach (var u in v)
                        {
                            if (lstUser.ContainsKey(u))
                            {
                                lstUser[u].Chanel = new QcChanel();
                            }
                        }
                        break;
                    case QcProtocol.QcCommand.QcUpdateMsg:
                        if (ReciveUpdateMsg != null)
                        {
                            var msg = QcMsg.GetMsg(e.Cmd.tokens(4));
                            ReciveUpdateMsg(this, new QcMessagerUpdateMsgEventArg(msg));
                        }
                        break;
                    case QcProtocol.QcCommand.QcDataUpdate:
                        string index = e.Cmd.tokens(2) + "|" + e.Cmd.tokens(1);

                        //收到数据更新消息，进行更新
                     //   QcLog.LogString(e.Cmd.ToString());
                        NodeChangeType type = NodeChangeType.Create;
                        if (Enum.TryParse<NodeChangeType>(e.Cmd.tokens(3), out type))
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
                                    string tablename = e.Cmd.tokens(2);
                                    string code = e.Cmd.tokens(1);
                                    string codefield = e.Cmd.tokens(4);
                                    QcNode node = new QcNewNode(tablename, code, codefield);
                                    node.InitFromString(e.Cmd.tokens(5));
                                    DataUpdate(this, new QcMessagerDataUpdateEventArg(node, type));
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                QcLog.LogException("QcMessagner:" , ex);
            }
           
        }
        public void DisConection()
        {

            client.Discnect();

        }
        public void Stop()
        {
            client.Send(QcNet.QcCmd.MakeCmd(QcNet.QcProtocol.QcCommand.QcLoginOut, User.Name));
            client.Close();
              
            //循环遍历发送登出消息
        }
        ~QcMessagnerCS()
        {
            try
            {
                Stop();
                m_Timer.Dispose();
            }
            catch
            {

            }           
        }
    }
}
