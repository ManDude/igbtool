using igbgui.Structs;
using OpenTK.Mathematics;
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

        public static void Write(byte[] dest, int offset, bool val) => Array.Copy(BitConverter.GetBytes(val), 0, dest, offset, 1);
        public static void Write(byte[] dest, int offset, byte val) => Array.Copy(BitConverter.GetBytes(val), 0, dest, offset, 1);
        public static void Write(byte[] dest, int offset, sbyte val) => Array.Copy(BitConverter.GetBytes(val), 0, dest, offset, 1);
        public static void Write(byte[] dest, int offset, short val) => Array.Copy(BitConverter.GetBytes(val), 0, dest, offset, 2);
        public static void Write(byte[] dest, int offset, ushort val) => Array.Copy(BitConverter.GetBytes(val), 0, dest, offset, 2);
        public static void Write(byte[] dest, int offset, int val) => Array.Copy(BitConverter.GetBytes(val), 0, dest, offset, 4);
        public static void Write(byte[] dest, int offset, uint val) => Array.Copy(BitConverter.GetBytes(val), 0, dest, offset, 4);
        public static void Write(byte[] dest, int offset, long val) => Array.Copy(BitConverter.GetBytes(val), 0, dest, offset, 8);
        public static void Write(byte[] dest, int offset, ulong val) => Array.Copy(BitConverter.GetBytes(val), 0, dest, offset, 8);
        public static void Write(byte[] dest, int offset, float val) => Array.Copy(BitConverter.GetBytes(val), 0, dest, offset, 4);
        public static void Write(byte[] dest, int offset, double val) => Array.Copy(BitConverter.GetBytes(val), 0, dest, offset, 8);
        public static void Write(byte[] dest, int offset, string val) => Array.Copy(Encoding.UTF8.GetBytes(val), 0, dest, offset, Encoding.UTF8.GetByteCount(val));

        public static bool ReadBool(byte[] data, int offset) => BitConverter.ToBoolean(data, offset);
        public static int ReadInt(byte[] data, int offset) => BitConverter.ToInt32(data, offset);
        public static float ReadFloat(byte[] data, int offset) => BitConverter.ToSingle(data, offset);
        public static string ReadString(byte[] data, int offset) => Encoding.UTF8.GetString(data, offset + 4, ReadInt(data, offset));
        public static Vector3 ReadVec3f(byte[] data, int offset) => new(ReadFloat(data, offset + 0), ReadFloat(data, offset + 4), ReadFloat(data, offset + 8));
        public static Vector4 ReadVec4f(byte[] data, int offset) => new(ReadFloat(data, offset + 0), ReadFloat(data, offset + 4), ReadFloat(data, offset + 8), ReadFloat(data, offset + 12));
        public static CrystalData ReadCrystalData(byte[] data, int offset) => new(data, offset);
        public static CrateData ReadCrateData(byte[] data, int offset) => new(data, offset);
        public static CNKLetterData ReadCNKLetterData(byte[] data, int offset) => new(data, offset);
        public static MagGravNode ReadMagGravNode(byte[] data, int offset) => new(data, offset);
        public static RestartPointData ReadRestartPointData(byte[] data, int offset) => new(data, offset);
        public static RankingNode ReadRankingNode(byte[] data, int offset) => new(data, offset);
    }
}
