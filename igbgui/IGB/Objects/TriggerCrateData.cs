using igbgui.Fields;

namespace igbgui.Objects
{
    public class TriggerCrateData : igObject
    {
        public igIntMetaField Int;
        public igVec3fMetaField Pos;
        public igVec4fMetaField Rot;
        public igStringMetaField TargetNames;

        public TriggerCrateData(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            Int = new(this, info, 0);
            Pos = new(this, info, 1);
            Rot = new(this, info, 2);
            TargetNames = new(this, info, 3);
        }
    }
}
