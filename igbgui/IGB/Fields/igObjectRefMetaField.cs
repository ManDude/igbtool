using igbgui.Objects;

namespace igbgui.Fields
{
    public class igObjectRefMetaField<T> : IgbClassField<T> where T : igObject
    {
        public igObjectRefMetaField(IgbObject parent, IgbObjectRef info, int index) : base(parent, info, index, null)
        {
            Value = parent.IGB.GetRef<T>(BitUtils.ReadInt(info.Data, GetOffset(info.Data)));
        }
        public igObjectRefMetaField(IgbEntity parent, IgbMemoryRef info, int offset) : base(parent, info, offset, null)
        {
            Value = parent.IGB.GetRef<T>(BitUtils.ReadInt(info.Data, offset));
        }

        public new static int Size => 4;
    }
}
