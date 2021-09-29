namespace igbgui.Fields
{
    public class igEnumMetaField : IgbStructField<int>
    {
        public igEnumMetaField(IgbObject parent, IgbObjectRef info, int index) : base(parent, info, index, BitUtils.ReadInt) { }
        public igEnumMetaField(IgbEntity parent, IgbMemoryRef info, int offset) : base(parent, info, offset, BitUtils.ReadInt) { }

        public new static int Size => 4;
    }
}
