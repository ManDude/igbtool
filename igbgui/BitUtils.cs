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

        public static void WriteBytes(byte[] data, int offset, int val) => Array.Copy(BitConverter.GetBytes(val), 0, data, offset, 4);
        public static void WriteBytes(byte[] data, int offset, float val) => Array.Copy(BitConverter.GetBytes(val), 0, data, offset, 4);
        public static void WriteBytes(byte[] data, int offset, string val) => Array.Copy(Encoding.UTF8.GetBytes(val), 0, data, offset, Encoding.UTF8.GetByteCount(val));
    }
}
