using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace NBouncer.Tools.Net
{
    public enum IPSelectionMethod : int
    {
        AllowOnlyOne = 0,
        First = 1,
        Random = 2,
    }

    public class DomainTools
    {
        public static IPEndPoint GetIPEndPointFromHostName(string hostName, int port)
        {
            return GetIPEndPointFromHostName(hostName, port, IPSelectionMethod.Random);
        }

        private static Random s_random;

        public static IPEndPoint GetIPEndPointFromHostName(string hostName, int port, IPSelectionMethod selectionMethod)
        {
            if (string.IsNullOrWhiteSpace(hostName)) throw new ArgumentException("Hostname cannot be empty or null!", "hostName");

            var addresses = System.Net.Dns.GetHostAddresses(hostName);
            if (addresses.Length == 0)
            {
                throw new ArgumentException(
                    "Unable to retrieve address from specified host name.",
                    "hostName"
                );
            }
            else
            {
                switch (selectionMethod)
                {
                    case IPSelectionMethod.AllowOnlyOne:

                        if (addresses.Length > 1)
                        {
                            throw new ArgumentException(
                                "There is more that one IP address to the specified host.",
                                "hostName"
                            );

                        }
                        return new IPEndPoint(addresses[0], port); // Port gets validated here.

                    case IPSelectionMethod.First:
                        return new IPEndPoint(addresses[0], port);


                    case IPSelectionMethod.Random:
                        if (s_random == null)
                            s_random = new Random();

                        var addr = s_random.Next(0, addresses.Length);
                        return new IPEndPoint(addresses[addr], port);

                    default: 
                        throw new NotImplementedException();
                }
            }
        }
    }
}
