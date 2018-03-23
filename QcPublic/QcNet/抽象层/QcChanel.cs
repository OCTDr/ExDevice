using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QcNet
{
    public class QcCmdEventArgs:EventArgs 
    {
        QcCmd cmd=null;
        IQcSocket socket = null;
        public QcCmd Cmd { get { return cmd; } }
        public IQcSocket  Chanel { get { return socket; } }
        
        public QcCmdEventArgs(QcCmd cmd,IQcSocket  socket)
        {
            this.cmd = cmd;
            this.socket = socket;
        }

    }
    enum ChanelType
    {
        ServerSocket,
        ClientSocket
    }
   public class QcChanel
    {
       internal IQcSocket Socket { get { return socket; } }
        IQcSocket socket=null;
        IQcDataReciver reciver = null;
        public string from;
        public string to;
       
        public event EventHandler<QcCmdEventArgs> ReceivedCmd = null;
        public QcChanel()
        {

        }
        public virtual bool SetChanel(IQcSocket socket,IQcDataReciver reciver)
        {
            if (this.socket != null) this.socket.CloseLink();
            this.socket = socket;
            this.reciver = reciver;
            if(reciver!=null)
            {
                reciver.ReceiveCmd += new QcNetEvent(reciver_ReceiveCmd);
            }
            return true;
        }

        void reciver_ReceiveCmd(object sender, QcCmdEventArgs e)
        {

            if (this.ReceivedCmd != null)
            {                
                this.ReceivedCmd(this,e);
            }
        }
       public bool Close()
        {
            if (socket != null)
                return socket.CloseLink();
            else return true;
        }
       public virtual bool Send(string cmd)
       {
           if(socket!=null)
            return socket.Send(cmd);
           return false;
       }
    }
}
