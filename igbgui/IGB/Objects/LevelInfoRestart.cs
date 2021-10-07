using igbgui.Fields;

namespace igbgui.Objects
{
    public class LevelInfoRestart : igObject
    {
        public igObjectRefMetaField<RestartPointDataList> RestartList;

        public LevelInfoRestart(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            RestartList = new(this, info, 0);
        }
    }
}
