using igbgui.Fields;

namespace igbgui.Objects
{
    public class LevelInfoCrystal : igObject
    {
        public igObjectRefMetaField<CrystalDataList> CrystalList;

        public LevelInfoCrystal(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            CrystalList = new(this, info, 0);
        }
    }
}
