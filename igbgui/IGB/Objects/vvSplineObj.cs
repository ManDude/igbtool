using igbgui.Types;

namespace igbgui.Structs
{
    public class vvSplineObj : igNode
    {
        public igObjectRefMetaField<igVec3fList> Spline;
        public vvSplineObj(IgbStruct s) : base(s)
        {
            Spline = new(this, 3);
        }
    }
}
