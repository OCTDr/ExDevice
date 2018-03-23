//// 文件传输的指令类
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QcNet
{
    /// <summary>
    ///  文件传输通信的基本协议
    /// </summary>
    public class QcCmd
    {
        public QcProtocol.QcCommand CmdType;//消息数组
        string[] m_tokens; //存储消息内容的数组
        public string From{get {return this.tokens(1);}}
        public string To {get {return this.tokens(2);}}
        public override string ToString()
        {
            string o="";
            foreach(var v in m_tokens)
            {
                if (o != "") o += "\a";
                o += v;
            }
            o += "\a\0";
            return o;
        }
        public QcCmd(string str)
        {
            if (str == null)
            {
                CmdType = QcProtocol.QcCommand.Undefine;
            }
            else
            {
                m_tokens = str.Split(new Char[] { '\a' });
                try
                {
                    CmdType = (QcProtocol.QcCommand)Convert.ToInt32(m_tokens[0]);
                }
                catch 
                {
                    CmdType = QcProtocol.QcCommand.Undefine;
                }
            }
        }
        public int TokensCount
        {
            get { return m_tokens.Length; }
        }
        /// <summary>
        ///  获取消息包含的参数
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>参数内容文本</returns>
        public string tokens(int index)
        {
            if (index > 0 && index < m_tokens.Length)
            {
                return m_tokens[index];

            }
            else return "";
        }
        /// <summary>
        ///  生成一条消息
        /// </summary>
        /// <param name="msgtype">消息类别</param>
        /// <param name="args">消息参数</param>
        /// <returns>构建的消息字符串</returns>
        public static string MakeCmd(QcProtocol.QcCommand msgtype, params object[] args)
        {
            string strReturn = "";
            foreach (object o in args)
            {
                if (o != null)
                {
                    strReturn += "\a" + o.ToString();
                }
                else
                {
                    strReturn += "\a" + "";
                }
            }
            return msgtype.ToString("D") + strReturn+"\a\0";
        }
    }
}
