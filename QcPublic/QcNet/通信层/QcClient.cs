//////////////////////////////////////////
//   文件传输的客户端，目前的问题，获取文件为异步操作，
//
/////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using System.Net;
using ZYSocket.Server;

namespace QcNet
{

    public class QcClient : QcSocketClient
        ,IQcSocket,IQcDataReciver 
    {
        static Coder coder = new QcCoder();
        public delegate void delegateLogEvent(string str);//数据到达事件接口
        /// <summary>
        ///  系统错误或者调试信息输出到日志，需要调用程序挂接事件进行最后处理
        /// </summary>
        public event delegateLogEvent LogEvent;
        public event QcNetEvent ReceiveCmd;
        public event EventHandler<EventArgs> ConnectedServer;
        public event EventHandler<EventArgs> DisConnectedServer;
        //private string SaveFileName="";
        private string m_ServerIP;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stri"></param>
        public  void LogString(string stri)
        {
            if (LogEvent != null)
                LogEvent(stri);
        }
        public QcClient()
            :base()
        {               
            //base.Resovlver = new DatagramResolver("\0");
            base.Connection += new ConnectionOk(QcClient_Connection);
            base.DataOn += new QcNet.DataOn(QcClient_DataOn);
            base.Disconnection += new ExceptionDisconnection(QcClient_Disconnection);
        }

        void QcClient_Disconnection(string message)
        {
            if(this.DisConnectedServer!=null)
                this.DisConnectedServer(this,new EventArgs());   
            
        }

        void QcClient_DataOn(byte[] Data)
        {

            
          //  QcPublic.QcLog.LogString(coder.GetEncodingString(Data, Data.Length));
            if(this.ReceiveCmd!=null)
                this.ReceiveCmd(this,new QcCmdEventArgs(new QcCmd(coder.GetEncodingString(Data,Data.Length)),this));
        }

        void QcClient_Connection(string message, bool IsConn)
        {
            if (IsConn)
            {
                if (this.ConnectedServer != null)
                    this.ConnectedServer(this, new EventArgs());
            }
        }
        public bool CloseLink()
        {
            return Discnect();
        }
        public bool Discnect()
        {
            base.Discnect();
               return true;
        }
        public bool Close()
        {
            base.Close();
            return true;
        }
        public bool Connect(string ip, int port)
        {
            m_ServerIP = ip;
            return base.ConnectionTo(ip, port);
        }   
        public bool Send(string cmd)
        {
            
            base.SendTo(coder.GetEncodingBytes(cmd));
            return true;
        }
       
    }
}
