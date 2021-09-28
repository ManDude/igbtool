namespace igbgui.Types
{
    public class igIntMetaField : IgbField
    {
        public int Value { get => ReadInt(); set => Write(value); }
        public igIntMetaField() { }
        public igIntMetaField(IgbObject parent, int index) : base(parent, index) { }
    }
}
