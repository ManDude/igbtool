using igbgui.Fields;

namespace igbgui.Objects
{
    public class RankingSegmentList : igObjectList<RankingSegment>, IigList<RankingSegment>
    {
        public igIntMetaField UnknownID;

        public RankingSegmentList(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            UnknownID = new(this, info, 3);
        }
    }
}
