using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QcNet
{
    public  class QcMsgUser:INetUser
    {
        public QcChanel Chanel {  set{chanel=value;} }
        internal IQcSocket Socket { get { if (chanel != null) return chanel.Socket; else return null; } }
        QcChanel chanel;
        public INetUser User = null;
        private string name;
        public string Name { get { return this.name; } }
        //ip
        public string IP { get; set; }
        //端口
        public ushort Port { get; set; }
        public string HostName { get; set; }
        //最后上线时间
        public DateTime LastOnline { get; set; }
        bool _islinked=false;
        public bool IsLinked { get { return (chanel != null || _islinked); } set { _islinked=value ; } }
          public QcMsgUser(INetUser user)
        {
            this.IP = user.IP;
           this.Port=user.Port;
           this.HostName = user.HostName;
            this.name = user.Name;
            this.User = user;
        }
        public bool Send(string cmd)
          {

              if (chanel != null)
              return chanel.Send(cmd);
              return false;
          }
        public void Close()
        {
            if (chanel != null)
            {
                chanel.Close();
                chanel = null;
            }
        }
        ~QcMsgUser()
        {
            Close();
        }

    }
}
