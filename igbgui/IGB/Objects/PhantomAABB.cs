using igbgui.Fields;

namespace igbgui.Objects
{
    public class PhantomAABB : igNamedObject
    {
        public igVec3fMetaField Pos;
        public igVec3fMetaField Size;
        public PhantomAABB(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            Pos = new(this, info, 1);
            Size = new(this, info, 2);
        }
    }
}
