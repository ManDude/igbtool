using OpenTK.Mathematics;

namespace igbgui.Structs
{
    public struct RankingNode
    {
        public float Progress;
        public Vector3 Pos;
        public Vector3 Vec;
        public Vector4 Rot;
        public int NodeID;

        public RankingNode(byte[] data, int offset)
        {
            Progress = BitUtils.ReadFloat(data, offset + 0);
            Pos = BitUtils.ReadVec3f(data, offset + 4);
            Vec = BitUtils.ReadVec3f(data, offset + 16);
            Rot = BitUtils.ReadVec4f(data, offset + 28);
            NodeID = BitUtils.ReadInt(data, offset + 44);
        }
    }
}
