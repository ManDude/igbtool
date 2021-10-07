using igbgui.Fields;

namespace igbgui.Objects
{
    public class LevelInfoTriggerCrates : igObject
    {
        public igObjectRefMetaField<igObjectList<TriggerCrateData>> CrateList;

        public LevelInfoTriggerCrates(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            CrateList = new(this, info, 0);
        }
    }
}
