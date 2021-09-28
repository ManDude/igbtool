using igbgui.Types;

namespace igbgui.Structs
{
    public class PhantomOBB : igNamedObject
    {
        public igVec4fMetaField Quat;
        public igVec3fMetaField Pos;
        public igVec3fMetaField Size;
        public PhantomOBB(IgbStruct s) : base(s)
        {
            Quat = new(this, 1);
            Pos = new(this, 2);
            Size = new(this, 3);
        }
    }
}
