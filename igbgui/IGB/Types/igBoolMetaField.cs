namespace igbgui.Types
{
    public class igBoolMetaField : IgbField
    {
        public bool Value { get => ReadBool(); set => Write(value); }
        public igBoolMetaField() { }
        public igBoolMetaField(IgbObject parent, int index) : base(parent, index) { }
    }
}
