using igbgui.Structs;

namespace igbgui.Fields
{
    public class RankingNodeMetaField : IgbStructField<RankingNode>
    {
        public RankingNodeMetaField(IgbObject parent, IgbObjectRef info, int index) : base(parent, info, index, BitUtils.ReadRankingNode) { }
        public RankingNodeMetaField(IgbEntity parent, IgbMemoryRef info, int offset) : base(parent, info, offset, BitUtils.ReadRankingNode) { }

        public new static int Size => 48;
    }
}
