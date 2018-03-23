using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QcNet
{
    public class QcProtocol
    {
        public enum QcCommand
        {
            QcUserLogin=1,//登录消息,用户身份的识别与确认
            QcLoginReplay=2,//登录确认消息
            QcLoginOut=3,//离线消息
            
            QcListUser=4,//获取服务器中可用的用户列表    
            QcRequesLogin=5,//请对方登录本机服务器
            QcServerShutDown=6,//服务器关闭


            QcGetList=50,//获取目录信息
            QcGetFile=51,//获取文件
            /// <summary>
            ///  QcMsg和QcUpdateMsg会被持久化到数据库，如果发送到目标用户，则设置标记已发
            /// </summary>
            QcMsg=60,//文本消息      
            /// <summary>
            /// 说明 文本消息为空则为更新类消息，不会被存入数据库
            /// 数据格式：
            ///  表名|数据ID|文本消息
            /// </summary>
            QcUpdateMsg=61,//发送项目、任务、作业、方案变更信息 
            QcDataUpdate=62,//项目、任务、作业、方案变更时通知相应的客户端 
            QcSystemMsg=63,//程序内部定义的消息

            QcCheckLicense=70,//用于查询授权情况
            QcKeepAlive=96,
            QcAck=98,//遇到未定义回复的消息将会发送ACK消息，另外可以作为心跳包
            Undefine=99//未知消息
        }
       
    }
}
