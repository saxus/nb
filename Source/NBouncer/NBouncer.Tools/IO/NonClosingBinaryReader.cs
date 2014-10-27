using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NBouncer.Tools.IO
{
    public class NonClosingBinaryReader: BinaryReader
    {
        public NonClosingBinaryReader(Stream input)
            : base(input) 
        {
        }

        public NonClosingBinaryReader(Stream input, Encoding encoding)
            : base(input, encoding)
        {
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(false);
        }
    }
}
