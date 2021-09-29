namespace igbgui.Fields
{
    public class igBoolMetaField : IgbStructField<bool>
    {
        public igBoolMetaField(IgbObject parent, IgbObjectRef info, int index) : base(parent, info, index, BitUtils.ReadBool) { }
        public igBoolMetaField(IgbEntity parent, IgbMemoryRef info, int offset) : base(parent, info, offset, BitUtils.ReadBool) { }

        public new static int Size => 1;
    }
}
