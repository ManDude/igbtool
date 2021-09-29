using igbgui.Fields;

namespace igbgui.Objects
{
    public class vvSplineObj : igNode
    {
        public igObjectRefMetaField<igVec3fList> Spline;
        public vvSplineObj(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            Spline = new(this, info, 3);
        }
    }
}
