using OpenTK.Mathematics;

namespace igbgui.Types
{
    public class igVec3fMetaField : IgbField
    {
        public Vector3 Value { get => ReadVec3f(Parent.Data); set => Write(Parent.Data, value); }
        public igVec3fMetaField() { }
        public igVec3fMetaField(IgbObject parent, int index) : base(parent, index) { }
    }
}
