using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Threading;
using QcPublic;
namespace QcNet
{


 
    public class QcMessagnerBase
    {
        protected class QcMessagerCmdEventArg : EventArgs
        {
            public QcMsgUser user;
            public QcCmd cmd;
        }
        protected QcServer server = null;
        protected QcChanel serverchanel = null;
        public INetUser User = null;
        public event EventHandler<QcMessagerLoginEventArg> Logined = null;
        public event EventHandler<QcMessagerLoginEventArg> Loginout = null;
        public event EventHandler<QcMessagerMsgEventArg> ReciveMsg = null;
        protected event EventHandler<QcMessagerCmdEventArg> ReciveCmd = null;
        protected IEnumerable<INetUser> Users = null;
        
        public QcMessagnerBase(IEnumerable<INetUser> users)
        {
            Users = users;
        }
        public ConcurrentDictionary<string,QcMsgUser> UserChanel

        {
            get
            {
                return lstUser;
            }

        }
        protected ConcurrentDictionary<string, QcMsgUser> lstUser = new ConcurrentDictionary<string, QcMsgUser>();
        public virtual void Start(ushort port,string ip="")
        {
            foreach (var v in Users)
            {
                var qmu = new QcMsgUser(v);

                lstUser.TryAdd(qmu.Name, qmu);
            }
            server = new QcServer(port);
            server.Start();
            server.ReceiveCmd += new QcNetEvent(server_ReceiveCmd);
            server.ClientClosed += new EventHandler<System.Net.Sockets.SocketAsyncEventArgs>(server_ClientClosed);
       
            serverchanel = new QcChanel();
            serverchanel.SetChanel(null, server);
            serverchanel.ReceivedCmd += serverchanel_ReceivedCmd;
            BroadcastLogin();
        }

        void server_ClientClosed(object sender, System.Net.Sockets.SocketAsyncEventArgs e)
        {
            if (this.Loginout != null)
            {
                foreach (var v in lstUser)
                {
                    if (v.Value.Socket is QcClientService)
                    {
                        var socket = v.Value.Socket;
                        if (socket != null)
                        {
                            var qcs = socket as QcClientService;
                            if (qcs.ID == e.AcceptSocket.ToString())
                            {
                                var evt = new QcMessagerLoginEventArg(v.Value);
                                v.Value.Chanel = null;
                                this.Loginout(this, evt);
                            }
                        }
                    }
                }
            }
        }

        void server_ReceiveCmd(object sender, QcCmdEventArgs e)
        {
            QcCmd cmd = e.Cmd;
            string from = e.Cmd.tokens(1);
            switch (cmd.CmdType)
            {
                case QcProtocol.QcCommand.QcUserLogin:

                    bool blLogined = false;
                    if (cmd.tokens(2) == User.Name)
                    {
                        blLogined = true;
                        QcChanel chanel = new QcChanel();
                        
                        chanel.SetChanel(e.Chanel, server);

                        if (lstUser.ContainsKey(from))
                        {
                            lstUser[from].Chanel = chanel;
                            if (this.Logined != null)
                            {
                                var userfrom = lstUser[from];
                                var evtarg = new QcMessagerLoginEventArg(userfrom);
                                this.Logined(this, evtarg);
                            }
                        }

                    }

                    QcClientService qcs = e.Chanel as QcClientService;
                    e.Chanel.Send( QcCmd.MakeCmd(QcProtocol.QcCommand.QcLoginReplay, blLogined));
                    if (blLogined && from != User.Name)
                        e.Chanel.Send(QcCmd.MakeCmd(QcProtocol.QcCommand.QcUserLogin, User.Name, from));

                    break;
            }
        }
        public void Close()
        {
            if (User != null)
            {
                BroadcastMsg(QcCmd.MakeCmd(QcProtocol.QcCommand.QcLoginOut, User.Name));

                foreach (var v in lstUser)
                {
                    v.Value.Close();
                }
            }
            if(server!=null)
            server.ShutDown();
        }
      

        void serverchanel_ReceivedCmd(object sender, QcCmdEventArgs e)
        {
            // 这是登录服务器，首次登陆，没有创建连接对象 
            if (lstUser.ContainsKey(e.Cmd.tokens(1)) == false) return;
            var userfrom = lstUser[e.Cmd.tokens(1)];
            if (userfrom == null) return;
            switch(e.Cmd.CmdType)
            {
                case QcProtocol.QcCommand.QcMsg:
                    if(this.ReciveMsg!=null)
                    {
                        var msgevtarg = new QcMessagerMsgEventArg(e.Cmd.tokens(1), e.Cmd.tokens(3),e.Cmd.tokens(4));
                        this.ReciveMsg(this, msgevtarg);
                    }
                    break;
                case QcProtocol.QcCommand.QcLoginReplay:
                    break;
                case QcProtocol.QcCommand.QcLoginOut:
                    
                    if (this.Loginout != null)
                    {
                        var evt = new QcMessagerLoginEventArg(userfrom);                        
                        this.Loginout(this, evt);
                    }
                    userfrom.Close();
                    break;
                case QcProtocol.QcCommand.QcUserLogin:
                    if (sender == serverchanel) return;
                    if (userfrom.Name == User.Name) return;
                    bool blLogined = false;
                    if (e.Cmd.tokens(2) == User.Name)
                    {
                        blLogined = true;
                        if (this.Logined != null)
                        {
                            var evtarg = new QcMessagerLoginEventArg(userfrom);
                            this.Logined(this, evtarg);
                        }
                    }
                    e.Chanel.Send(QcCmd.MakeCmd(QcProtocol.QcCommand.QcLoginReplay, blLogined));                   
                    break;
            }
            if(this.ReciveCmd!=null)
            {
                var cmdevt=new QcMessagerCmdEventArg(){
                    cmd=e.Cmd,user=userfrom                  
                };
                this.ReciveCmd(this,cmdevt);
            }
        }


         void BroadcastLogin()
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {                
                foreach (var v in lstUser)
                {
                    var qmu = v.Value;
                    var qc = TryConnectUser(qmu);
                    if (qc != null)
                    {
                        var c = new QcChanel();
                        qmu.Chanel = c;
                        c.SetChanel(qc,qc);
                        c.ReceivedCmd += serverchanel_ReceivedCmd;
                        qc.DisConnectedServer +=(o,e)=>{
                            if (this.Loginout != null)
                            {
                                var evtarg = new QcMessagerLoginEventArg(qmu);
                                qmu.Chanel = null;
                                this.Loginout(this, evtarg);
                            }
                            
                        };
                    }
                }
            });
        }

    
        public  void BroadcastMsg(string cmd)
        {
          
            //Parallel.ForEach(lstUser, v =>
        
            foreach(var v in lstUser.Values.Where(t=>t.IsLinked))
            {
                bool ret = v.Send(cmd);
               
            }
            //);
        }
        public  void BroadcastLoginOut()
        {
            BroadcastMsg(QcCmd.MakeCmd(QcProtocol.QcCommand.QcLoginOut, User.Name));
            
        }   
         QcClient TryConnectUser(INetUser user)
        {
            ushort port = user.Port;
           
                string name = user.Name;
                string pcname = user.HostName;
                try
                {
                    var ips = user.IP.Split(new[] { ',' });
                    foreach (var i in ips)
                    {
                        var r = TryConnectIp(i, port, name);
                        if (r != null) return r;
                    }
                    /*
                     // 这是个费时的操作
                    var ipfromhost = System.Net.Dns.GetHostAddresses(pcname);                    
                    foreach (var i in ipfromhost)
                    {
                        
                    }*/
                   
                }
                catch(Exception ex)
                {
                    //string a=ex.ToString;
                    return null;
                }

            return null;
        }
        QcClient TryConnectIp(string ip, ushort port, string name)
        {
            if (ip == "") return null;
            Ping myPing;

            myPing = new Ping();
            AutoResetEvent are = new AutoResetEvent(false);
            bool pingsuccess = false;
            myPing.PingCompleted += (o, e) => { are.Set(); pingsuccess = (e.Reply.Status == IPStatus.Success); };
            string pingIP = ip.ToString();
            try
            {
                myPing.SendAsync(pingIP, 100, null);
            }
            catch
            {
                return null;
            }
            are.WaitOne(100);//等待ping完成
            if (pingsuccess == true)
              //  if (myPing.Send(pingIP, 10000).Status == IPStatus.Success)
                {
                    QcClient client = new QcClient();
                    AutoResetEvent msgresetevt = new AutoResetEvent(false);
                    client.ConnectedServer += (o, e) => { msgresetevt.Set(); };
                    client.Connect(ip, port);
                    if (msgresetevt.WaitOne(1000))
                    {
                        bool logined = false;
                        client.ReceiveCmd += (o, e) =>
                        {
                            QcCmd cmd = e.Cmd;
                            switch (cmd.CmdType)
                            {
                                case QcProtocol.QcCommand.QcLoginReplay:
                                    if (cmd.tokens(1) == "True")
                                    {
                                        logined = true;
                                    };
                                    break;
                                default:
                                    break;
                            }
                            msgresetevt.Set();
                        };
                        client.Send(QcCmd.MakeCmd(QcProtocol.QcCommand.QcUserLogin, User.Name, name));
                        if (msgresetevt.WaitOne(100))
                        {
                            if (logined)
                                return client;
                        }
                        client.Close();
                    }

                }
          
            //失败返回null
            return null;
        }
    }
}
