using OpenTK.Mathematics;

namespace igbgui.Fields
{
    public class igVec3fMetaField : IgbStructField<Vector3>
    {
        public igVec3fMetaField(IgbObject parent, IgbObjectRef info, int index) : base(parent, info, index, BitUtils.ReadVec3f) { }
        public igVec3fMetaField(IgbEntity parent, IgbMemoryRef info, int offset) : base(parent, info, offset, BitUtils.ReadVec3f) { }

        public new static int Size => 12;
    }
}
