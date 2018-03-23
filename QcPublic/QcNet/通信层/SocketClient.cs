/*
 * 北风之神SOCKET框架(ZYSocket)
 *  Borey Socket Frame(ZYSocket)
 *  by luyikk@126.com
 *  Updated 2010-12-26 
 */
using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
namespace QcNet  
{

    public delegate void ConnectionOk(string message,bool IsConn);
    public delegate void DataOn(byte[] Data);
    public delegate void ExceptionDisconnection(string message);

    /// <summary>
    /// ZYSOCKET 客户端
    /// （一个简单的异步SOCKET客户端，性能不错。支持.NET 3.0以上版本。适用于silverlight)
    /// </summary>
    public class QcSocketClient
    {
        /// <summary>
        /// SOCKET对象
        /// </summary>
        private Socket sock;

        /// <summary>
        /// 连接成功事件
        /// </summary>
        protected event ConnectionOk Connection;

        /// <summary>
        /// 数据包进入事件
        /// </summary>
        protected event DataOn DataOn;
        /// <summary>
        /// 出错或断开触发事件
        /// </summary>
        protected event ExceptionDisconnection Disconnection;

        private System.Threading.AutoResetEvent wait = new System.Threading.AutoResetEvent(false);
        
        public QcSocketClient()
        {           
            
            
        }

        private bool IsConn;

        /// <summary>
        /// 异步连接到指定的服务器
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public void BeginConnectionTo(string host, int port)
        {
            IPEndPoint myEnd = null;

            #region ipformat
            try
            {
                myEnd = new IPEndPoint(IPAddress.Parse(host), port);
            }
            catch (FormatException)
            {
                IPHostEntry p = Dns.GetHostEntry(Dns.GetHostName());

                foreach (IPAddress s in p.AddressList)
                {
                    if (!s.IsIPv6LinkLocal)
                        myEnd = new IPEndPoint(s, port);
                }
            }

            #endregion
            if (sock != null)
            {
                Close();
            }
            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.RemoteEndPoint = myEnd;
            e.Completed += new EventHandler<SocketAsyncEventArgs>(e_Completed);
            sock = new Socket(myEnd.AddressFamily, SocketType.Stream, ProtocolType.Tcp);  
            
            if (!sock.ConnectAsync(e))
            {
                eCompleted(e);
            }
        }

        public bool ConnectionTo(string host, int port)
        {
            IPEndPoint myEnd = null;

            #region ipformat
            try
            {
                myEnd = new IPEndPoint(IPAddress.Parse(host), port);
            }
            catch (FormatException)
            {
                IPHostEntry p = Dns.GetHostEntry(Dns.GetHostName());

                foreach (IPAddress s in p.AddressList)
                {
                    if (!s.IsIPv6LinkLocal)
                        myEnd = new IPEndPoint(s, port);
                }
            }

            #endregion
            if (sock != null)
            {
                Close();
            }
            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.RemoteEndPoint = myEnd;
            e.Completed += new EventHandler<SocketAsyncEventArgs>(e_Completed);
            sock = new Socket(myEnd.AddressFamily, SocketType.Stream, ProtocolType.Tcp);              
            if (!sock.ConnectAsync(e))
            {
                eCompleted(e);
            }

            wait.WaitOne(100);
            wait.Reset();

            return IsConn;
        }



        void e_Completed(object sender, SocketAsyncEventArgs e)
        {
            eCompleted(e);
        }


        void eCompleted(SocketAsyncEventArgs e)
        {
            try
            {
                switch (e.LastOperation)
                {
                    case SocketAsyncOperation.Connect:

                        if (e.SocketError == SocketError.Success)
                        {

                            IsConn = true;
                            wait.Set();

                            if (Connection != null)
                                Connection("连接成功", true);

                            byte[] data = new byte[4098];
                            e.SetBuffer(data, 0, data.Length);  //设置数据包

                            if (!sock.ReceiveAsync(e)) //开始读取数据包
                                eCompleted(e);

                        }
                        else
                        {
                            IsConn = false;
                            wait.Set();
                            if (Connection != null)
                                Connection("连接失败", false);
                        }
                        break;

                    case SocketAsyncOperation.Receive:
                        try
                        {
                            if (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
                            {
                                byte[] data = new byte[e.BytesTransferred];
                                Buffer.BlockCopy(e.Buffer, 0, data, 0, data.Length);

                                byte[] dataLast = new byte[4098];
                                e.SetBuffer(dataLast, 0, dataLast.Length);

                                if (!sock.ReceiveAsync(e))
                                    eCompleted(e);

                                if (DataOn != null)
                                    DataOn(data);

                            }
                            else
                            {
                                if (Disconnection != null)
                                    Disconnection("与服务器断开连接");
                            }
                            break;
                        }
                        catch
                        {
                            break;
                        }

                }
            }
            catch
            { }
        }


     
        /// <summary>
        /// 发送数据包
        /// </summary>
        /// <param name="data"></param>
        public virtual void SendTo(byte[] data)
        {
            try
            {
                SocketAsyncEventArgs e = new SocketAsyncEventArgs();
                e.SetBuffer(data, 0, data.Length);
                sock.SendAsync(e);
            }
            catch
            {
            }
        }

        public virtual void BeginSend(byte[] data)
        {
            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.SetBuffer(data, 0, data.Length);
            sock.SendAsync(e);
        }
        public void Discnect()
        {
            if (Disconnection != null)
                Disconnection("与服务器断开连接");
        }
        public void Close()
        {
            try
            {
                sock.Shutdown(SocketShutdown.Both);
                sock.Disconnect(false);
                sock.Close();
              
            }
            catch (ObjectDisposedException)
            {
            }
            catch (NullReferenceException)
            {

            }
            catch (Exception)
            {
            }
        }
    }
}
