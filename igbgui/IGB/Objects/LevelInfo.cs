using igbgui.Types;

namespace igbgui.Structs
{
    public class LevelInfo : igInfo
    {
        public igObjectRefMetaField<igObjectList<igObject>> Obj;
        public LevelInfo(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            Obj = new(this, info, 2);
        }
    }
}
