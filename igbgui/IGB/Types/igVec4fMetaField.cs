using OpenTK.Mathematics;

namespace igbgui.Types
{
    public class igVec4fMetaField : IgbStructField<Vector4>
    {
        public igVec4fMetaField(IgbObject parent, IgbObjectRef info, int index) : base(parent, info, index, BitUtils.ReadVec4f) { }
        public igVec4fMetaField(IgbEntity parent, IgbMemoryRef info, int offset) : base(parent, info, offset, BitUtils.ReadVec4f) { }

        public new static int Size => 16;
    }
}
