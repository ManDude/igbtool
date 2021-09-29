using igbgui.Fields;

namespace igbgui.Objects
{
    public class LevelInfoKartStart : igObject
    {
        public igObjectRefMetaField<igVec3fList> PosList;
        public igObjectRefMetaField<igVec4fList> RotList;

        public LevelInfoKartStart(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            PosList = new(this, info, 0);
            RotList = new(this, info, 1);
        }
    }
}
