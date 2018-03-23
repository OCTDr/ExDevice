////////////////////////////////
// �ͻ�������������ӣ���ÿ�������ͻ��˹���һ������
//  abao 2008-12-10
// ʹ��TCPCSFrameWork��װ,��Ϊ�������������ͻ���������Ϣ
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
       
        private Socket ClientSocket;                              //�����ͻ������ӵ�Socket����
        private QcServer server = null;
        public string id="";
        public string ID {get {return id;}}
        
/// <summary>
///  ��һ�����������Ϲ���һ�������ȡ��д����
/// </summary>
/// <param name="socket"></param>
        public QcClientService(Socket  socket,QcServer server)
        {                     
            ClientSocket = socket;
            this.server = server;
            id = socket.LocalEndPoint.ToString();
        }
/// <summary>
///  ������ӿڷ�������
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
///  �ر�����
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
