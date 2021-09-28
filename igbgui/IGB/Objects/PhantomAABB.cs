using igbgui.Types;

namespace igbgui.Structs
{
    public class PhantomAABB : igNamedObject
    {
        public igVec3fMetaField Pos;
        public igVec3fMetaField Size;
        public PhantomAABB(IgbStruct s) : base(s)
        {
            Pos = new(this, 1);
            Size = new(this, 2);
        }
    }
}
