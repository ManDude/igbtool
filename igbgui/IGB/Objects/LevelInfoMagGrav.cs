using igbgui.Fields;

namespace igbgui.Objects
{
    public class LevelInfoMagGrav : igObject
    {
        public igObjectRefMetaField<igObjectList<MagGravSpline>> MagGravSplineList;

        public LevelInfoMagGrav(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            MagGravSplineList = new(this, info, 0);
        }
    }
}
