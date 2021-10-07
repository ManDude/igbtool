using OpenTK.Mathematics;

namespace igbgui.Structs
{
    public struct CNKLetterData
    {
        public int Letter;
        public Vector3 Pos;
        public Vector4 Rot;

        public CNKLetterData(byte[] data, int offset)
        {
            Letter = BitUtils.ReadInt(data, offset + 0);
            Pos = BitUtils.ReadVec3f(data, offset + 4);
            Rot = BitUtils.ReadVec4f(data, offset + 16);
        }
    }
}
