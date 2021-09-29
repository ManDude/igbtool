using igbgui.Fields;
using igbgui.Structs;
using System.Collections.Generic;

namespace igbgui.Objects
{
    public class MagGravNodeList : igDataList<MagGravNodeMetaField>, IigList<MagGravNode>
    {
        public MagGravNodeList(IGB igb, IgbObjectRef info) : base(igb, info)
        {
        }

        public new List<MagGravNode> GetList()
        {
            var list = new List<MagGravNode>();
            foreach (var v in DataList.Value.Data)
            {
                list.Add(v.Value);
            }
            return list;
        }
        public new MagGravNode At(int index)
        {
            return DataList.Value.Data[index].Value;
        }
    }
}
