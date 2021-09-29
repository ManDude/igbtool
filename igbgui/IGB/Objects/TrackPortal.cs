using igbgui.Types;

namespace igbgui.Structs
{
    public class TrackPortal : igObject
    {
        public igVec3fMetaField Pos;
        public igVec3fMetaField Size;
        public igVec4fMetaField Rot;
        public igBoolMetaField Bool1;
        public igIntMetaField Int1;
        public igBoolMetaField Bool2;
        public igIntMetaField Int2;
        public igObjectRefMetaField<igIntList> IntList;

        public TrackPortal(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            Pos = new(this, info, 0);
            Size = new(this, info, 1);
            Rot = new(this, info, 2);
            Bool1 = new(this, info, 3);
            Int1 = new(this, info, 4);
            Bool2 = new(this, info, 5);
            Int2 = new(this, info, 6);
            IntList = new(this, info, 7);
        }
    }
}
