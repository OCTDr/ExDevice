using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QcNet
{
    public interface INetUser
    {
        string Name { get; }
        string IP { get; }
        ushort Port { get; }
        string HostName { get; }
    }
}
