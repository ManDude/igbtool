namespace igbgui.Fields
{
    public class igIntMetaField : IgbStructField<int>
    {
        public igIntMetaField(IgbObject parent, IgbObjectRef info, int index) : base(parent, info, index, BitUtils.ReadInt) { }
        public igIntMetaField(IgbEntity parent, IgbMemoryRef info, int offset) : base(parent, info, offset, BitUtils.ReadInt) { }

        public new static int Size => 4;
    }
}
