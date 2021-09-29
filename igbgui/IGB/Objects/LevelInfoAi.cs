using igbgui.Fields;

namespace igbgui.Objects
{
    public class LevelInfoAi : igObject
    {
        public igObjectRefMetaField<igObjectList<vvSplineObj>> SplineList;

        public LevelInfoAi(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            SplineList = new(this, info, 0);
        }
    }
}
