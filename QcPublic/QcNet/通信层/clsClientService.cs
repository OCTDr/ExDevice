////////////////////////////////
// 客户端网络服务连接，对每个单独客户端构建一个连接
//  abao 2008-12-10
// 使用TCPCSFrameWork封装,做为数据容器包含客户端连接信息
// abao 2009-11-13 
///////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace QcNet
{
    public class QcClientService:IQcSocket
    {
       
        private Socket ClientSocket;                              //单个客户端连接的Socket对象
        private QcServer server = null;
        public string id="";
        public string ID {get {return id;}}
        
/// <summary>
///  在一个网络连接上构建一个网络读取和写入流
/// </summary>
/// <param name="socket"></param>
        public QcClientService(Socket  socket,QcServer server)
        {                     
            ClientSocket = socket;
            this.server = server;
            id = socket.LocalEndPoint.ToString();
        }
/// <summary>
///  向网络接口发送数据
/// </summary>
/// <param name="text"></param>
/// <returns></returns>
        public  bool Send(string text)
        {
            try
            {
               this.server.SendData(this.ClientSocket,text);
                return true;
            }
            catch
            {

                return false;
            }
        }
        public bool CloseLink()
        {
            return true;
        }
/// <summary>
///  关闭连接
/// </summary>
        public bool Close()
        {
            try
            {
              
                return true;
            }
            catch
            {
            }
            return false;
        }
         ~QcClientService()
        {
            Close();
        }
    }
}
