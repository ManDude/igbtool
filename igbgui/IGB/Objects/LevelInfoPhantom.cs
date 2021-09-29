using igbgui.Types;

namespace igbgui.Structs
{
    public class LevelInfoPhantom : igObject
    {
        public igObjectRefMetaField<igObjectList<PhantomOBB>> OBBList1;
        public igObjectRefMetaField<igObjectList<PhantomOBB>> OBBList2;
        public igObjectRefMetaField<igObjectList<PhantomAABB>> AABBList;
        public igObjectRefMetaField<igObjectList<igObject>> SphereList;
        public igObjectRefMetaField<igObjectList<igObject>> QHullList;

        public LevelInfoPhantom(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            OBBList1 = new(this, info, 0);
            OBBList2 = new(this, info, 1);
            AABBList = new(this, info, 2);
            SphereList = new(this, info, 3);
            QHullList = new(this, info, 4);
        }
    }
}
