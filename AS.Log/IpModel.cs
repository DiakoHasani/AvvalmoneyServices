using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AS.Log
{
    public class IpModel
    {
        public string HostName { get; set; } = "";
        public IPAddress[] HostAddresses { get; set; }
        public string Ipv6 { get; set; }
    }
}
