using OpenTK.Mathematics;

namespace igbgui.Structs
{
    public struct CrateData
    {
        public int Val;
        public Vector3 Pos;
        public Vector4 Rot;

        public CrateData(byte[] data, int offset)
        {
            Val = BitUtils.ReadInt(data, offset + 0);
            Pos = BitUtils.ReadVec3f(data, offset + 4);
            Rot = BitUtils.ReadVec4f(data, offset + 16);
        }
    }
}
