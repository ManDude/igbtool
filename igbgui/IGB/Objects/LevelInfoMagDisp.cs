using igbgui.Fields;

namespace igbgui.Objects
{
    public class LevelInfoMagDisp : igObject
    {
        public igObjectRefMetaField<igObjectList<MagGravSpline>> MagGravSplineList;

        public LevelInfoMagDisp(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            MagGravSplineList = new(this, info, 0);
        }
    }
}
