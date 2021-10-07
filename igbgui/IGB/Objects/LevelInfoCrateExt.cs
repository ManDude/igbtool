using igbgui.Fields;

namespace igbgui.Objects
{
    public class LevelInfoCrateExt : igObject
    {
        public igObjectRefMetaField<igIntList> CrateExtList;

        public LevelInfoCrateExt(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            CrateExtList = new(this, info, 0);
        }
    }
}
