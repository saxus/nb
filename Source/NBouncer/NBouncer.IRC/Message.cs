using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBouncer.IRC
{
    public class Message
    {
        public string Origin { get; set; }
        public string MessageType { get; set; }
        public string Target { get; set; }
        public string Flags { get; set; }
        public string Message { get; set; }

        public string OriginalMessage { get; set; }

        public Client Client { get; set; }

    }
}
