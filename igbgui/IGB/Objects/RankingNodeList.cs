using igbgui.Fields;
using igbgui.Structs;
using System.Collections.Generic;

namespace igbgui.Objects
{
    public class RankingNodeList : igDataList<RankingNodeMetaField>, IigList<RankingNode>
    {
        public RankingNodeList(IGB igb, IgbObjectRef info) : base(igb, info)
        {
        }

        public new List<RankingNode> GetList()
        {
            var list = new List<RankingNode>();
            foreach (var v in DataList.Value.Data)
            {
                list.Add(v.Value);
            }
            return list;
        }
        public new RankingNode At(int index)
        {
            return DataList.Value.Data[index].Value;
        }
    }
}
