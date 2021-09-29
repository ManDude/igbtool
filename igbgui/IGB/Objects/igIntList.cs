using igbgui.Fields;
using System.Collections.Generic;

namespace igbgui.Objects
{
    public class igIntList : igDataList<igIntMetaField>, IigList<int>
    {
        public igIntList(IGB igb, IgbObjectRef info) : base(igb, info)
        {
        }

        public new List<int> GetList()
        {
            var list = new List<int>();
            foreach (var v in DataList.Value.Data)
            {
                list.Add(v.Value);
            }
            return list;
        }
        public new int At(int index)
        {
            return DataList.Value.Data[index].Value;
        }
    }
}
