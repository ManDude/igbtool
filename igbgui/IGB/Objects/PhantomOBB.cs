using igbgui.Types;

namespace igbgui.Structs
{
    public class PhantomOBB : igNamedObject
    {
        public igVec4fMetaField Quat;
        public igVec3fMetaField Pos;
        public igVec3fMetaField Size;
        public PhantomOBB(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            Quat = new(this, info, 1);
            Pos = new(this, info, 2);
            Size = new(this, info, 3);
        }
    }
}
