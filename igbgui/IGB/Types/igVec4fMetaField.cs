using OpenTK.Mathematics;

namespace igbgui.Types
{
    public class igVec4fMetaField : IgbField
    {
        public Vector4 Value { get => ReadVec4f(Parent.Data); set => Write(Parent.Data, value); }
        public igVec4fMetaField() { }
        public igVec4fMetaField(IgbObject parent, int index) : base(parent, index) { }
    }
}
