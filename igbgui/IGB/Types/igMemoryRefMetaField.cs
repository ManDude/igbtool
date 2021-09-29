namespace igbgui.Types
{
    public class igMemoryRefMetaField<T> : IgbClassField<IgbMemory<T>> where T : IgbField
    {
        public igMemoryRefMetaField(IgbObject parent, IgbObjectRef info, int index) : base(parent, info, index, null)
        {
            Value = parent.IGB.GetRef<IgbMemory<T>>(BitUtils.ReadInt(info.Data, GetOffset(info.Data)));
        }
        public igMemoryRefMetaField(IgbEntity parent, IgbMemoryRef info, int offset) : base(parent, info, offset, null)
        {
            Value = parent.IGB.GetRef<IgbMemory<T>>(BitUtils.ReadInt(info.Data, offset));
        }

        public new static int Size => 4;
    }
}
