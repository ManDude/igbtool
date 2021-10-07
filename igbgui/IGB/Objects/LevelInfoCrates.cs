using igbgui.Fields;

namespace igbgui.Objects
{
    public class LevelInfoCrates : igObject
    {
        public igObjectRefMetaField<CrateDataList> CrateList;

        public LevelInfoCrates(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            CrateList = new(this, info, 0);
        }
    }
}
