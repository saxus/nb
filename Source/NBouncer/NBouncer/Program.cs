using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBouncer.IRC;
using System.Net;
using NBouncer.Tools.Net;

namespace NBouncer
{
    class Program
    {
        static void Main(string[] args)
        {
            var cl = new Client("freenode", DomainTools.GetIPEndPointFromHostName("irc.freenode.org", 6667));
            cl.Connect();
            cl.Send("NICK saxus|test");
            cl.Send("USER saxus 0 * :n/a");

            while (true)
            {
                var s = Console.ReadLine();

                if (s == "quit")
                    break;

                cl.Send(s);
            }
        }
    }
}
