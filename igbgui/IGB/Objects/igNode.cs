using igbgui.Types;

namespace igbgui.Structs
{
    public class igNode : igNamedObject
    {
        public igObjectRefMetaField<igObject> Obj;
        public igIntMetaField Int;
        public igNode(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            Obj = new(this, info, 1);
            Int = new(this, info, 2);
        }
    }
}
