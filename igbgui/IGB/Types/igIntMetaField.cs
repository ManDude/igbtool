namespace igbgui.Types
{
    public class igIntMetaField : IgbField
    {
        public int Value { get => ReadInt(Parent.Data); set => Write(Parent.Data, value); }
        public igIntMetaField() { }
        public igIntMetaField(IgbObject parent, int index) : base(parent, index) { }
    }
}
