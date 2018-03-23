using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QcNet
{
    public interface IQcSocket
    {
        bool Send(string cmd);
        bool CloseLink();
    }
}
