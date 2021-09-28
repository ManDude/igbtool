using System;
using System.Text;

namespace igbgui
{
    public static class BitUtils
    {
        public static int Align(int val, int n)
        {
            int res = val + (n - 1);
            return res - res % n;
        }

        public static void WriteBytes(byte[] data, int offset, bool val) => Array.Copy(BitConverter.GetBytes(val), 0, data, offset, 1);
        public static void WriteBytes(byte[] data, int offset, byte val) => Array.Copy(BitConverter.GetBytes(val), 0, data, offset, 1);
        public static void WriteBytes(byte[] data, int offset, sbyte val) => Array.Copy(BitConverter.GetBytes(val), 0, data, offset, 1);
        public static void WriteBytes(byte[] data, int offset, short val) => Array.Copy(BitConverter.GetBytes(val), 0, data, offset, 2);
        public static void WriteBytes(byte[] data, int offset, ushort val) => Array.Copy(BitConverter.GetBytes(val), 0, data, offset, 2);
        public static void WriteBytes(byte[] data, int offset, int val) => Array.Copy(BitConverter.GetBytes(val), 0, data, offset, 4);
        public static void WriteBytes(byte[] data, int offset, uint val) => Array.Copy(BitConverter.GetBytes(val), 0, data, offset, 4);
        public static void WriteBytes(byte[] data, int offset, long val) => Array.Copy(BitConverter.GetBytes(val), 0, data, offset, 8);
        public static void WriteBytes(byte[] data, int offset, ulong val) => Array.Copy(BitConverter.GetBytes(val), 0, data, offset, 8);
        public static void WriteBytes(byte[] data, int offset, float val) => Array.Copy(BitConverter.GetBytes(val), 0, data, offset, 4);
        public static void WriteBytes(byte[] data, int offset, double val) => Array.Copy(BitConverter.GetBytes(val), 0, data, offset, 8);
        public static void WriteBytes(byte[] data, int offset, string val) => Array.Copy(Encoding.UTF8.GetBytes(val), 0, data, offset, Encoding.UTF8.GetByteCount(val));
    }
}
