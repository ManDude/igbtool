using OpenTK.Mathematics;

namespace igbgui.Structs
{
    public struct MagGravNode
    {
        public Vector3 Pos;
        public Vector3 Grav;

        public MagGravNode(byte[] data, int offset)
        {
            Pos = BitUtils.ReadVec3f(data, offset+0);
            Grav = BitUtils.ReadVec3f(data, offset+12);
        }
    }
}
