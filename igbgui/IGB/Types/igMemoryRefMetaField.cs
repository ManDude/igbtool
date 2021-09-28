namespace igbgui.Types
{
    public class igMemoryRefMetaField : IgbField
    {
        public IgbMemory Value { get; set; }
        public igMemoryRefMetaField() { }
        public igMemoryRefMetaField(IgbObject parent, int index) : base(parent, index) { }

        public void GetVal() => Value = ReadMemRef();
        public void SetVal() => Write(Value);
    }
}
