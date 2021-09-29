using igbgui.Fields;

namespace igbgui.Objects
{
    public class MagGravSpline : igObject
    {
        public igEnumMetaField Enum;
        public igIntMetaField Int;
        public igObjectRefMetaField<MagGravNodeList> NodeList;
        public MagGravSpline(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            Enum = new(this, info, 0);
            Int = new(this, info, 1);
            NodeList = new(this, info, 2);
        }
    }
}
