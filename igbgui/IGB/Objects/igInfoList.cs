using igbgui.Types;

namespace igbgui.Structs
{
    public class igInfoList : igObjectList<LevelInfo>
    {
        public igInfoList(IGB igb, IgbObjectRef info) : base(igb, info)
        {
        }
    }
}
