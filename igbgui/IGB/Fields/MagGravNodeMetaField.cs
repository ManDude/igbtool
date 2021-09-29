using igbgui.Structs;

namespace igbgui.Fields
{
    public class MagGravNodeMetaField : IgbStructField<MagGravNode>
    {
        public MagGravNodeMetaField(IgbObject parent, IgbObjectRef info, int index) : base(parent, info, index, BitUtils.ReadMagGravNode) { }
        public MagGravNodeMetaField(IgbEntity parent, IgbMemoryRef info, int offset) : base(parent, info, offset, BitUtils.ReadMagGravNode) { }

        public new static int Size => 24;
    }
}
