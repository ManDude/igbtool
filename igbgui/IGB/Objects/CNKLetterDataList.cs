using igbgui.Fields;
using igbgui.Structs;
using System.Collections.Generic;

namespace igbgui.Objects
{
    public class CNKLetterDataList : igDataList<CNKLetterDataMetaField>, IigList<CNKLetterData>
    {
        public CNKLetterDataList(IGB igb, IgbObjectRef info) : base(igb, info)
        {
        }

        public new List<CNKLetterData> GetList()
        {
            var list = new List<CNKLetterData>();
            foreach (var v in DataList.Value.Data)
            {
                list.Add(v.Value);
            }
            return list;
        }
        public new CNKLetterData At(int index)
        {
            return DataList.Value.Data[index].Value;
        }
    }
}
