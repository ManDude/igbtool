using OpenTK.Mathematics;

namespace igbgui.Structs
{
    public struct CrystalData
    {
        public Vector3 Pos;
        public Vector4 Rot;
        public int Val;

        public CrystalData(byte[] data, int offset)
        {
            Pos = BitUtils.ReadVec3f(data, offset + 0);
            Rot = BitUtils.ReadVec4f(data, offset + 12);
            Val = BitUtils.ReadInt(data, offset + 28);
        }
    }
}
