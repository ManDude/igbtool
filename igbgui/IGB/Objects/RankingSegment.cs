using igbgui.Fields;

namespace igbgui.Objects
{
    public class RankingSegment : igObject
    {
        public igIntMetaField ID1;
        public igIntMetaField ID2;
        public igObjectRefMetaField<RankingNodeList> NodeList;

        public RankingSegment(IGB igb, IgbObjectRef info) : base(igb, info)
        {
            ID1 = new(this, info, 0);
            ID2 = new(this, info, 1);
            NodeList = new(this, info, 2);
        }
    }
}
