/////////////////////////
//  日志信息输出类
// abao++
// 2013/12/20 使用多线程安全类改进了多线程下的安全性
////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QcPublic
{
    public class QcLog
    {
        /// <summary>
        ///  挂接日志输出事件
        /// </summary>
        /// <param name="str"></param>
        public delegate void delegateLogEvent(string str);
        public static event delegateLogEvent LogEvent;
        
        /// <summary>
        ///  登记一个日志信息，当存在日志输出处理的函数时，会被输出到日志
        /// </summary>
        /// <param name="stri"></param>
        public static object lockobj = new object();
        
        public static void LogString(string stri)
        {
            lock (lockobj)
            {
                if (LogEvent != null)
                    LogEvent(DateTime.Now.ToString() + stri + "\r\n");
            }
        }
        public static void LogException(string stri,Exception e)
        {
            LogString(stri + "Message:"+e.Message+"\r\n stack:" + e.StackTrace);
        }
    }
}
