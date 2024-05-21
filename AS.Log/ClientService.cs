using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AS.Log
{
    public class Client: IClient
    {
        public IpModel GetIp()
        {
            return new IpModel
            {
                HostAddresses = Dns.GetHostAddresses(Dns.GetHostName()),
                HostName = Dns.GetHostName(),
                Ipv6 = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString()
            };
        }
    }
    public interface IClient
    {
        IpModel GetIp();
    }
}
