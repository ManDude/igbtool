namespace igbgui.Types
{
    public class igStringMetaField : IgbField
    {
        public string Value { get => ReadString(); set => Write(value); }
        public igStringMetaField() { }
        public igStringMetaField(IgbObject parent, int index) : base(parent, index) { }
    }
}
