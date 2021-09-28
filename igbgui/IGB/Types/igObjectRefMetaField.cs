namespace igbgui.Types
{
    public class igObjectRefMetaField<T> : IgbField where T : IgbObject
    {
        public T Value { get; set; }
        public igObjectRefMetaField() { }
        public igObjectRefMetaField(IgbObject parent, int index) : base(parent, index) { }

        public void GetVal() => Value = ReadObjRef() as T;
        public void SetVal() => Write(Value);
    }
}
