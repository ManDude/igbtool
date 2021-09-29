using igbgui.Fields;

namespace igbgui.Objects
{
    public class LevelInfoCamera : igObject
    {
        public igObjectRefMetaField<vvSplineObj> Spline;

        public LevelInfoCamera(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            Spline = new(this, info, 0);
        }
    }
}
