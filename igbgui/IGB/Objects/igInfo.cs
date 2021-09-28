using igbgui.Types;

namespace igbgui.Structs
{
    public class igInfo : igNamedObject
    {
        public igBoolMetaField Bool;
        public igInfo(IgbStruct s) : base(s)
        {
            Bool = new(this, 1);
        }
    }
}
