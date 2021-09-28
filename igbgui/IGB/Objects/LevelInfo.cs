using igbgui.Types;

namespace igbgui.Structs
{
    public class LevelInfo : igInfo
    {
        public igObjectRefMetaField<igObjectList> Obj;
        public LevelInfo(IgbStruct s) : base(s)
        {
            Obj = new(this, 2);
        }
    }
}
