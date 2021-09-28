using igbgui.Types;

namespace igbgui.Structs
{
    public class igNode : igNamedObject
    {
        public igObjectRefMetaField<igObject> Obj;
        public igIntMetaField Int;
        public igNode(IgbStruct s) : base(s)
        {
            Obj = new(this, 1);
            Int = new(this, 2);
        }
    }
}
