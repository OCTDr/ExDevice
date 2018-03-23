// 文件传输服务器
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.Xml.Linq;
using ZYSocket.share;
using ZYSocket.Server;
using QcPublic;
namespace QcNet
{


   
   public class QcServer:ZYSocket.Server.ZYSocketSuper
       ,IQcDataReciver
    {

       static QcCoder coder = new QcCoder();

       public event QcNetEvent ReceiveCmd;
       public event EventHandler<SocketAsyncEventArgs> ClientConnected;
       public event EventHandler<SocketAsyncEventArgs> ClientClosed;
       
        public QcServer(ushort port)
           // :base(port)
            :base("any",port,50000,1496)
        {
            
        }
       
        protected new   bool Start()
        {

            base.BinaryInput = new BinaryInputHandler(BinaryInputHandler); //设置输入代理
            base.Connetions = new ConnectionFilter(ConnectionFilter); //设置连接代理
            base.MessageInput = new MessageInputHandler(MessageInputHandler); //设置 客户端断开
            base.Start();            
            return true;
        }
        // <summary>
        /// 用户断开代理（你可以根据socketAsync 读取到断开的
        /// </summary>
        /// <param name="message">断开消息</param>
        /// <param name="socketAsync">断开的SOCKET</param>
        /// <param name="erorr">错误的ID</param>
         void MessageInputHandler(string message, SocketAsyncEventArgs socketAsync, int erorr)
        {
            try
            {
                if (this.ClientClosed != null)
                {
                    this.ClientClosed(this, socketAsync);
                }
                socketAsync.UserToken = null;
                socketAsync.AcceptSocket.Close();
            }
            catch (Exception e)
            {
                QcPublic.QcLog.LogException("ServerMessageInput:", e);
            }
        }
        /// <summary>
        /// 用户连接的代理
        /// </summary>
        /// <param name="socketAsync">连接的SOCKET</param>
        /// <returns>如果返回FALSE 则断开连接,这里注意下 可以用来封IP</returns>
        bool ConnectionFilter(SocketAsyncEventArgs socketAsync)
        {
            try
            {
                socketAsync.UserToken = socketAsync.AcceptSocket.RemoteEndPoint.ToString();
                if (this.ClientConnected != null)
                {
                    this.ClientConnected(this, socketAsync);
                }
                return true;
            }
            catch (Exception e)
            {
                QcPublic.QcLog.LogException("QcServer", e);
                return false;
            }

        }

        public void SendData(Socket socket, string str)
        {
            byte[] data = coder.GetEncodingBytes(str);
            base.SendData(socket, data);
        }

        /// <summary>
        /// 数据包输入
        /// </summary>
        /// <param name="data">输入数据</param>
        /// <param name="socketAsync">该数据包的通讯SOCKET</param>
       void BinaryInputHandler(byte[] data, SocketAsyncEventArgs socketAsync)
        {
            try
            {

                if (this.ReceiveCmd != null)
                {
                  //  QcLog.LogString(coder.GetEncodingString(data, data.Length));
                    this.ReceiveCmd(this, new QcCmdEventArgs(
                        new QcCmd(coder.GetEncodingString(data, data.Length))
                        , new QcClientService(socketAsync.AcceptSocket, this)));
                }
            }
            catch (Exception ex)
            {
                QcPublic.QcLog.LogString(ex.Message);
            }

        }

        void QcServer_MessageOut(object sender, ZYSocket.Server.LogOutEventArgs e)
        {
           // QcLog.LogString(e.Mess);   
        }       
        //服务器关闭
        public virtual void ShutDown()
        {            
            base.Stop();


        }   
    }
}
