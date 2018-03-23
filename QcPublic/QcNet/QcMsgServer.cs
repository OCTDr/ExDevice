using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using QcNet;
using QcPublic;
namespace QcPublic
{
    public class QcMessagerCmdEventArg : EventArgs
    {
        public QcMsgUser user;
        public QcCmd cmd;
    }
    public class QcMsgServer : QcServer
    {

        public QcLicense License = null;
        protected QcChanel serverchanel = null;
        public INetUser User = null;
        public event EventHandler<QcMessagerLoginEventArg> Logined = null;
        public event EventHandler<QcMessagerLoginEventArg> Loginout = null;
        protected IEnumerable<INetUser> Users = null;
        System.Threading.Timer m_Timer = null;
        public IEnumerable<QcUser> OnlineUsers
        {
            get
            {
                var onlineusers = from u in lstUser.Values
                                  where u.IsLinked
                                  select u.User as QcUser;
                return onlineusers;
            }
        }
        public QcMsgServer(IEnumerable<INetUser> users, ushort port = 1800)
            : base(port)
        {
            Users = users;
        }
        ~QcMsgServer()
        {
            try
            {
                this.Close();
            }
            catch
            {

            }
        }
        public ConcurrentDictionary<string, QcMsgUser> UserChanel
        {
            get
            {
                return lstUser;
            }
        }
        protected ConcurrentDictionary<string, QcMsgUser> lstUser = new ConcurrentDictionary<string, QcMsgUser>();
        void TimerCallBack(object o)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    foreach (var v in lstUser.Values.Where(t => t.IsLinked))
                    {
                        TimeSpan t = DateTime.Now - v.LastOnline;
                        //超过3分钟以上，就关闭
                        if (t.TotalSeconds > 100)
                        {

                            v.Close();
                            if (this.Loginout != null)
                            {
                                var evt = new QcMessagerLoginEventArg(v);
                                this.Loginout(this, evt);
                            }
                            this.BroadcastMsg(QcCmd.MakeCmd(QcProtocol.QcCommand.QcLoginOut, "*", v.Name));
                        }
                    }
                });
        }
        public void SetLicense(QcLicense lic)
        {
            this.License = lic;
        }
        public virtual void Start()
        {
            if (m_Timer == null)
                m_Timer = new System.Threading.Timer(TimerCallBack);

            foreach (var v in Users)
            {
                var qmu = new QcMsgUser(v);
                lstUser.TryAdd(qmu.Name, qmu);
            }
            base.Start();
            m_Timer.Change(10000, 60000);
            base.ReceiveCmd += new QcNetEvent(server_ReceiveCmd);
            base.ClientClosed += new EventHandler<System.Net.Sockets.SocketAsyncEventArgs>(server_ClientClosed);
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
        object obj = new object();
        void server_ReceiveCmd(object sender, QcCmdEventArgs e)
        {
            lock (obj)
            {
                QcCmd cmd = e.Cmd;
                string from = e.Cmd.tokens(1);
                string to = e.Cmd.tokens(2);
                if (lstUser.ContainsKey(from))
                {
                    lstUser[from].LastOnline = DateTime.Now;
                }
                switch (cmd.CmdType)
                {
                    case QcProtocol.QcCommand.QcCheckLicense:
                        try
                        {

                            LicenseRetCode ret = LicenseRetCode.未授权;
                            //CONTINUE:
                            //    if (cmd == null) goto CONTINUE;
                            string type = cmd.tokens(1);
                            string arg = cmd.tokens(2);
                            switch (type)
                            {
                                case "Feature":
                                    int id = 0;
                                    int.TryParse(arg, out id);
                                    ret = License.CheckFeature(id);
                                    break;

                                case "Module":
                                    ret = License.CheckModule(arg);
                                    break;
                                case "Product":
                                    ret = License.CheckProduct(arg);
                                    break;
                                case "Regsiter":
                                    ret = License.RegsisterUser(arg);
                                    break;
                                case "Release":
                                    License.ReleaseUser(arg);
                                    break;
                            }
                            e.Chanel.Send(QcCmd.MakeCmd(QcProtocol.QcCommand.QcCheckLicense, ret.ToString()));
                            e.Chanel.CloseLink();
                        }
                        catch (Exception ex)
                        {
                            QcLog.LogString("QcCheckLicense   " + cmd.ToString() + ":" + ex.Message);
                        }

                        break;
                    case QcProtocol.QcCommand.QcUserLogin:
                        bool blLogined = false;
                        blLogined = true;
                        if (lstUser.ContainsKey(from) == false)
                        {
                            var user = QcUser.RefreshUser(from);
                            if (user != null)
                            {
                                var qmu = new QcMsgUser(user);
                                lstUser.TryAdd(qmu.Name, qmu);
                            }
                        }

                        if (lstUser.ContainsKey(from))
                        {
                            //这里判断一下是否可以登录
                            var msguser = lstUser[from];
                            var user = msguser.User as QcUser;

                            string sql = "select * from " + QcUser.TableName + " where " + user.CodeField + "='" + user.Code + "' and 状态='启用'";
                            if (DbHelper.Exists(sql))
                            {
                                QcChanel chanel = new QcChanel();
                                chanel.SetChanel(e.Chanel, this);
                                lstUser[from].Chanel = chanel;
                                if (this.Logined != null)
                                {
                                    var userfrom = lstUser[from];
                                    var evtarg = new QcMessagerLoginEventArg(userfrom);
                                    this.Logined(this, evtarg);
                                }
                                //this.BroadcastMsg(e.Cmd.ToString());
                                this.BroadcastMsg(QcCmd.MakeCmd(QcProtocol.QcCommand.QcUserLogin  , from, "*"));
                                blLogined = true;
                            }
                            else
                                blLogined = false;
                            QcClientService qcs = e.Chanel as QcClientService;
                            e.Chanel.Send(QcCmd.MakeCmd(QcProtocol.QcCommand.QcLoginReplay, blLogined));
                        }
                        break;
                    case QcProtocol.QcCommand.QcListUser:
                        var strusers = "";
                        foreach (var u in OnlineUsers) strusers += u.Name + ",";
                        e.Chanel.Send(QcCmd.MakeCmd(QcProtocol.QcCommand.QcListUser, strusers));
                        break;
                    case QcProtocol.QcCommand.QcLoginOut:
                        if (lstUser.ContainsKey(from))
                        {
                            var userfrom = lstUser[from];
                            if (this.Loginout != null)
                            {
                                var evt = new QcMessagerLoginEventArg(userfrom);
                                this.Loginout(this, evt);
                            }
                            this.BroadcastMsg(QcCmd.MakeCmd(QcProtocol.QcCommand.QcLoginOut, from, "*"));
                            userfrom.Close();
                          
                        }
                        break;
                    case QcProtocol.QcCommand.QcDataUpdate:
                        this.BroadcastMsg(e.Cmd.ToString());
                        break;
                    default:
                        if (to == "*")
                        {
                            this.BroadcastMsg(e.Cmd.ToString());
                            return;
                        }
                        else
                        {
                            if (lstUser.ContainsKey(to))
                            {
                                var user = lstUser[to];
                                user.Send(e.Cmd.ToString());
                            }
                        }
                        break;
                }
            }
        }
        public void Close()
        {
            BroadcastShutdown();
            foreach (var v in lstUser)
            {
                v.Value.Close();
            }
            base.ShutDown();
            m_Timer.Dispose();
        }


        public void BroadcastMsg(string cmd)
        {

            //Parallel.ForEach(lstUser, v =>

            foreach (var v in lstUser.Values.Where(t => t.IsLinked))
            {
                bool ret = v.Send(cmd);
            }
            //);
        }
        public void BroadcastShutdown()
        {
            BroadcastMsg(QcCmd.MakeCmd(QcProtocol.QcCommand.QcLoginOut, User.Name));
        }
    }
}
