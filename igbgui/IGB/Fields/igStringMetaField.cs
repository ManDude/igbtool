namespace igbgui.Fields
{
    public class igStringMetaField : IgbClassField<string>
    {
        public igStringMetaField(IgbObject parent, IgbObjectRef info, int index) : base(parent, info, index, BitUtils.ReadString) { }
        public igStringMetaField(IgbEntity parent, IgbMemoryRef info, int offset) : base(parent, info, offset, BitUtils.ReadString) { }

        public new static int Size => 4;
    }
}
