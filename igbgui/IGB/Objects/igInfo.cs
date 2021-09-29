using igbgui.Fields;

namespace igbgui.Objects
{
    public class igInfo : igNamedObject
    {
        public igBoolMetaField Bool;
        public igInfo(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            Bool = new(this, info, 1);
        }
    }
}
