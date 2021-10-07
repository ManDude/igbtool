using igbgui.Fields;

namespace igbgui.Objects
{
    public class LevelInfoRanking : igObject
    {
        public igObjectRefMetaField<RankingSegmentList> SegmentList;

        public LevelInfoRanking(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            SegmentList = new(this, info, 0);
        }
    }
}
