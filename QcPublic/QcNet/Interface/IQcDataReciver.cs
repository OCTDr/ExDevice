using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QcNet
{

    public delegate void QcNetEvent(object sender, QcCmdEventArgs e);
    
    public interface IQcDataReciver
    {
        
       event QcNetEvent ReceiveCmd;
    }
}
