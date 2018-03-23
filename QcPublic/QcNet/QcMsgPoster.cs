using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using QcNet;
namespace QcPublic
{
    public class QcMsgPoster
    {
        static QcMsgClient client=null;
        public static void SetServer(string ip, ushort port)
        {
            QcMessagner.IP = ip;
            QcMessagner.Port = port;
        }

        public static bool PostSystemMsg(string ID, string DestUser, string Msg)
        {
            QcProtocol.QcCommand cmd = QcProtocol.QcCommand.QcSystemMsg;
            return PostMessage(QcCmd.MakeCmd(cmd, "*", DestUser, Msg, DateTime.Now.ToString(), ID));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code">数据编码值</param>
        /// <param name="TableName">表明</param>
        /// <param name="CodeField">数据编码字段名</param>
        /// <param name="ChangeType">变更类型</param>
        /// <returns></returns>
       public static bool PostMeassage( string Code,string TableName,string CodeField, NodeChangeType ChangeType=NodeChangeType.Update)
        {
            return PostMessage(QcNet.QcCmd.MakeCmd(QcProtocol.QcCommand.QcDataUpdate
                                          , Code
                                          , TableName
                                          , ChangeType
                                          , CodeField));
       }
        public static bool PostMessage(string cmd)
        {
            if (client == null)
            {
                client = new QcMsgClient(null, null);
                client.ConnectedServer += (o, e) =>
                {
                    client.Send(cmd);
                };
                client.Connect(QcMessagner.IP, QcMessagner.Port);
            }
            else
            {
                try
                {
                    client.Send(cmd);
                }
                catch (Exception e)
                {
                    QcLog.LogString("PostMessagee" + e.Message);
                    client = null;
                    PostMessage(cmd);
                }
            }
            return true;
        }
    }
}
