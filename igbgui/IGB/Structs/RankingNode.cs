using OpenTK.Mathematics;

namespace igbgui.Structs
{
    public struct RankingNode
    {
        public float Progress;
        public float Unk1;
        public Vector3 Pos;
        public float Unk2;
        public float Unk3;
        public Vector4 UnkVec;
        public int NodeID;

        public RankingNode(byte[] data, int offset)
        {
            Progress = BitUtils.ReadFloat(data, offset + 0);
            Unk1 = BitUtils.ReadFloat(data, offset + 4);
            Pos = BitUtils.ReadVec3f(data, offset + 8);
            Unk2 = BitUtils.ReadFloat(data, offset + 20);
            Unk3 = BitUtils.ReadFloat(data, offset + 24);
            UnkVec = BitUtils.ReadVec4f(data, offset + 28);
            NodeID = BitUtils.ReadInt(data, offset + 44);
        }
    }
}
