namespace igbgui.Types
{
    public class igStringMetaField : IgbField
    {
        public string Value { get => ReadString(Parent.Data); set => Write(Parent.Data, value); }
        public igStringMetaField() { }
        public igStringMetaField(IgbObject parent, int index) : base(parent, index) { }
    }
}
