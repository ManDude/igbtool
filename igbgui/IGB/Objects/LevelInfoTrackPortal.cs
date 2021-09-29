using igbgui.Fields;

namespace igbgui.Objects
{
    public class LevelInfoTrackPortal : igObject
    {
        public igObjectRefMetaField<igObjectList<TrackPortal>> PortalList;

        public LevelInfoTrackPortal(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            PortalList = new(this, info, 0);
        }
    }
}
