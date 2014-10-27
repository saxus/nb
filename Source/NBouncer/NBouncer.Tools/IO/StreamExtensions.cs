using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NBouncer.Tools.IO
{
    public static class StreamExtensions
    {
        public static void WriteArray(this Stream stream, byte[] array)
        {
            stream.Write(array, 0, array.Length);
        }


        public static StreamReader CreateTextReader(this Stream stream)
        {
            return new StreamReader(stream);
        }

        public static StreamWriter CreateTextWriter(this Stream stream)
        {
            return new StreamWriter(stream);
        }

        public static BinaryReader CreateBinaryReader(this Stream stream)
        {
            return new BinaryReader(stream);
        }

        public static BinaryWriter CreateBinaryWriter(this Stream stream)
        {
            return new BinaryWriter(stream);
        }

        public static NonClosingBinaryReader CreateNonClosingReader(this Stream stream)
        {
            return new NonClosingBinaryReader(stream);
        }

        public static NonClosingBinaryWriter CreateNonClosingWriter(this Stream stream)
        {
            return new NonClosingBinaryWriter(stream);
        }


        public static void WriteInt32(this Stream stream, int val)
        {
            stream.Write(BitConverter.GetBytes(val), 0, sizeof(int));
        }

        public static int ReadInt32(this Stream stream)
        {
            byte[] buff = new byte[sizeof(int)];

            stream.Read(buff, 0, sizeof(int));

            return BitConverter.ToInt32(buff, 0);
        }


        public static short ReadInt16(this Stream stream)
        {
            byte[] buff = new byte[sizeof(short)];

            stream.Read(buff, 0, sizeof(short));

            return BitConverter.ToInt16(buff, 0);
        }




        public static void ReadToBuffer(this Stream stream, byte[] buffer, int need)
        {
            if (buffer.Length < need)
            {
                throw new ArgumentOutOfRangeException("need");
            }

            int received = 0;

            while (received < need)
            {
                received += stream.Read(buffer, received, need - received);
            }
        }

    }
}
